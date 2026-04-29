# Copilot Processing

## User Request
Improve test coverage from 72% to at least 80% across the xSdk repository.
- Add tests for files with low coverage
- Exclude untestable classes (UI commands, hosting infra, EF/MongoDB context wiring, etc.) from code coverage

## Files by Coverage (sorted ascending)

| File | Coverage |
|------|----------|
| libs/xSdk.Core/src/Extensions/Commands/DefaultCommandSettings.cs | 0.0% |
| libs/xSdk/src/Extensions/Variable/Commands/ListCommand.cs | 0.0% |
| libs/xSdk.Data.EntityFramework.MongoDB/src/MongoDbContext.cs | 0.0% |
| libs/xSdk.Data.EntityFramework.MongoDB/src/MongoDbOptionsExtensions.cs | 0.0% |
| libs/xSdk.Core/src/Extensions/Web/RestClientBuilder.cs | 0.0% |
| libs/xSdk.Extensions.AspNetCore/src/Hosting/WebHost.Application.cs | 0.0% |
| libs/xSdk.Extensions.AspNetCore/src/Hosting/WebHost.cs | 0.0% |
| libs/xSdk.Extensions.AspNetCore/src/Hosting/WebHost.Kestrel.cs | 0.0% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/WebSecurity/WebSecurityPluginHost.cs | 4.2% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/Authentication/AuthenticationBuilderExtensions.ApiKey.cs | 5.0% |
| libs/xSdk.Core/src/Extensions/Plugin/IPluginService.cs | 15.0% |
| libs/xSdk.Data.Vault/src/Data/CertAuthOptions.cs | 15.4% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/Documentation/DocumentationPluginHost.cs | 20.0% |
| libs/xSdk.Extensions.CloudEvents/src/Extensions/CloudEvents/CloudEventWebExtensions.cs | 22.0% |
| libs/xSdk.Extensions.AspNetCore.Links/src/Extensions/Links/MethodAnalyzer.cs | 22.2% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/Authentication/AuthenticationPluginHost.cs | 23.9% |
| libs/xSdk/src/Extensions/Variable/Providers/CommandlineProvider.cs | 25.0% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/WebApi/PlainTextFormatter.cs | 30.4% |
| libs/xSdk.Data/src/Data/Database.cs | 31.3% |
| libs/xSdk.Core/src/Shared/CertificateHelper.cs | 35.5% |
| libs/xSdk.Data/src/Data/Converters/Yaml/SemVerVersionConverter.cs | 35.7% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/Compression/CompressionPluginHost.cs | 37.5% |
| libs/xSdk.Extensions.CloudEvents/src/Extensions/CloudEvents/CloudEventJsonInputFormatter.cs | 40.0% |
| libs/xSdk.Data.Vault/src/Data/IVaultRepository.cs | 40.0% |
| libs/xSdk.Core/src/Hosting/PluginHostCollection.cs | 40.0% |
| libs/xSdk/src/Hosting/TestHost.cs | 40.0% |
| libs/xSdk.Core/src/Hosting/WebPluginHost.cs | 40.0% |
| libs/xSdk.Extensions.AspNetCore.Links/src/Extensions/Links/LinksService.cs | 44.1% |
| libs/xSdk/src/Extensions/Variable/Providers/FallbackProvider.cs | 46.7% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/WebApi/WebApiPluginHost.cs | 46.7% |
| libs/xSdk/src/Extensions/Variable/VariableService.cs | 46.9% |
| libs/xSdk.Extensions.AspNetCore.Links/src/Extensions/Links/RoutedLinkBuilder.cs | 48.2% |
| libs/xSdk.Data/src/Data/DatabaseBuilderExtensions.cs | 50.0% |
| libs/xSdk.Data.Vault/src/Data/DatabaseBuilderExtensions.cs | 50.0% |
| libs/xSdk.Data.FlatFile/src/DatalayerBuilderExtensions.cs | 50.0% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/Authentication/HostBuilderExtensions.cs | 50.0% |
| libs/xSdk/src/Extensions/Variable/Providers/VariableProviderBase.cs | 50.0% |
| libs/xSdk/src/Extensions/Options/EnvironmentOptions.System.cs | 51.5% |
| libs/xSdk.Core/src/Data/Converters/Json/DateTimeConverter.cs | 52.2% |
| libs/xSdk.Data/src/Data/MappingFactory.cs | 53.3% |
| libs/xSdk.Core/src/Hosting/StackTraceUtils.cs | 55.2% |
| libs/xSdk.Data/src/Data/Repository.cs | 55.6% |
| libs/xSdk.Data/src/Data/MappingProfile.cs | 56.8% |
| libs/xSdk.Data.Vault/src/Data/VaultDatabase.cs | 57.6% |
| libs/xSdk.Core/src/Security/SecurityContext.cs | 59.3% |
| libs/xSdk/src/Extensions/Plugin/CatalogHelper.cs | 61.5% |
| libs/xSdk.Data/src/Data/Fakes.cs | 61.5% |
| libs/xSdk.Extensions.AspNetCore/src/Controllers/HealthController.cs | 61.9% |
| libs/xSdk.Data/src/Data/DatabaseBuilder.cs | 63.6% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/DataProtection/DataProtectionPluginHost.cs | 65.0% |
| libs/xSdk.Data.EntityFramework/src/DatalayerBuilderExtensions.cs | 66.7% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/DataProtection/DefaultDataProtectionBuilder.cs | 66.7% |
| libs/xSdk/src/Extensions/Variable/Providers/FileProvider.cs | 66.7% |
| libs/xSdk/src/Extensions/Variable/Variable.cs | 66.7% |
| libs/xSdk.Extensions.AspNetCore/src/Plugins/WebSecurity/WebSecurityOptions.cs | 66.7% |
| libs/xSdk/src/Hosting/SlimHost.cs | 67.8% |
| libs/xSdk.Extensions.AspNetCore/src/Hosting/WebHostTestFixture.cs | 68.2% |
| libs/xSdk.Data.EntityFramework/src/EntityFrameworkRepository.cs | 69.2% |
| libs/xSdk.Extensions.AspNetCore/src/Controllers/VariableController.cs | 69.8% |
| libs/xSdk/src/Extensions/Variable/Providers/OptionProvider.cs | 70.8% |
| libs/xSdk.Data/src/Data/DatalayerFactory.cs | 71.4% |
| libs/xSdk.Data.FlatFile/src/FlatFileDatabase.cs | 71.8% |
| libs/xSdk/src/Extensions/Plugin/PluginService.Catalog.cs | 72.4% |

