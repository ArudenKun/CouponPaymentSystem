using System.Web;
using Abp.Dependency;

namespace CouponPaymentSystem.Services;

public class CookieManager : IPerWebRequestDependency
{
    private readonly HttpRequestBase _httpRequest;
    private readonly HttpResponseBase _httpResponse;

    public CookieManager(HttpRequestBase httpRequest, HttpResponseBase httpResponse)
    {
        _httpRequest = httpRequest;
        _httpResponse = httpResponse;
    }
}
