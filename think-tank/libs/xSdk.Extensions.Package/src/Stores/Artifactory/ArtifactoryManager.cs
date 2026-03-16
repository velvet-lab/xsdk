/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//using xSdk.Data;
//using Microsoft.Extensions.Logging;
//using RestSharp;
//using RestSharp.Authenticators;
//using xSdk.Extensions.Variable;

//namespace xSdk.Extensions.Package.Stores.Artifactory
//{
//    internal partial class ArtifactoryManager
//    {
//        private readonly PackageSetup _setup;
//        private readonly EnvironmentSetup _envSetup;
//        private readonly ILogger<ArtifactoryManager> _logger;

//        public ArtifactoryManager(PackageSetup setup, EnvironmentSetup envSetup, ILogger<ArtifactoryManager> logger)
//        {
//            _setup = setup ?? throw new ArgumentNullException(nameof(setup));
//            _envSetup = envSetup ?? throw new ArgumentNullException(nameof(envSetup));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        //public Dependencies LoadDependenciesFromPath(string path)
//        //{
//        //    var packageJsonFiles = Glob.Files(path, "**/package.json");

//        //    var collected = new Dependencies();

//        //    foreach (var packageJsonFile in packageJsonFiles)
//        //    {
//        //        var packageJsonFileInfo = new FileInfo(Path.Combine(path, packageJsonFile));
//        //        if (packageJsonFileInfo.Exists)
//        //        {
//        //            var json = File.ReadAllText(packageJsonFileInfo.FullName);
//        //            var packageJson = JsonSerializer.Deserialize<PackageJson>(json, JsonHelper.GetSerializerOptions());
//        //            if (packageJson is not null)
//        //            {
//        //                packageJson.File = packageJsonFileInfo.Name;
//        //                packageJson.Root = packageJsonFileInfo.Directory.FullName;

//        //                collected.Merge(packageJson.AutomationHub.Dependencies);
//        //            }
//        //        }
//        //    }

//        //    return collected;
//        //}




//        private IRestClient CreateRestClient(string message = default, bool withAuth = true)
//        {
//            IProgress<double> progress = null;
//            IAuthenticator authenticator = null;

//            if ((_envSetup.Mode == RunningMode.Tool
//                || _envSetup.Mode == RunningMode.Console
//                || _envSetup.Mode == RunningMode.PowerCli)
//                && !_envSetup.IsConsoleDisabled)
//            {
//                progress = new ConsoleProgress(_consoleSvc);

//                if (!string.IsNullOrEmpty(message))
//                {
//                    _consoleSvc
//                        .SetMessage(message)
//                        .DisableLogger()
//                        .WriteProgress();
//                }

//                if (withAuth)
//                {
//                    authenticator = GetAuthenticator();
//                }
//            }

//            return RestClientBuilder.CreateRestClient(_setup.BaseUrl, authenticator, progress);
//        }

//        private HttpBasicAuthenticator GetAuthenticator()
//        {
//            var creds = CredentialManager.LoadCredentials<PackageCredentials>(Globals.SecurityContext);
//            if(creds == null)
//            {
//                throw new SdkException("No credentials found to access artifactory");
//            }

//            var user = creds.ArtifactoryUser;
//            var token = creds.ArtifactoryToken;

//            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(token))
//            {
//                return new HttpBasicAuthenticator(user, token);
//            }

//            if (string.IsNullOrEmpty(user))
//                throw new SdkException("No Artifactory User given");

//            if (string.IsNullOrEmpty(token))
//                throw new SdkException("No Artifactory token given");

//            throw new SdkException("Authenticator could not created");
//        }

//        internal (string repository, string location) ExtractRepositoryAndLocation(PackageModel sourcePackage)
//        {
//            var repository = Globals.DefaultRepository;
//            var location = Globals.DefaultLocation;
//            if (sourcePackage.DevelopmentKit != null && sourcePackage.DevelopmentKit.Artifactory != null)
//            {
//                repository = sourcePackage.DevelopmentKit.Artifactory.Repository;
//                location = sourcePackage.DevelopmentKit.Artifactory.Location;
//            }

//            if (string.IsNullOrEmpty(repository))
//            {
//                repository = Globals.DefaultRepository;
//            }

//            if (string.IsNullOrEmpty(location))
//            {
//                location = Globals.DefaultLocation;
//            }

//            return (repository, location);
//        }
//    }
//}
