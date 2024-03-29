﻿using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces;
using Catalog.Domain.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Catalog.Application.Specification
{
    public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : IAuditableEntity
    {
        protected Specification(IPaginationBase pagination)
        {
            Skip = pagination.Offset;
            Take = pagination.Limit;
            Descending = pagination?.Descending ?? false;
            ApplyOrderBy(pagination?.OrderBy ?? "Created");
        }

        public Expression<Func<TEntity, bool>> Criteria { get; protected set; }
        public bool Descending { get; private set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; }

        public int Skip { get; private set; }
        public int Take { get; private set; }

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }


        public void ApplyOrderBy(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "p");
            var propertyInfo = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new BadRequestException($"Could not find {propertyName} on {typeof(TEntity).Name}");
            }

            var property = Expression.Property(parameter, propertyInfo);
            var conversion = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);

            OrderBy = lambda;
        }
    }
}