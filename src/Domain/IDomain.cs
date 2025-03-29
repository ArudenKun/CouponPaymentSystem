using Vogen;

[assembly: VogenDefaults(
    conversions: Conversions.Default | Conversions.DapperTypeHandler,
    toPrimitiveCasting: CastOperator.Implicit
)]

namespace Domain;

public interface IDomain;
