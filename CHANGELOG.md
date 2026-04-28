## [2.0.0-next.1](https://github.com/velvet-lab/xsdk/compare/v1.1.0...v2.0.0-next.1) (2026-04-28)

### ⚠ BREAKING CHANGES

* **xsdk:** introduce IPluginHostCollection and refactor SlimHost

### Features

* add comprehensive guidelines for custom instruction files ([1438132](https://github.com/velvet-lab/xsdk/commit/1438132f43314a9e8f4f776b12792b0a9954d85f))
* add copilot instructions for project-wide guidance ([85bb263](https://github.com/velvet-lab/xsdk/commit/85bb2636096e649d7d82b0ddb7dba09d6c0aa246))
* add docker compose configuration for aspire-dashboard service ([c9d0543](https://github.com/velvet-lab/xsdk/commit/c9d05431b1d634f012715ea4922b18e3c2e11f22))
* add new agents for C# Expert, ADR Generator, GitHub Actions Expert, and Technical Debt Remediation Plan ([11d5ee3](https://github.com/velvet-lab/xsdk/commit/11d5ee3ff69d47e53fa779d713540ac63d471e76))
* add new projects for apps and Proxy to solution ([eb20ab6](https://github.com/velvet-lab/xsdk/commit/eb20ab68c1faaac9c0b058a69c3b59a4b092a048))
* add SonarQube MCP guidelines and configuration files ([cdd07e1](https://github.com/velvet-lab/xsdk/commit/cdd07e1e8c61916895b83c4f6be9c30c599ee094))
* **data:** add certificate and vault options with validation ([ea41524](https://github.com/velvet-lab/xsdk/commit/ea41524676e17fdcd208ee2c22db98f927decd2c))
* **data:** add ClearDataAsync method for test isolation ([5fbddb3](https://github.com/velvet-lab/xsdk/commit/5fbddb381c81d9abeaf63735cd5bd24508b10e13))
* **data:** add project references and update runsettings for Consul ([42f168e](https://github.com/velvet-lab/xsdk/commit/42f168ea79a4d17d4356e800fa8de83c5c2147ba))
* **data:** implement comprehensive unit tests for NoSQL functionality ([5d8fda6](https://github.com/velvet-lab/xsdk/commit/5d8fda6af0e094b883cde04a8b60b8625b363bd7))
* **data:** implement Entity Framework support for data layer ([cb3f5e4](https://github.com/velvet-lab/xsdk/commit/cb3f5e49b7928b367c1738ca8ba01c402ebb1d96))
* **data:** implement MongoDbSetup and MongoDbSetupExtensions classes ([15e7bbb](https://github.com/velvet-lab/xsdk/commit/15e7bbb5bcf945a8a549086e8973ab2a57328fd2))
* **data:** introduce flat file database support ([529ccb1](https://github.com/velvet-lab/xsdk/commit/529ccb11608027d659b6dc07d1b38174cd61e64f))
* **demos:** implement custom host and plugin architecture ([eda04fa](https://github.com/velvet-lab/xsdk/commit/eda04fafd7a09d6466f1499321350dc08c8ebd2d))
* **docs:** add ADR-024 and ADR-025 for core foundation and Consul provider ([0262dcf](https://github.com/velvet-lab/xsdk/commit/0262dcff6b570d2785b4cdd75dbc81321f763f6b))
* **docs:** add multiple ADRs for new features and integrations ([a6e2a6a](https://github.com/velvet-lab/xsdk/commit/a6e2a6a96a8e45a617877b1966699a1b7c401528))
* **docs:** update architectural decision records for plugin model redesign ([3d49f1e](https://github.com/velvet-lab/xsdk/commit/3d49f1e933727893c21d0d641e03881551f8b370))
* enhance sonar-build solution with code coverage collection ([7f41578](https://github.com/velvet-lab/xsdk/commit/7f41578fa69a00cd37f23ff30c79d5ac5ee53ea8))
* **extensions:** add ICommandsPluginBuilder interface for command apps ([0edf2ac](https://github.com/velvet-lab/xsdk/commit/0edf2ac22b7a9e3ce14492780c047eea7fbfe3c0))
* **extensions:** add unit tests for VariableModel and ApiKeySetup ([a178f6f](https://github.com/velvet-lab/xsdk/commit/a178f6f3be6da87def14d450f239014274d1e219))
* **extensions:** add YARP proxy and telemetry support ([aaf61d9](https://github.com/velvet-lab/xsdk/commit/aaf61d93d15e42355cf06c52dd60cb19de3a05b7))
* **extensions:** enhance test structure and add service dependencies ([accb90d](https://github.com/velvet-lab/xsdk/commit/accb90da915902951b06c090181829a8624f8c5b))
* **extensions:** implement EnvironmentProvider for variable management ([c949143](https://github.com/velvet-lab/xsdk/commit/c94914346ad35e21a40cfa7146f38a9c02ea3e16))
* **extensions:** implement IRoutedLink and IHateoasItem interfaces ([8aaf52b](https://github.com/velvet-lab/xsdk/commit/8aaf52b2a49da7cb1f1bc44bf7135940b1e29cb6))
* **extensions:** introduce web security and telemetry plugins ([5373be9](https://github.com/velvet-lab/xsdk/commit/5373be9caa1179ab6742922ed53d1b5943a36028))
* **extensions:** remove obsolete command interfaces and implementations ([6e1f990](https://github.com/velvet-lab/xsdk/commit/6e1f990f238bd1c5e13e9a77db33ab0eda422bf4))
* **plugin:** add authentication defaults and interfaces ([5daf48e](https://github.com/velvet-lab/xsdk/commit/5daf48e2ed38564011cce87beec17d6c288fc646))
* **plugin:** add ContentTypes class and ProblemDetailsExtensions methods ([7ee2920](https://github.com/velvet-lab/xsdk/commit/7ee2920c643447d1099849d7d06f01e2085ad2cb))
* **plugin:** add ILinksPluginBuilder and ITelemetryPluginBuilder interfaces ([8219373](https://github.com/velvet-lab/xsdk/commit/8219373fe068301213f026df8fc2b217d4ba4570))
* **plugin:** add ITelemetryPluginBuilder interface for telemetry setup ([7ae8065](https://github.com/velvet-lab/xsdk/commit/7ae806509012d9c482bcaaad12a58913c6e5fbe0))
* **plugin:** add PluginBuilder and generic setup loading functionality ([e67f3c7](https://github.com/velvet-lab/xsdk/commit/e67f3c7908019906697a2eda69a569203250537e))
* **plugin:** create IPluginHost interface for plugin hosting ([8c31250](https://github.com/velvet-lab/xsdk/commit/8c312505a00e663ccd81689871b0b9802408b050))
* **plugin:** enhance PluginHost with setup loading functionality ([4b6c11e](https://github.com/velvet-lab/xsdk/commit/4b6c11eee8ab234cdd4d4969fff9c7cac8b27bd9))
* **plugin:** implement MyPluginHost and logging service ([a2e4287](https://github.com/velvet-lab/xsdk/commit/a2e4287c81d25bf1491c1d9d040cdd694335f37e))
* reenable consul layer from think-tank ([8b01651](https://github.com/velvet-lab/xsdk/commit/8b01651212998ca54ff4691b314c6879047c538e))
* **secret-scanning:** add comprehensive guide for secret scanning setup ([f57d4bb](https://github.com/velvet-lab/xsdk/commit/f57d4bb6edd9c5c7401ef094fc6e821ee9571722))
* update package versions and add new hosted service extensions ([e0e7a57](https://github.com/velvet-lab/xsdk/commit/e0e7a57ed35e0d3ea1ae9477972508b04566a851))
* update solution file to reflect new project structure ([0de4b6b](https://github.com/velvet-lab/xsdk/commit/0de4b6b73870755c99def34d78ee08079fb7b50d))
* **xsdk:** add utility tools for networking, object handling, and string manipulation ([ddd13b4](https://github.com/velvet-lab/xsdk/commit/ddd13b4c949f980fedee7d23fd3a4882e265447b))
* **xsdk:** introduce IPluginHostCollection and refactor SlimHost ([ebdf1fb](https://github.com/velvet-lab/xsdk/commit/ebdf1fb354862255a81c99c95799c98999d99af8))

### Bug Fixes

* correct argument index for shell command in module.just ([10915c3](https://github.com/velvet-lab/xsdk/commit/10915c32018326b0af15b8fdb3d2761b37f41baf))

## [1.1.0](https://github.com/velvet-lab/xsdk/compare/v1.0.0...v1.1.0) (2026-03-16)

### Features

* add clean, restore, build, and test recipes for .NET solutions ([164b09d](https://github.com/velvet-lab/xsdk/commit/164b09d5995ff8b7dd36f6054657f480277526d2))
* add formatting and check-format recipes for .NET solutions ([c3fe648](https://github.com/velvet-lab/xsdk/commit/c3fe648a7b69d21825ef33288c03b159bc0e8a65))
* add initial dependabot configuration for NuGet package updates ([b65e655](https://github.com/velvet-lab/xsdk/commit/b65e6552751a3a8cee3065f8ea9be93618ee830b))
* add license header validation and management tools ([26374f5](https://github.com/velvet-lab/xsdk/commit/26374f5ee86c48fd0fa735d13a6267f4a40a2185))
* add linting recipes for MegaLinter in .NET project ([1ef76c4](https://github.com/velvet-lab/xsdk/commit/1ef76c436803db6ef36fc9f65acd8cb059c333c2))
* add maintenance recipes for .NET tool installation and updates ([3074784](https://github.com/velvet-lab/xsdk/commit/3074784bd4f4ef4fd0f601450113d8cac0bb14ba))
* add release and maintenance recipes for .NET tools management ([6e6b4fc](https://github.com/velvet-lab/xsdk/commit/6e6b4fca4222879a0ee4ccdc5be386eb1abc7054))
* add release configuration and VS Code settings ([a781b10](https://github.com/velvet-lab/xsdk/commit/a781b10b0cdce9c02083bda87459a76d633341a1))
* add release recipe for publishing NuGet packages ([9798524](https://github.com/velvet-lab/xsdk/commit/979852432bacb2712107e6eb9f972dcff9b0b855))
* add sonar-build solution for SonarQube analysis with coverage ([529ea19](https://github.com/velvet-lab/xsdk/commit/529ea195037f1ca402775d639e195d9fe66c58fd))
* add workflows for linting, testing, sonar scanning, and feature branch builds ([7bfb958](https://github.com/velvet-lab/xsdk/commit/7bfb958d18be188ed7ca6adeb3f751d793fbd6d5))
* aktualisiere Workflows für SonarCloud-Analyse und Unit-Tests mit neuen Konfigurationen ([6fca7d1](https://github.com/velvet-lab/xsdk/commit/6fca7d1a16a2f1dc3e5d2f3bf1110ef427414eca))
* **data:** add project descriptions for data layer integrations ([5b56f40](https://github.com/velvet-lab/xsdk/commit/5b56f40883a2401828cac70b834ed13fefb6e572))
* **data:** implement soft-delete functionality in EntityFramework provider ([9d0d314](https://github.com/velvet-lab/xsdk/commit/9d0d314a958fa806f63adcfda5124e7dcb2e8687))
* **data:** implement soft-delete functionality in EntityFramework provider ([5066afd](https://github.com/velvet-lab/xsdk/commit/5066afd8bb078f4faa4b540aff69ca730ade7277))
* enhance NuGet package publishing with symbol package support ([e4fe6ed](https://github.com/velvet-lab/xsdk/commit/e4fe6eddb7a3697ddd154d309c7d5ec76e4cb1ee))
* enhance setup repository action with credential persistence ([3d294c6](https://github.com/velvet-lab/xsdk/commit/3d294c6d0b0a76d5c48f199cdc2501d7b8db6833))
* erweitere dependabot.yml zur Überwachung von NuGet, GitHub Actions und npm-Paketen ([367bbc5](https://github.com/velvet-lab/xsdk/commit/367bbc55f19e8d158ef962b6300faf3021360f3d))
* füge neue Befehle für .NET-Tools hinzu und verbessere die Formatierungs- und Linting-Rezepte ([32ee46c](https://github.com/velvet-lab/xsdk/commit/32ee46cb215be415b19d1a391ba3a00c7585d454))
* füge Release-Workflow hinzu und aktualisiere .NET Setup in den Unit-Tests und SonarCloud Workflows ([df6ded0](https://github.com/velvet-lab/xsdk/commit/df6ded0e8b7ec2b2a8ea1182c30c197a7cc6b560))
* füge Veröffentlichung der Testberichtzusammenfassung zu den Unit-Tests hinzu ([31d1052](https://github.com/velvet-lab/xsdk/commit/31d105219d31dfeebcd380e211a0debab1af1bee))
* füge Workflows für SonarCloud-Analyse und Unit-Tests hinzu ([1961bb8](https://github.com/velvet-lab/xsdk/commit/1961bb8366612a505fa5cfc721b7c33a8525b4ab))
* füge zusätzliche Badges zur README.md hinzu ([68de383](https://github.com/velvet-lab/xsdk/commit/68de383954fd29d1566012a6de1f8ab6414878c0))
* implement setup repository action for CI workflows ([4bd23d0](https://github.com/velvet-lab/xsdk/commit/4bd23d0892d4bb3e4e6bfca85ebdf21af10e4afa))
* **plugin:** add unit tests for various helper classes ([8c49b5d](https://github.com/velvet-lab/xsdk/commit/8c49b5d7ef018c542a4c9b5a1f23bf48965777f7))
* reorganize Justfile recipes for better structure ([361ad8a](https://github.com/velvet-lab/xsdk/commit/361ad8a41068e5153bb1bdd2765fd6bba562a4ac))
* update release workflow and Justfile for improved dependency management ([1bc4a6e](https://github.com/velvet-lab/xsdk/commit/1bc4a6e4f9cd246d9edd7c60c6fcb4874dc39dfe))

### Bug Fixes

* aktualisiere .NET-Version auf 8.0.x und füge Berechtigungen für Jobs hinzu ([77ee273](https://github.com/velvet-lab/xsdk/commit/77ee273162acab097c67f49a2203e187398d98af))
* aktualisiere Intervall für GitHub Actions auf wöchentlich und aktualisiere die Version des Testberichterstatters ([e80ea6a](https://github.com/velvet-lab/xsdk/commit/e80ea6a4925d0b217cd4b01dfc2860de69234297))
* aktualisiere Kommentare in dependabot.yml auf Englisch ([57b2224](https://github.com/velvet-lab/xsdk/commit/57b222483905419ad9952b46dd7bd9b37696a5cd))
* aktualisiere pnpm-Setup und Installationsbefehle in den Workflows ([e31e656](https://github.com/velvet-lab/xsdk/commit/e31e6567857675a5a8792119573fdea5f6f64fbe))
* aktualisiere Version des Testberichterstatters in den Unit-Tests ([d1c2693](https://github.com/velvet-lab/xsdk/commit/d1c26935d6bfd08b6dceecac14463283cc2fb093))
* **controller:** rename GetSampleAsyncV2 to GetSampleAsync ([2e16f5c](https://github.com/velvet-lab/xsdk/commit/2e16f5cbba1ebd2dbfcee012d2991625e0acf85a))
* **demos:** update package versions and remove unused references ([c261bda](https://github.com/velvet-lab/xsdk/commit/c261bda3a38bc1912b8a6102c2c044e76577f5e0))
* entferne nicht benötigte DotNet-Setup-Parameter und füge workflow_dispatch zu mehreren Workflow-Dateien hinzu ([27faaab](https://github.com/velvet-lab/xsdk/commit/27faaab3fd7c095766caed6bcb0b1165a05eaa26))
* **extensions:** filter out null namespaces when retrieving types ([59dc9c3](https://github.com/velvet-lab/xsdk/commit/59dc9c30dce925ba1580cd0cfb806747a272d8d4))
* format release.config.js for consistency and readability ([f554269](https://github.com/velvet-lab/xsdk/commit/f554269a86ea8fa08baca410c425fa6b02f3b424))
* füge Berechtigungen für Pull-Requests in den Workflow-Dateien hinzu ([3875991](https://github.com/velvet-lab/xsdk/commit/38759914f298338c4a3f400e84d49673c631c602))
* resolve merge conflicts in pnpm-lock.yaml ([78b2962](https://github.com/velvet-lab/xsdk/commit/78b29621374138ec5f3f70c2e250cdd86f3d6004))

## [1.1.0-next.4](https://github.com/velvet-lab/xsdk/compare/v1.1.0-next.3...v1.1.0-next.4) (2026-03-15)

### Features

* add license header validation and management tools ([26374f5](https://github.com/velvet-lab/xsdk/commit/26374f5ee86c48fd0fa735d13a6267f4a40a2185))
* **data:** add project descriptions for data layer integrations ([5b56f40](https://github.com/velvet-lab/xsdk/commit/5b56f40883a2401828cac70b834ed13fefb6e572))
* enhance NuGet package publishing with symbol package support ([e4fe6ed](https://github.com/velvet-lab/xsdk/commit/e4fe6eddb7a3697ddd154d309c7d5ec76e4cb1ee))

## [1.1.0-next.3](https://github.com/velvet-lab/xsdk/compare/v1.1.0-next.2...v1.1.0-next.3) (2026-03-14)

### Features

* **data:** implement soft-delete functionality in EntityFramework provider ([9d0d314](https://github.com/velvet-lab/xsdk/commit/9d0d314a958fa806f63adcfda5124e7dcb2e8687))

### Bug Fixes

* **controller:** rename GetSampleAsyncV2 to GetSampleAsync ([2e16f5c](https://github.com/velvet-lab/xsdk/commit/2e16f5cbba1ebd2dbfcee012d2991625e0acf85a))
* **extensions:** filter out null namespaces when retrieving types ([59dc9c3](https://github.com/velvet-lab/xsdk/commit/59dc9c30dce925ba1580cd0cfb806747a272d8d4))
* resolve merge conflicts in pnpm-lock.yaml ([78b2962](https://github.com/velvet-lab/xsdk/commit/78b29621374138ec5f3f70c2e250cdd86f3d6004))

## [1.1.0-next.2](https://github.com/velvet-lab/xsdk/compare/v1.1.0-next.1...v1.1.0-next.2) (2026-03-09)

### Features

* add clean, restore, build, and test recipes for .NET solutions ([164b09d](https://github.com/velvet-lab/xsdk/commit/164b09d5995ff8b7dd36f6054657f480277526d2))
* add formatting and check-format recipes for .NET solutions ([c3fe648](https://github.com/velvet-lab/xsdk/commit/c3fe648a7b69d21825ef33288c03b159bc0e8a65))
* add initial dependabot configuration for NuGet package updates ([b65e655](https://github.com/velvet-lab/xsdk/commit/b65e6552751a3a8cee3065f8ea9be93618ee830b))
* add linting recipes for MegaLinter in .NET project ([1ef76c4](https://github.com/velvet-lab/xsdk/commit/1ef76c436803db6ef36fc9f65acd8cb059c333c2))
* add maintenance recipes for .NET tool installation and updates ([3074784](https://github.com/velvet-lab/xsdk/commit/3074784bd4f4ef4fd0f601450113d8cac0bb14ba))
* add release and maintenance recipes for .NET tools management ([6e6b4fc](https://github.com/velvet-lab/xsdk/commit/6e6b4fca4222879a0ee4ccdc5be386eb1abc7054))
* add release recipe for publishing NuGet packages ([9798524](https://github.com/velvet-lab/xsdk/commit/979852432bacb2712107e6eb9f972dcff9b0b855))
* add sonar-build solution for SonarQube analysis with coverage ([529ea19](https://github.com/velvet-lab/xsdk/commit/529ea195037f1ca402775d639e195d9fe66c58fd))
* add workflows for linting, testing, sonar scanning, and feature branch builds ([7bfb958](https://github.com/velvet-lab/xsdk/commit/7bfb958d18be188ed7ca6adeb3f751d793fbd6d5))
* aktualisiere Workflows für SonarCloud-Analyse und Unit-Tests mit neuen Konfigurationen ([6fca7d1](https://github.com/velvet-lab/xsdk/commit/6fca7d1a16a2f1dc3e5d2f3bf1110ef427414eca))
* **data:** implement soft-delete functionality in EntityFramework provider ([5066afd](https://github.com/velvet-lab/xsdk/commit/5066afd8bb078f4faa4b540aff69ca730ade7277))
* enhance setup repository action with credential persistence ([3d294c6](https://github.com/velvet-lab/xsdk/commit/3d294c6d0b0a76d5c48f199cdc2501d7b8db6833))
* erweitere dependabot.yml zur Überwachung von NuGet, GitHub Actions und npm-Paketen ([367bbc5](https://github.com/velvet-lab/xsdk/commit/367bbc55f19e8d158ef962b6300faf3021360f3d))
* füge neue Befehle für .NET-Tools hinzu und verbessere die Formatierungs- und Linting-Rezepte ([32ee46c](https://github.com/velvet-lab/xsdk/commit/32ee46cb215be415b19d1a391ba3a00c7585d454))
* füge Release-Workflow hinzu und aktualisiere .NET Setup in den Unit-Tests und SonarCloud Workflows ([df6ded0](https://github.com/velvet-lab/xsdk/commit/df6ded0e8b7ec2b2a8ea1182c30c197a7cc6b560))
* füge Veröffentlichung der Testberichtzusammenfassung zu den Unit-Tests hinzu ([31d1052](https://github.com/velvet-lab/xsdk/commit/31d105219d31dfeebcd380e211a0debab1af1bee))
* füge Workflows für SonarCloud-Analyse und Unit-Tests hinzu ([1961bb8](https://github.com/velvet-lab/xsdk/commit/1961bb8366612a505fa5cfc721b7c33a8525b4ab))
* füge zusätzliche Badges zur README.md hinzu ([68de383](https://github.com/velvet-lab/xsdk/commit/68de383954fd29d1566012a6de1f8ab6414878c0))
* implement setup repository action for CI workflows ([4bd23d0](https://github.com/velvet-lab/xsdk/commit/4bd23d0892d4bb3e4e6bfca85ebdf21af10e4afa))
* **plugin:** add unit tests for various helper classes ([8c49b5d](https://github.com/velvet-lab/xsdk/commit/8c49b5d7ef018c542a4c9b5a1f23bf48965777f7))
* reorganize Justfile recipes for better structure ([361ad8a](https://github.com/velvet-lab/xsdk/commit/361ad8a41068e5153bb1bdd2765fd6bba562a4ac))
* update release workflow and Justfile for improved dependency management ([1bc4a6e](https://github.com/velvet-lab/xsdk/commit/1bc4a6e4f9cd246d9edd7c60c6fcb4874dc39dfe))

### Bug Fixes

* aktualisiere .NET-Version auf 8.0.x und füge Berechtigungen für Jobs hinzu ([77ee273](https://github.com/velvet-lab/xsdk/commit/77ee273162acab097c67f49a2203e187398d98af))
* aktualisiere Intervall für GitHub Actions auf wöchentlich und aktualisiere die Version des Testberichterstatters ([e80ea6a](https://github.com/velvet-lab/xsdk/commit/e80ea6a4925d0b217cd4b01dfc2860de69234297))
* aktualisiere Kommentare in dependabot.yml auf Englisch ([57b2224](https://github.com/velvet-lab/xsdk/commit/57b222483905419ad9952b46dd7bd9b37696a5cd))
* aktualisiere pnpm-Setup und Installationsbefehle in den Workflows ([e31e656](https://github.com/velvet-lab/xsdk/commit/e31e6567857675a5a8792119573fdea5f6f64fbe))
* aktualisiere Version des Testberichterstatters in den Unit-Tests ([d1c2693](https://github.com/velvet-lab/xsdk/commit/d1c26935d6bfd08b6dceecac14463283cc2fb093))
* **demos:** update package versions and remove unused references ([c261bda](https://github.com/velvet-lab/xsdk/commit/c261bda3a38bc1912b8a6102c2c044e76577f5e0))
* entferne nicht benötigte DotNet-Setup-Parameter und füge workflow_dispatch zu mehreren Workflow-Dateien hinzu ([27faaab](https://github.com/velvet-lab/xsdk/commit/27faaab3fd7c095766caed6bcb0b1165a05eaa26))
* format release.config.js for consistency and readability ([f554269](https://github.com/velvet-lab/xsdk/commit/f554269a86ea8fa08baca410c425fa6b02f3b424))
* füge Berechtigungen für Pull-Requests in den Workflow-Dateien hinzu ([3875991](https://github.com/velvet-lab/xsdk/commit/38759914f298338c4a3f400e84d49673c631c602))

# Changelog

## [1.1.0-next.1](https://github.com/velvet-lab/xsdk/compare/v1.0.0...v1.1.0-next.1) (2026-03-06)

### Features

* add release configuration and VS Code settings ([a781b10](https://github.com/velvet-lab/xsdk/commit/a781b10b0cdce9c02083bda87459a76d633341a1))
