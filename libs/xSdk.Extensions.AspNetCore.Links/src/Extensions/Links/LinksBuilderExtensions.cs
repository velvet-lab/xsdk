using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Links;

public static class LinksBuilderExtensions
{
    extension(LinksBuilder builder)
    {
        public LinksBuilder UseLinks(Action<LinksOptions> configure)
        {
            builder.ConfigureLinksAction = configure;
            return builder;
        }
    }
}
