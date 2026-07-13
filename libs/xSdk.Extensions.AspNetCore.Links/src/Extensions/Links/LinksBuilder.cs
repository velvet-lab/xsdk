using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Links;

public class LinksBuilder : BuilderBase
{
    internal Action<LinksOptions> ConfigureLinksAction
    {
        get => field ?? (_ => ConfigureLinks(_));
        set;
    }

    protected virtual void ConfigureLinks(LinksOptions options)
    {

    }
}
