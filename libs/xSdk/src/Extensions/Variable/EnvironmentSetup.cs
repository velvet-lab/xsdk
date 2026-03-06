using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Variable
{
    [VariableNoPrefix]
    public sealed partial class EnvironmentSetup : Setup, IEnvironmentSetup
    {
        [Variable(
            name: Definitions.AppName.Name,
            helpText: Definitions.AppName.HelpText,
            defaultValue: Definitions.AppName.DefaultValue,
            resourceNames: new[] { "app.name" },
            hidden: true
        )]
        public string AppName
        {
            get => this.ReadValue<string>(Definitions.AppName.Name);
            set => this.SetValue(Definitions.AppName.Name, value);
        }

        [Variable(
            name: Definitions.AppDescription.Name,
            helpText: Definitions.AppDescription.HelpText,
            resourceNames: new[] { "app.description" },
            hidden: true
        )]
        public string AppDescription
        {
            get => this.ReadValue<string>(Definitions.AppDescription.Name);
            set => this.SetValue(Definitions.AppDescription.Name, value);
        }

        [Variable(
            name: Definitions.AppCompany.Name,
            helpText: Definitions.AppCompany.HelpText,
            defaultValue: Definitions.AppCompany.DefaultValue,
            resourceNames: new[] { "app.company" },
            hidden: true
        )]
        public string AppCompany
        {
            get => this.ReadValue<string>(Definitions.AppCompany.Name);
            set => this.SetValue(Definitions.AppCompany.Name, value);
        }

        [Variable(
            name: Definitions.AppPrefix.Name,
            helpText: Definitions.AppPrefix.HelpText,
            defaultValue: Definitions.AppPrefix.DefaultValue,
            resourceNames: new[] { "app.prefix" },
            hidden: true
        )]
        public string AppPrefix
        {
            get => this.ReadValue<string>(Definitions.AppPrefix.Name);
            set => this.SetValue(Definitions.AppPrefix.Name, value);
        }

        [Variable(name: Definitions.AppVersion.Name, helpText: Definitions.AppVersion.HelpText, resourceNames: new[] { "app.version" }, hidden: true)]
        public string AppVersion
        {
            get => this.ReadValue<string>(Definitions.AppVersion.Name);
            set => this.SetValue(Definitions.AppVersion.Name, value);
        }

        [Variable(
            name: DefaultCommandSettings.Definitions.Stage.Name,
            template: DefaultCommandSettings.Definitions.Stage.Template,
            helpText: DefaultCommandSettings.Definitions.Stage.HelpText,
            defaultValue: DefaultCommandSettings.Definitions.Stage.DefaultValue,
            resourceNames: new[] { "{{app.prefix}}.environment.stage", "deployment.environment" }
        )]
        public Stage Stage
        {
            get => this.ReadValue<Stage>(DefaultCommandSettings.Definitions.Stage.Name);
            set => this.SetValue(DefaultCommandSettings.Definitions.Stage.Name, value);
        }

        [Variable(
            name: DefaultCommandSettings.Definitions.Demo.Name,
            template: DefaultCommandSettings.Definitions.Demo.Template,
            helpText: DefaultCommandSettings.Definitions.Demo.HelpText,
            resourceNames: new[] { "{{app.prefix}}.environment.demo" }
        )]
        public bool IsDemo
        {
            get => this.ReadValue<bool>(DefaultCommandSettings.Definitions.Demo.Name);
            set => this.SetValue(DefaultCommandSettings.Definitions.Demo.Name, value);
        }

        [Variable(
            name: DefaultCommandSettings.Definitions.ContentRoot.Name,
            template: DefaultCommandSettings.Definitions.ContentRoot.Template,
            helpText: DefaultCommandSettings.Definitions.ContentRoot.HelpText
        )]
        public string ContentRoot
        {
            get => this.ReadValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
            set => this.SetValue(DefaultCommandSettings.Definitions.ContentRoot.Name, value);
        }

        [Variable(
            name: DefaultCommandSettings.Definitions.LogLevel.Name,
            template: DefaultCommandSettings.Definitions.LogLevel.Template,
            helpText: DefaultCommandSettings.Definitions.LogLevel.HelpText,
            defaultValue: DefaultCommandSettings.Definitions.LogLevel.DefaultValue
        )]
        public string LogLevel
        {
            get => this.ReadValue<string>(DefaultCommandSettings.Definitions.LogLevel.Name);
            set => this.SetValue(DefaultCommandSettings.Definitions.LogLevel.Name, value);
        }

        public SemVer Version { get; private set; }

        protected override void ValidateSetup()
        {
            var result = string.IsNullOrEmpty(ServiceName);

            this.ValidateMember(x => string.IsNullOrEmpty(x.ServiceName), "Unique Service Name is missing");
            this.ValidateMember(x => string.IsNullOrEmpty(x.ServiceNamespace), "Unique Service Namespace is missing");
            this.ValidateMember(x => string.IsNullOrEmpty(x.ServiceVersion), "Unique Service Version is missing");
            this.ValidateMember(x => string.IsNullOrEmpty(x.ServiceFullName), "Unique Service Fullname is missing");
        }

        protected override void Initialize()
        {
            var version = EnvironmentSetupExtensions.GetVersion(typeof(EnvironmentSetup));
            Version = version;
            AppVersion = version.ToString();

            ContentRoot = this.DetermineContentRoot();

            InitializeSystem();
            InitializeService();
        }
    }
}
