using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Demos.Builders;
using xSdk.Plugins.Authentication;
using xSdk.Plugins.Compression;
using xSdk.Plugins.DataProtection;
using xSdk.Plugins.Documentation;
using xSdk.Plugins.Links;
using xSdk.Plugins.WebApi;
using xSdk.Plugins.WebSecurity;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

const string APP_NAME = "webapi";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "webapi";

var host = xSdk
    .Hosting.WebHost.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .EnableWebApi()
    .EnableDocumentation<DocumentationPluginBuilder>()
    .EnableWebSecurity()
    //.EnableAuthentication<AuthenticationPluginBuilder>()
    .EnableCompression()
    .EnableDataProtection<DataProtectionPluginBuilder>()
    .EnableLinks<LinksPluginBuilder>()
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
