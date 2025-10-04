using Abp.UI;
using Abp.Web.Mvc.Controllers;

namespace Web.Controllers;

public abstract class WebControllerBase : AbpController
{
    protected virtual void CheckModelState(string message = "")
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(
                string.IsNullOrEmpty(message) ? "Form is not valid" : message
            );
        }
    }
}
