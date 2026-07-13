using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace xSdk.Extensions.WebApi;

public static class WebApiBuilderExtensions
{
    extension(WebApiBuilder builder)
    {
        public WebApiBuilder WithMvcOptions(Action<MvcOptions> configureMvcAction)
        {
            builder.ConfigureMvcAction = configureMvcAction;
            return builder;
        }
    }  
}
