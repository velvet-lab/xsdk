using Microsoft.AspNetCore.Mvc;

namespace xSdk.Extensions.Links;

public static class HateoasExtensions
{
    public static bool IsHateoasEnabled(this ControllerBase controller)
    {
        if (controller.HttpContext.Request.Headers.TryGetValue("X-HATEOAS-ENABLED", out var values))
        {
            if (values.ToString().ToLower() == "true" || values.ToString() == "1")
            {
                //
                return true;
            }
        }
        return false;
    }
}
