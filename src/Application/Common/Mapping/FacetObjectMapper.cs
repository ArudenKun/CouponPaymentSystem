using Abp.ObjectMapping;
using CouponPaymentSystem.Application.Common.Extensions;

namespace CouponPaymentSystem.Application.Common.Mapping;

public sealed class FacetObjectMapper : IObjectMapper
{
    public TDestination Map<TDestination>(object source) =>
        (TDestination)source.ToFacet(typeof(TDestination));

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) =>
        source.ToFacet<TSource, TDestination>();

    public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source) =>
        (IQueryable<TDestination>)source.SelectFacet(source.ElementType, typeof(TDestination));
}
