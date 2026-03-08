using xSdk.Data;

namespace xSdk.Demos.Data;

internal class SampleDatabase
{
    private List<SampleModel> database = new List<SampleModel>();
    private static SampleDatabase singleton;

    public static List<SampleModel> Load()
    {
        if (singleton == null)
        {
            singleton = new SampleDatabase();
            singleton.database = FakeGenerator.GenerateList<SampleModelExamples, SampleModel>(10).ToList();
        }

        return singleton.database;
    }

    public static List<SampleModel> Save(SampleModel model)
    {
        if (singleton == null)
        {
            singleton = new SampleDatabase();
            singleton.database.Add(model);
        }

        return singleton.database;
    }
}
