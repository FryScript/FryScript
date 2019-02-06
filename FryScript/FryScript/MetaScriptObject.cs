namespace FryScript
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Binders;
    using Helpers;
    using HostInterop;

    public class MetaScriptObject : ScriptMetaObjectBase
    {
        private static readonly MethodInfo NewMethodInfo = typeof(ScriptObject).GetMethod("New", BindingFlags.NonPublic | BindingFlags.Instance);

        public IScriptObject ScriptObject { get { return Value as IScriptObject; } }

        public Expression ScriptObjectExpr { get { return Expression.Convert(Expression, Value.GetType()); } }

        public Expression ScriptObjectTargetExpr { get { return Expression.PropertyOrField(Expression.Convert(Expression, typeof(IScriptObject)), nameof(IScriptObject.Target)); } }

        public MetaScriptObject(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            if (TypeProvider.Current.TryGetProperty(LimitType, binder.Name, out PropertyInfo propertyInfo))
            {
                var propertyExpr = Expression.Property(ScriptObjectExpr, propertyInfo);
                var setPropertyExpr = Expression.Assign(propertyExpr,
                    ExpressionHelper.DynamicConvert(value.Expression, propertyInfo.PropertyType));
                var convertSetPropertyExpr = Expression.Convert(setPropertyExpr, typeof(object));

                return new DynamicMetaObject(convertSetPropertyExpr, GetDefaultRestrictions());
            }

            if (ScriptObject.HasTarget && TypeProvider.Current.TryGetProperty(ScriptObject.TargetType, binder.Name, out propertyInfo))
            {
                var propertyExpr = Expression.Property(
                    Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType),
                    propertyInfo);
                var setPropertyExpr = Expression.Assign(propertyExpr,
                    ExpressionHelper.DynamicConvert(value.Expression, propertyInfo.PropertyType));
                var convertSetPropertyExpr = Expression.Convert(setPropertyExpr, typeof(object));

                return new DynamicMetaObject(convertSetPropertyExpr, GetDefaultRestrictions());
            }

            var info = MemberIndexLookup.Current.MutateMemberIndex(ScriptObject.ObjectCore.MemberIndex, binder.Name);

            var convertExpr = Expression.Convert(Expression, typeof(IScriptObject));

            var expr = Expression.Call(typeof(ScriptObjectExtensions),
                nameof(ScriptObjectExtensions.SetMember),
                null,
                convertExpr,
                Expression.Constant(info.Index),
                ExpressionHelper.DynamicConvert(value.Expression, typeof(object))
                );

            var restrictions = GetDefaultRestrictions().Merge(
                BindingRestrictions.GetExpressionRestriction(
                    Expression.Call(
                        typeof(ScriptObjectExtensions),
                        nameof(ScriptObjectExtensions.IsValidSetMember),
                        null,
                        convertExpr,
                        Expression.Constant(info.MemberIndex)
                        )
                    )
                );

            return new DynamicMetaObject(expr, restrictions);
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            if (TypeProvider.Current.TryGetProperty(LimitType, binder.Name, out PropertyInfo propertyInfo))
            {
                var propertyExpr = ExpressionHelper.DynamicConvert(Expression.Property(ScriptObjectExpr, propertyInfo), typeof(object));
                return new DynamicMetaObject(propertyExpr, GetDefaultRestrictions());
            }

            if (ScriptObject.HasTarget && TypeProvider.Current.TryGetProperty(ScriptObject.TargetType, binder.Name, out propertyInfo))
            {
                var propertyExpr = ExpressionHelper.DynamicConvert(
                    Expression.Property(
                        Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType),
                        propertyInfo), typeof(object)
                    );

                return new DynamicMetaObject(propertyExpr, GetDefaultRestrictions());
            }

            if (MemberIndexLookup.Current.TryGetMemberIndex(ScriptObject.ObjectCore.MemberIndex, binder.Name, out MemberLookupInfo info))
            {
                var convertExpr = Expression.Convert(Expression, typeof(IScriptObject));

                var expr = Expression.Call(
                typeof(ScriptObjectExtensions),
                nameof(ScriptObjectExtensions.GetMember),
                null,
                convertExpr,
                Expression.Constant(info.Index)
                );

                var restrictions = GetDefaultRestrictions()
                    .Merge(
                    BindingRestrictions.GetExpressionRestriction(
                        Expression.Call(
                            typeof(ScriptObjectExtensions),
                            nameof(ScriptObjectExtensions.IsValidGetMember),
                            null,
                            convertExpr,
                            Expression.Constant(info.MemberIndex)
                            )
                        )
                    );

                return new DynamicMetaObject(expr, restrictions);
            }

            if (ExpressionHelper.TryGetMethodExpression(ScriptObjectExpr, binder.Name, out Expression methodExpr))
            {
                var setMemberExpr = ExpressionHelper.DynamicSetMember(binder.Name, Expression, methodExpr);

                var convertExpr = Expression.Convert(Expression, typeof(IScriptObject));
                var coreExpr = Expression.PropertyOrField(convertExpr, nameof(IScriptObject.ObjectCore));
                var callExpr = Expression.Call(
                    typeof(ScriptObjectExtensions),
                    nameof(ScriptObjectExtensions.IsValidGetMember),
                    null,
                    convertExpr,
                    Expression.Constant(ScriptObject.ObjectCore.MemberIndex)
                    );

                var restrictions = GetDefaultRestrictions()
                   .Merge(
                   BindingRestrictions.GetExpressionRestriction(callExpr)
                   );

                //var restrictions = GetDefaultRestrictions()
                //    .Merge(
                //    BindingRestrictions.GetExpressionRestriction(
                //        Expression.Call(
                //            ScriptObjectExpr,
                //            "IsValidGetMember",
                //            null,
                //            Expression.Constant(ScriptObject.MemberIndex)
                //            )
                //        )
                //    );

                return new DynamicMetaObject(setMemberExpr, restrictions);
            }

            //if (ScriptObject.HasTarget &&
            //    ExpressionHelper.TryGetMethodExpression(Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType), binder.Name, out methodExpr))
            //{
            //    var setMemberExpr = ExpressionHelper.DynamicSetMember(binder.Name, Expression, methodExpr);
            //    var restrictions = GetDefaultRestrictions()
            //        .Merge(
            //        BindingRestrictions.GetExpressionRestriction(
            //            Expression.Call(
            //                ScriptObjectExpr,
            //                "IsValidGetMember",
            //                null,
            //                Expression.Constant(ScriptObject.MemberIndex)
            //                )
            //            )
            //        );

            //    return new DynamicMetaObject(setMemberExpr, restrictions);
            //}

            throw ExceptionHelper.MemberUndefined(binder.Name);
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            if (indexes.Length == 1)
            {
                if (TypeProvider.Current.TryGetIndex(LimitType, indexes[0].LimitType, out PropertyInfo propertyInfo))
                {
                    var indexType = propertyInfo.GetIndexParameters().Single().ParameterType;
                    var indexValue = ExpressionHelper.DynamicConvert(indexes[0].Expression, indexType);
                    var indexExpr = Expression.MakeIndex(
                        Expression.Convert(ScriptObjectExpr, LimitType),
                        propertyInfo,
                        new[] { indexValue });
                    var setIndexExpr = Expression.Assign(
                        indexExpr,
                        ExpressionHelper.DynamicConvert(value.Expression, propertyInfo.PropertyType));

                    var convertValueExpr = ExpressionHelper.DynamicConvert(setIndexExpr, typeof(object));

                    var restrictions = GetIndexBindingRestrictions(indexes[0]);

                    return new DynamicMetaObject(convertValueExpr, restrictions);
                }

                if (ScriptObject.HasTarget && TypeProvider.Current.TryGetIndex(ScriptObject.TargetType, indexes[0].LimitType, out propertyInfo))
                {
                    var indexType = propertyInfo.GetIndexParameters().Single().ParameterType;
                    var indexValue = ExpressionHelper.DynamicConvert(indexes[0].Expression, indexType);
                    var indexExpr = Expression.MakeIndex(
                        Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType),
                        propertyInfo,
                        new[] { indexValue });
                    var setIndexExpr = Expression.Assign(
                        indexExpr,
                        ExpressionHelper.DynamicConvert(value.Expression, propertyInfo.PropertyType));

                    var convertValueExpr = ExpressionHelper.DynamicConvert(setIndexExpr, typeof(object));

                    var restrictions = GetIndexBindingRestrictions(indexes[0]);

                    return new DynamicMetaObject(convertValueExpr, restrictions);
                }

                if (indexes[0].Value is string)
                {
                    var expr = Expression.Call(
                        typeof(ScriptObjectExtensions),
                        nameof(ScriptObjectExtensions.SetIndex),
                        null,
                        Expression.Convert(Expression, typeof(IScriptObject)),
                        Expression.Convert(indexes[0].Expression, typeof(string)),
                        ExpressionHelper.DynamicConvert(value.Expression, typeof(object)));

                    var restrictions = GetIndexBindingRestrictions(indexes[0]);

                    return new DynamicMetaObject(expr, restrictions);
                }
                else
                {
                    var convertExpr = ExpressionHelper.DynamicConvert(indexes[0].Expression, typeof(string));
                    var setIndexExpr = ExpressionHelper.DynamicSetIndex(Expression, convertExpr, value.Expression);

                    var restrictions =
                        BindingRestrictions.GetTypeRestriction(Expression, LimitType)
                            .Merge(BindingRestrictions.GetTypeRestriction(indexes[0].Expression, indexes[0].LimitType));

                    return new DynamicMetaObject(setIndexExpr, restrictions);
                }
            }

            throw ExceptionHelper.InvalidIndexes();
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            if (indexes.Length == 1)
            {
                if (TypeProvider.Current.TryGetIndex(LimitType, indexes[0].LimitType, out PropertyInfo propertyInfo))
                {
                    var indexType = propertyInfo.GetIndexParameters().Single().ParameterType;
                    var indexValue = ExpressionHelper.DynamicConvert(indexes[0].Expression, indexType);
                    var indexExpr = ExpressionHelper.DynamicConvert(
                        Expression.MakeIndex(
                            Expression.Convert(ScriptObjectExpr, LimitType),
                            propertyInfo,
                            new[] { indexValue }),
                        typeof(object)
                        );

                    var restrictions = GetIndexBindingRestrictions(indexes[0]);

                    return new DynamicMetaObject(indexExpr, restrictions);
                }

                if (ScriptObject.HasTarget && TypeProvider.Current.TryGetIndex(ScriptObject.TargetType, indexes[0].LimitType, out propertyInfo))
                {
                    var indexType = propertyInfo.GetIndexParameters().Single().ParameterType;
                    var indexValue = ExpressionHelper.DynamicConvert(indexes[0].Expression, indexType);
                    var indexExpr = ExpressionHelper.DynamicConvert(
                        Expression.MakeIndex(
                            Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType),
                            propertyInfo,
                            new[] { indexValue }),
                        typeof(object)
                        );

                    var restrictions = GetIndexBindingRestrictions(indexes[0]);

                    return new DynamicMetaObject(indexExpr, restrictions);
                }

                if (indexes[0].Value is string)
                {
                    var expr = Expression.Call(
                        typeof(ScriptObjectExtensions),
                        nameof(ScriptObjectExtensions.GetIndex),
                        null,
                        Expression.Convert(Expression, typeof(IScriptObject)),
                        Expression.Convert(indexes[0].Expression, typeof(string))
                        );

                    return new DynamicMetaObject(expr, GetIndexBindingRestrictions(indexes[0]));
                }
                else
                {
                    var convertExpr = ExpressionHelper.DynamicConvert(indexes[0].Expression, typeof(string));
                    var getIndexExpr = ExpressionHelper.DynamicGetIndex(Expression, convertExpr);

                    return new DynamicMetaObject(getIndexExpr, GetIndexBindingRestrictions(indexes[0]));
                }
            }

            throw ExceptionHelper.InvalidIndexes();
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var getMemberExpr = ExpressionHelper.DynamicGetMember(binder.Name, Expression);
            var invokeExpr = ExpressionHelper.DynamicInvoke(getMemberExpr, args.Select(a => a.Expression).ToArray());

            var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

            return new DynamicMetaObject(invokeExpr, restrictions);
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            throw new NotImplementedException("Constructor not implemented");
            //if (ScriptObject.Ctor == null)
            //    ExceptionHelper.InvalidCreateInstance(ScriptObject.GetScriptType());

            //var newInstanceExpr = Expression.Parameter(typeof(ScriptObject));
            //var newExpr = Expression.Call(ScriptObjectExpr, "CreateInstance", null);
            //var assignNewInstanceExpr = Expression.Assign(newInstanceExpr, newExpr);
            //var hasCtorExpr = Expression.IfThen(
            //    Expression.Call(newInstanceExpr, "HasMemberOfType", null, Expression.Constant("ctor"), Expression.Constant(typeof(ScriptFunction))),
            //    ExpressionHelper.DynamicInvokeMember(
            //        newInstanceExpr,
            //        "ctor",
            //        args.Select(a => a.Expression).ToArray())
            //    );

            //var blockExpr = Expression.Block(typeof(object), new[] { newInstanceExpr }, assignNewInstanceExpr, hasCtorExpr, newInstanceExpr);

            //return new DynamicMetaObject(blockExpr, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            if (binder.Type == typeof(object))
            {
                var expr = Expression.Convert(Expression, typeof(object));
                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

                return new DynamicMetaObject(expr, restrictions);
            }

            if (ScriptObject.HasTarget && binder.Type.IsAssignableFrom(ScriptObject.TargetType))
            {
                var expr = Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType);
                var restrictions = GetDefaultRestrictions();

                return new DynamicMetaObject(expr, restrictions);
            }

            if (ScriptObject.HasTarget && TypeProvider.Current.TryGetConvertOperator(ScriptObject.TargetType, binder.Type, out MethodInfo convertMethod))
            {
                var convertExpr = Expression.Call(convertMethod,
                    Expression.Convert(ScriptObjectTargetExpr, ScriptObject.TargetType));
                var restrictions = GetDefaultRestrictions();

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if (binder.Type.IsAssignableFrom(LimitType))
            {
                var expr = Expression.Convert(Expression, LimitType);
                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

                return new DynamicMetaObject(expr, restrictions);
            }

            
            if (TypeProvider.Current.TryGetConvertOperator(LimitType, binder.Type, out convertMethod))
            {
                var callExpr = Expression.Call(
                    convertMethod,
                    Expression.Convert(Expression, LimitType));

                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

                return new DynamicMetaObject(callExpr, restrictions);
            }

            throw ExceptionHelper.InvalidConvert(LimitType, binder.Type);
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return BindHelper.BindBinaryOperation(binder, this, arg);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return ScriptObject.GetMembers();
        }

        public override DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value)
        {
            return BindHelper.BindIsOperation(binder, this, value);
        }

        public override DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            return BindHelper.BindExtendsOperation(binder, this, value);
        }

        public override DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder)
        {
            var hasMember = ScriptObject.GetMembers().Any(m => m == binder.Name);

            var restrictions = GetDefaultRestrictions()
                    .Merge(
                    BindingRestrictions.GetExpressionRestriction(
                        Expression.Call(
                            typeof(ScriptObjectExtensions),
                            nameof(ScriptObjectExtensions.IsValidGetMember),
                            null,
                            Expression.Convert(Expression, typeof(IScriptObject)),
                            Expression.Constant(ScriptObject.ObjectCore.MemberIndex)
                            )
                        )
                    );

            return new DynamicMetaObject(Expression.Constant(hasMember, typeof(object)), restrictions);
        }

        //public override DynamicMetaObject BindBeginMemberOperation(ScriptBeginMemberOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    var getMemberExpr = ExpressionHelper.DynamicGetMember(binder.Name, Expression);
        //    var invokeExpr = ExpressionHelper.DynamicBeginOperation(getMemberExpr, args.Select(a => a.Expression).ToArray());

        //    var restrictions = GetDefaultRestrictions()
        //           .Merge(
        //           BindingRestrictions.GetExpressionRestriction(
        //               Expression.Call(
        //                   ScriptObjectExpr,
        //                   "IsValidGetMember",
        //                   null,
        //                   Expression.Constant(ScriptObject.MemberIndex)
        //                   )
        //               )
        //           );

        //    return new DynamicMetaObject(invokeExpr, restrictions);
        //}

        //public override DynamicMetaObject BindAwaitMemberOperation(ScriptAwaitMemberOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    var getMemberExpr = ExpressionHelper.DynamicGetMember(binder.Name, Expression);
        //    var invokeExpr = ExpressionHelper.DynamicAwaitOperation(getMemberExpr, args.Select(a => a.Expression).ToArray());

        //    var restrictions = GetDefaultRestrictions()
        //           .Merge(
        //           BindingRestrictions.GetExpressionRestriction(
        //               Expression.Call(
        //                   ScriptObjectExpr,
        //                   "IsValidGetMember",
        //                   null,
        //                   Expression.Constant(ScriptObject.MemberIndex)
        //                   )
        //               )
        //           );

        //    return new DynamicMetaObject(invokeExpr, restrictions);
        //}

        private BindingRestrictions GetDefaultRestrictions()
        {
            return BindingRestrictions.GetTypeRestriction(Expression, LimitType)
                .Merge(
                    RestrictionsHelper.TypeOrNullRestriction(
                        new DynamicMetaObject(ScriptObjectTargetExpr,
                            BindingRestrictions.Empty, ScriptObject.Target))
                );
        }

        private BindingRestrictions GetIndexBindingRestrictions(DynamicMetaObject index)
        {
            return GetDefaultRestrictions()
                .Merge(BindingRestrictions.GetTypeRestriction(Expression, LimitType))
                .Merge(BindingRestrictions.GetTypeRestriction(index.Expression, index.LimitType));
        }
    }
}
