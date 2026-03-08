using xSdk.Data;

namespace xSdk.Demos.Data;

internal class SampleDatabase
{
    private List<SampleModel> _database = new List<SampleModel>();
    private static SampleDatabase _singleton;

    public static List<SampleModel> Load()
    {
        if (_singleton == null)
        {
            _singleton = new SampleDatabase();
            _singleton._database = FakeGenerator.GenerateList<SampleModelExamples, SampleModel>(10).ToList();
        }

        return _singleton._database;
    }

    public static List<SampleModel> Save(SampleModel model)
    {
        if (_singleton == null)
        {
            _singleton = new SampleDatabase();
            _singleton._database.Add(model);
        }

        return _singleton._database;
    }
}
