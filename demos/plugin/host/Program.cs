using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Demos;
using xSdk.Hosting;

const string APP_NAME = "plugin";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "pl";

var host = xSdk.Hosting.Host.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX).EnablePlugin<MyPlugin>().Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
