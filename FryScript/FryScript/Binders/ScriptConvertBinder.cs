using FryScript.Extensions;
using FryScript.Helpers;
using FryScript.HostInterop;
using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Binders
{

    public class ScriptConvertBinder : ConvertBinder
    {
        public ScriptConvertBinder(Type type)
            : base(type, true)
        {
        }

        public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            if (target.Value == null)
            {
                return new DynamicMetaObject(
                    ExpressionHelper.Null(Type),
                    RestrictionsHelper.TypeOrNullRestriction(target));
            }

            if (Type.IsEnum)
            {
                var typeExpr = Expression.Constant(Type);
                var toObjectExpr = Expression.Call(typeof(Enum), nameof(Enum.ToObject), new Type[0], typeExpr, target.Expression);
                var convertExpr = Expression.Convert(toObjectExpr, Type);
                return new DynamicMetaObject(
                    convertExpr,
                    RestrictionsHelper.TypeOrNullRestriction(target));
            }

            if (Type == typeof(object))
            {
                var expr = Expression.Convert(target.Expression, typeof(object));
                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                return new DynamicMetaObject(expr, restrictions);
            }

            if (target.LimitType == Type)
            {
                var convertExpr = Expression.Convert(target.Expression, target.LimitType);
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if (target.LimitType.TryCanConvert(Type, out MethodInfo convertMethod))
            {
                var expr = Expression.Convert(target.Expression, target.LimitType, convertMethod);
                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                return new DynamicMetaObject(expr, restrictions);
            }

            if (TypeProvider.Current.TryGetConvertOperator(target.LimitType, Type, out convertMethod))
            {
                var callExpr = Expression.Call(
                    convertMethod,
                    Expression.Convert(target.Expression, target.LimitType));

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                return new DynamicMetaObject(callExpr, restrictions);
            }

            if (TypeProvider.Current.IsNumericType(target.LimitType) && TypeProvider.Current.IsNumericType(Type))
            {
                var convertExpr = Expression.Convert(target.Expression, target.LimitType);
                convertExpr = Expression.Convert(convertExpr, Type);

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if (Type.IsNullable())
            {
                var unwrappedNullable = Type.UnwrapNullable();

                if (TypeProvider.Current.TryGetConvertOperator(target.LimitType, unwrappedNullable, out convertMethod))
                {
                    Expression callExpr = Expression.Call(
                        convertMethod,
                        Expression.Convert(target.Expression, target.LimitType));

                    callExpr = Expression.Convert(callExpr, Type);

                    var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                    return new DynamicMetaObject(callExpr, restrictions);
                }

                // return new DynamicMetaObject(Expression.Convert(target.Expression, Type),
                //     BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                //     );
            }

            if (target.Value is IScriptable)
            {
                var convertExpr = Expression.Convert(target.Expression, typeof(IScriptable));
                var scriptProperty = Expression.Property(convertExpr, "Script");
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(scriptProperty, restrictions);
            }

            if (Type.IsAssignableFrom(target.LimitType))
            {
                var convertExpr = Expression.Convert(target.Expression, Type);
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            // if (target.Value is IConvertible && typeof(IConvertible).IsAssignableFrom(Type))
            // {
            //     var changeTypeExpr = Expression.Call(
            //     typeof(Convert),
            //     "ChangeType",
            //     null,
            //     Expression.Convert(target.Expression, typeof(object)),
            //     Expression.Constant(Type));

            //     var convertExpr = Expression.Convert(changeTypeExpr, Type);

            //     var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

            //     return new DynamicMetaObject(convertExpr, restrictions);
            // }

            throw ExceptionHelper.InvalidConvert(target.LimitType, Type);
        }
    }
}
