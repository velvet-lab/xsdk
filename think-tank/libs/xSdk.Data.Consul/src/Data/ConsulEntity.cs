namespace xSdk.Data
{
    public class ConsulEntity : Entity, IEntity<ConsulPrimaryKey, string>
    {
        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }

        public string Key
        {
            get => Id;
            set => Id = value;
        }

        public string Value { get; set; }
    }
}
