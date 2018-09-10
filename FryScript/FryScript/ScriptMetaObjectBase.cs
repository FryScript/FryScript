namespace FryScript
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq.Expressions;
    using Helpers;
    using Binders;
    using System;

    public abstract class ScriptMetaObjectBase : DynamicMetaObject,
        //IBindAwaitMemberOperationProvider,
        //IBindAwaitOperationProvider,
        //IBindBeginMemberOperationProvider,
        //IBindBeginOperationProvider,
        IBindExtendsOperationProvider,
        IBindHasOperationProvider,
        IBindIsOperationProvider
        //IBindResumeOperationProvider
    {
        protected ScriptMetaObjectBase(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        //public virtual DynamicMetaObject BindAwaitMemberOperation(ScriptAwaitMemberOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    throw ExceptionHelper.NonAwaitable(LimitType);
        //}

        //public virtual DynamicMetaObject BindAwaitOperation(ScriptAwaitOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    throw ExceptionHelper.NonAwaitable(LimitType);
        //}

        //public virtual DynamicMetaObject BindBeginMemberOperation(ScriptBeginMemberOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    throw ExceptionHelper.NonBeginable(LimitType);
        //}

        //public virtual DynamicMetaObject BindBeginOperation(ScriptBeginOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    throw ExceptionHelper.NonBeginable(LimitType);
        //}

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            throw ExceptionHelper.NonBinaryOperation(LimitType);
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            throw ExceptionHelper.NonConvertible(LimitType);
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            throw ExceptionHelper.NonCreateInstance(LimitType);
        }

        public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
        {
            throw ExceptionHelper.NonDeleteIndex(LimitType);
        }

        public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
        {
            throw ExceptionHelper.NonDeleteMember(LimitType);
        }

        public virtual DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            throw ExceptionHelper.InvalidExtendsOperation(LimitType);
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            throw ExceptionHelper.NonGetIndex(LimitType);
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            throw ExceptionHelper.NonGetMember(LimitType);
        }

        public virtual DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder)
        {
            throw ExceptionHelper.InvalidHasOperation(LimitType);
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            throw ExceptionHelper.NonInvokable(LimitType);
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            throw ExceptionHelper.NonInvokeMember(LimitType);
        }

        public virtual DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value)
        {
            throw ExceptionHelper.InvalidIsOperation(LimitType);
        }

        //public virtual DynamicMetaObject BindResumeOperation(ScriptResumeOperationBinder binder)
        //{
        //    throw ExceptionHelper.NonResumable(LimitType);
        //}

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            throw ExceptionHelper.NonSetIndex(LimitType);
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            throw ExceptionHelper.NonSetMember(LimitType);
        }

        public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            throw ExceptionHelper.NonUnaryOperation(LimitType);
        }
    }
}
