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
