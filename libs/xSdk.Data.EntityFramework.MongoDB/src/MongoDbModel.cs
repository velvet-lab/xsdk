namespace xSdk.Data
{
    public class MongoDbModel : Model, IModel<MongoDbModelPK, string>
    {
        public MongoDbModel()
        {
            this.PrimaryKey = new MongoDbModelPK();
        }

        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }
    }
}