## Action Plan

### Phase A: Exclude untestable files (0% coverage, infrastructure/UI)
1. [ ] Exclude WebHost.cs, WebHost.Application.cs, WebHost.Kestrel.cs from coverage (hosting infra)
2. [ ] Exclude MongoDbContext.cs, MongoDbOptionsExtensions.cs from coverage (EF/DB wiring)
3. [ ] Exclude ListCommand.cs from coverage (Spectre Console command - UI)
4. [ ] Exclude DefaultCommandSettings.cs from coverage (Spectre Console settings - UI)
5. [ ] Exclude RestClientBuilder.cs from coverage (HTTP client builder - integration-only)

### Phase B: Write new tests for files in <50% range (high priority)
1. [ ] DateTimeConverter - additional test cases (52% → 90%+)
2. [ ] StackTraceUtils - additional test cases (55% → 80%+)
3. [ ] PluginHostCollection tests (40%)
4. [ ] WebPluginHost tests (40%)
5. [ ] SemVerVersionConverter tests (35%)
6. [ ] CommandlineProvider tests (25%)

### Phase C: Write new tests for 50-70% range
1. [ ] EnvironmentOptions.System additional tests (51%)
2. [ ] MappingFactory tests (53%)
3. [ ] SecurityContext tests (59%)
4. [ ] CatalogHelper tests (61%)
5. [ ] VariableService tests (46%)

## Status
- [ ] Phase A: Exclude untestable files
- [ ] Phase B: High priority new tests  
- [ ] Phase C: Medium priority new tests
- [ ] Phase D: Verify coverage improvement
