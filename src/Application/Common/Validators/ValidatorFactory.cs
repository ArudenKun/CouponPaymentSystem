using FluentValidation;

namespace Application.Common.Validators;

public class ValidatorFactory : ValidatorFactoryBase
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override IValidator CreateInstance(Type validatorType) =>
        (IValidator)_serviceProvider.GetService(validatorType);
}
