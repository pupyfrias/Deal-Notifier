using System.Linq.Expressions;

namespace WebScraping.Core.Application.Extensions
{
    public static class ExpressioExtension
    {
        public static BinaryExpression AddConditions(this BinaryExpression body, Expression parameter, List<string>? list, bool exclude)
        {
            if (list != null)
            {
                Expression condition = Expression.Empty();

                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        condition = CreateMethodCallContains(parameter, list[i]);
                    }
                    else
                    {
                        condition = Expression.Or(condition, CreateMethodCallContains(parameter, list[i]));
                    }
                };

                if (exclude)
                {
                    condition = Expression.Not(condition);
                }
                return Expression.And(body, condition);
            }

            return body;
        }

        public static BinaryExpression AddConditions(this BinaryExpression body, Expression parameter, List<int>? list)
        {
            if (list != null)
            {
                Expression condition = Expression.Empty();

                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        condition = CreateEqual(parameter, list[i]);
                    }
                    else
                    {
                        condition = Expression.Or(condition, CreateEqual(parameter, list[i]));
                    }
                };

                return Expression.And(body, condition);
            }

            return body;
        }

        public static BinaryExpression AddConditions(this BinaryExpression body, Expression parameter, decimal price, bool greaterThan)
        {
            if (price > 0)
            {
                Expression condition = Expression.Empty();

                if (greaterThan)
                {
                    condition = CreateLessThanOrEqual(parameter, price);
                }
                else
                {
                    condition = CreateGreaterThanOrEqual(parameter, price);
                }

                return Expression.And(body, condition);
            }

            return body;
        }

        public static MethodCallExpression CreateMethodCallContains(Expression parameter, string value)
        {
            return Expression.Call(parameter, "Contains", null, Expression.Constant(value), Expression.Constant(StringComparison.OrdinalIgnoreCase));
        }

        private static Expression CreateEqual(Expression parameter, int value)
        {
            return Expression.Equal(parameter, Expression.Constant(value));
        }

        private static Expression CreateGreaterThanOrEqual(Expression parameter, decimal value)
        {
            return Expression.GreaterThanOrEqual(parameter, Expression.Constant(value));
        }

        private static Expression CreateLessThanOrEqual(Expression parameter, decimal value)
        {
            return Expression.LessThanOrEqual(parameter, Expression.Constant(value));
        }
    }
}