using Microsoft.Extensions.Options;

namespace Domain.Configuration;

[OptionsValidator]
public partial class ValidateCpsOptions : IValidateOptions<CpsOptions>;
