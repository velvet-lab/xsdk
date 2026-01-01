namespace xSdk.Data
{
    public abstract class EFModel : Model, IModel<GuidStringPK, string>
    {
        public EFModel()
        {
            this.PrimaryKey = new GuidStringPK();
        }

        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }
    }
}
