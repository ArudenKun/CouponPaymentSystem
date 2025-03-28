using Vogen;

[assembly: VogenDefaults(
    conversions: Conversions.TypeConverter
        | Conversions.NewtonsoftJson
        | Conversions.DapperTypeHandler,
    toPrimitiveCasting: CastOperator.Implicit
)]

namespace Domain;

public interface IDomain;
