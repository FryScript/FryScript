using FryScript.Helpers;
using FryScript.HostInterop;
using System;
using System.Dynamic;
using System.Linq.Expressions;

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

            if(Type.IsEnum)
            {
                var typeExpr = Expression.Constant(Type);
                var toObjectExpr = Expression.Call(typeof(Enum), "ToObject", new Type[0], typeExpr, target.Expression);
                var convertExpr = Expression.Convert(toObjectExpr, Type);
                return new DynamicMetaObject(
                    convertExpr,
                    RestrictionsHelper.TypeOrNullRestriction(target));
            }

            if(target.LimitType == Type)
            {
                var convertExpr = Expression.Convert(target.Expression, target.LimitType);
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            var metaObject = TypeProvider.Current.GetMetaObject(target.Expression, target.Value);

            if (metaObject != null)
            {
                return metaObject.BindConvert(this);
            }

            if(target.Value is IScriptable)
            {
                var convertExpr = Expression.Convert(target.Expression, typeof(IScriptable));
                var scriptProperty = Expression.Property(convertExpr, "Script");
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(scriptProperty, restrictions);
            }

            if(Type.IsAssignableFrom(target.LimitType))
            {
                var convertExpr = Expression.Convert(target.Expression, Type);
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if(target.Value is IConvertible && typeof(IConvertible).IsAssignableFrom(Type))
            {
                var changeTypeExpr = Expression.Call(
                typeof(Convert),
                "ChangeType",
                null,
                Expression.Convert(target.Expression, typeof(object)),
                Expression.Constant(Type));

                var convertExpr = Expression.Convert(changeTypeExpr, Type);

                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            throw ExceptionHelper.InvalidConvert(target.LimitType, Type);
        }
    }
}
