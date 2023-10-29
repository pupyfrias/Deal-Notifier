using AutoMapper;

namespace Catalog.Application.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllSourceNullProperties<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            mappingExpression.ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));
            return mappingExpression;
        }
    }
}