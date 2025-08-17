using Abp.ObjectMapping;
using CouponPaymentSystem.Application.Common.Extensions;

namespace CouponPaymentSystem.Application.Common.Mapping;

public sealed class FacetObjectMapper : IObjectMapper
{
    public TDestination Map<TSource, TDestination>(TSource source) =>
        source.ToFacet<TSource, TDestination>();

    public void Map<TSource, TDestination>(TSource source, TDestination destination) =>
        source.ToFacet(destination);

    public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source) =>
        source.SelectFacet<TSource, TDestination>();
}
