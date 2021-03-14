using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{

    public class BinderCache
    {
        public static BinderCache Current = new BinderCache();

        private readonly ConcurrentDictionary<string, SetMemberBinder> _setMemberBinders = new ConcurrentDictionary<string, SetMemberBinder>();
        private readonly ConcurrentDictionary<string, GetMemberBinder> _getMemberBinders = new ConcurrentDictionary<string, GetMemberBinder>();
        private readonly ConcurrentDictionary<int, SetIndexBinder> _setIndexBinders = new ConcurrentDictionary<int, SetIndexBinder>();
        private readonly ConcurrentDictionary<int, GetIndexBinder> _getIndexBinders = new ConcurrentDictionary<int, GetIndexBinder>();
        private readonly ConcurrentDictionary<int, InvokeBinder> _invokeBinders = new ConcurrentDictionary<int, InvokeBinder>();
        private readonly ConcurrentDictionary<Type, ConvertBinder> _convertBinders = new ConcurrentDictionary<Type, ConvertBinder>();
        private readonly ConcurrentDictionary<ExpressionType, BinaryOperationBinder> _binaryOperationBinders = new ConcurrentDictionary<ExpressionType, BinaryOperationBinder>(); 
        private readonly ConcurrentDictionary<ExpressionType, UnaryOperationBinder> _unaryOperationBinders = new ConcurrentDictionary<ExpressionType, UnaryOperationBinder>();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, InvokeMemberBinder>> _invokeMemberBinders = new ConcurrentDictionary<string, ConcurrentDictionary<int, InvokeMemberBinder>>(); 
        private readonly ConcurrentDictionary<int, CreateInstanceBinder> _createInstanceBinders = new ConcurrentDictionary<int, CreateInstanceBinder>();
        private readonly ConcurrentDictionary<string, DynamicMetaObjectBinder> _hasBinders = new ConcurrentDictionary<string, DynamicMetaObjectBinder>();
        private readonly DynamicMetaObjectBinder _isOperationBinder = new ScriptIsOperationBinder();
        private readonly DynamicMetaObjectBinder _extendsOperationBinder = new ScriptExtendsOperationBinder();
        private readonly DynamicMetaObjectBinder _getMembersBinder = new ScriptGetMembersBinder();

        public SetMemberBinder SetMemberBinder(string name)
        {
            if (!_setMemberBinders.TryGetValue(name, out SetMemberBinder binder))
            {
                _setMemberBinders[name] = binder = new ScriptSetMemberBinder(name);
            }

            return binder;
        }

        public GetMemberBinder GetMemberBinder(string name)
        {
            if (!_getMemberBinders.TryGetValue(name, out GetMemberBinder binder))
            {
                _getMemberBinders[name] = binder = new ScriptGetMemberBinder(name);
            }

            return binder;
        }

        public SetIndexBinder SetIndexBinder(int argCount)
        {
            if (!_setIndexBinders.TryGetValue(argCount, out SetIndexBinder binder))
            {
                _setIndexBinders[argCount] = binder = new ScriptSetIndexBinder(argCount);
            }

            return binder;
        }

        public GetIndexBinder GetIndexBinder(int argCount)
        {
            if (!_getIndexBinders.TryGetValue(argCount, out GetIndexBinder binder))
            {
                _getIndexBinders[argCount] = binder = new ScriptGetIndexBinder(argCount);
            }

            return binder;
        }

        public InvokeBinder InvokeBinder(int argCount)
        {
            if (!_invokeBinders.TryGetValue(argCount, out InvokeBinder binder))
            {
                _invokeBinders[argCount] = binder = new ScriptInvokeBinder(argCount);
            }

            return binder;
        }

        public ConvertBinder ConvertBinder(Type type)
        {
            if (!_convertBinders.TryGetValue(type, out ConvertBinder binder))
            {
                _convertBinders[type] = binder = new ScriptConvertBinder(type);
            }

            return binder;
        }

        public BinaryOperationBinder BinaryOperationBinder(ExpressionType operation)
        {
            if (!_binaryOperationBinders.TryGetValue(operation, out BinaryOperationBinder binder))
            {
                _binaryOperationBinders[operation] = binder = new ScriptBinaryOperationBinder(operation);
            }

            return binder;
        }

        public InvokeMemberBinder InvokeMemberBinder(string name, int argCount)
        {
            if (!_invokeMemberBinders.TryGetValue(name, out ConcurrentDictionary<int, InvokeMemberBinder> argCounts))
            {
                _invokeMemberBinders[name] = argCounts = new ConcurrentDictionary<int, InvokeMemberBinder>();
            }

            if (!argCounts.TryGetValue(argCount, out InvokeMemberBinder binder))
            {
                argCounts[argCount] = binder = new ScriptInvokeMemberBinder(name, argCount);
            }

            return binder;
        }

        public CreateInstanceBinder CreateInstanceBinder(int argCount)
        {
            if (!_createInstanceBinders.TryGetValue(argCount, out CreateInstanceBinder binder))
            {
                _createInstanceBinders[argCount] = binder = new ScriptCreateInstanceBinder(argCount);
            }

            return binder;
        }

        public DynamicMetaObjectBinder HasOperationBinder(string name)
        {
            if (!_hasBinders.TryGetValue(name, out DynamicMetaObjectBinder binder))
            {
                _hasBinders[name] = binder = new ScriptHasOperationBinder(name);
            }

            return binder;
        }

        public DynamicMetaObjectBinder IsOperationBinder()
        {
            return _isOperationBinder;
        }

        public DynamicMetaObjectBinder ExtendsOperationBinder()
        {
            return _extendsOperationBinder;
        }

        public DynamicMetaObjectBinder GetMembersBinder()
        {
            return _getMembersBinder;
        }
    }
}
