using FryScript.Ast;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FryScript.Compilation
{
    public class Scope
    {
        private readonly Dictionary<string, ParameterExpression> _members;
        private readonly HashSet<string> _localMembers = new HashSet<string>();
        private readonly HashSet<string> _hoistedMembers = new HashSet<string>();
        private readonly Dictionary<string, object> _dataBag;
        private readonly Dictionary<string, int> _tempNames;

        public bool Hoisted { get; }
        public Scope Parent { get; }
        public bool IsRoot => Parent == null;
        public Scope()
            : this(new Dictionary<string, ParameterExpression>(), 
                  new Dictionary<string, object>(),
                  new Dictionary<string, int>(),
                  null, 
                  false)
        {
        }

        private Scope(Dictionary<string, ParameterExpression> members, 
            Dictionary<string, object> dataBag, 
            Dictionary<string, int> tempNames,
            Scope parent, bool hoisted)
        {
            _members = members;
            _dataBag = dataBag;
            _tempNames = tempNames;
            Parent = parent;
            Hoisted = hoisted;
        }

        public Scope New(bool resetDataBag = false, bool? hoisted = null)
        {
            return new Scope(
                new Dictionary<string, ParameterExpression>(_members), 
                resetDataBag ? new Dictionary<string, object>() : new Dictionary<string, object>(_dataBag),
                _tempNames,
                this, 
                hoisted ?? Hoisted);
        }

        public Scope Clone(bool resetDataBag = false, bool? hoisted = null)
        {
            return new Scope(
                _members,
                resetDataBag ? new Dictionary<string, object>() : new Dictionary<string, object>(_dataBag),
                _tempNames,
                Parent,
                hoisted ?? Hoisted);
        }

        public ParameterExpression AddMember(string name, AstNode astNode, Type type = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if(_members.ContainsKey(name))
                throw CompilerException.FromAst(string.Format("Identifier {0} has already been defined in the current scope", name), astNode);

            type = type ?? typeof(object);

            if (!_localMembers.Contains(name))
                _localMembers.Add(name);

            var parameter = Expression.Parameter(type, name);

            if (Hoisted)
            {
                var hoistedScope = GetHoistedScope(this);

                if (hoistedScope._hoistedMembers.Contains(name))
                    return hoistedScope._members[name];

                hoistedScope._hoistedMembers.Add(name);
                hoistedScope._members[name] = parameter;
            }

            return _members[name] = parameter;
        }

        public ParameterExpression AddTempMember(string prefix, AstNode astNode, Type type = null)
        {
            return AddMember(GetTempName(prefix), astNode, type);
        }

        public ParameterExpression AddKeywordMember<T>(string name, AstNode astNode)
        {
            if (!_localMembers.Contains(name))
                _localMembers.Add(name);

            return _members[name] = Expression.Parameter(typeof(T), name);
        }

        public bool HasLocalMember(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return _localMembers.Contains(name);
        }

        public bool HasMember(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if(Hoisted)
            {
                var hoistedScope = GetHoistedScope(this);
                return hoistedScope._members.ContainsKey(name);
            }

            return _members.ContainsKey(name);
        }

        public ParameterExpression GetMemberExpression(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (Hoisted)
            {
                var hoistedScope = GetHoistedScope(this);
                return hoistedScope._members[name];
            }

            return _members[name];
        }

        public IEnumerable<ParameterExpression> GetLocalExpressions()
        {
            if (Hoisted)
            {
                var rootHoist = GetHoistedScope(this);

                if (this != rootHoist)
                    yield break;

                foreach(var parameter in _members)
                {
                    if (!_hoistedMembers.Contains(parameter.Key))
                        continue;

                    yield return parameter.Value;
                }
            }
            else
            {
                foreach (var parameter in _members)
                {
                    if (!_localMembers.Contains(parameter.Key))
                        continue;

                    yield return parameter.Value;
                }
            }
        }

        public IEnumerable<ParameterExpression> GetExpressions()
        {
            return _members.Values;
        }

        public Expression ScopeBlock(params Expression[] expressions)
        {
            return Expression.Block(
                typeof (object),
                GetLocalExpressions(),
                expressions
                );
        }

        public Expression ScopeBlock(Type type, params Expression[] expressions)
        {
            return Expression.Block(
                type,
                GetLocalExpressions(),
                expressions
                );
        }

        public T SetData<T>(string name, T data)
        {
            _dataBag[name] = data;
            return data;
        }

        public bool TryGetData<T>(string name, out T data)
        {
            if (!_dataBag.TryGetValue(name, out object dataObj))
            {
                data = default(T);
                return false;
            }

            data = (T) dataObj;
            return true;
        }

        public bool HasData(string name)
        {
            return _dataBag.ContainsKey(name);
        }

        public void RemoveData(string name)
        {
            _dataBag.Remove(name);
        }

        public string GetTempName(string name)
        {
            if (!_tempNames.TryGetValue(name, out int count))
                _tempNames[name] = 0;

            return $"<>{name}_{_tempNames[name]++}";
        }

        private Scope GetHoistedScope(Scope scope)
        {
            while(scope != null && scope.Parent != null)
            {
                if (!scope.Parent.Hoisted)
                    return scope;

                scope = scope.Parent;
            }

            return scope;
        }
    }
}
