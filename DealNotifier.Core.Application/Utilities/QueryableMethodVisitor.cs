﻿using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Utilities
{
    public class QueryableMethodVisitor : ExpressionVisitor
    {
        public bool MethodCalled { get; private set; }
        private readonly string _methodName;

        public QueryableMethodVisitor(string methodName)
        {
            _methodName = methodName;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == _methodName)
            {
                MethodCalled = true;
            }
            return base.VisitMethodCall(node);
        }
    }
}