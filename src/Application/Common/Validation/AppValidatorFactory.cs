using FluentValidation;

namespace Application.Common.Validation;

public class AppValidatorFactory : ValidatorFactoryBase
{
    private readonly IServiceProvider _serviceProvider;

    public AppValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override IValidator CreateInstance(Type validatorType) =>
        (IValidator)_serviceProvider.GetService(validatorType);
}
