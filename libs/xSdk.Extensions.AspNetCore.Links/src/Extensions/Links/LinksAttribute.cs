namespace xSdk.Extensions.Links
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LinksAttribute : Attribute
    {
        public LinksAttribute(string policyName)
        {
            this.PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

        public string PolicyName { get; internal set; }
    }
}
