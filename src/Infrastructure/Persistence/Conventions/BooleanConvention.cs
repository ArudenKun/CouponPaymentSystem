using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Infrastructure.Persistence.Conventions;

internal class BooleanConvention : IUserTypeConvention
{
    public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
    {
        criteria.Expect(p => p.Type == typeof(bool) || p.Type == typeof(bool?));
    }

    public void Apply(IPropertyInstance instance)
    {
        instance.CustomType(instance.Property.PropertyType);
        instance.CustomSqlType("bit");
    }
}
