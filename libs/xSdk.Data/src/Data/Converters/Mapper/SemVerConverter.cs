using NLog;

namespace xSdk.Data.Converters.Mapper;

public static class SemVerConverter
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static string Convert(SemVer sourceMember)
    {
        string result = default;
        try
        {
            if (sourceMember != null)
            {
                result = sourceMember.Version;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Version could not converted. See further Log for further Details");
            if (ex.InnerException != null)
                logger.Info(ex.InnerException.Message);

            throw;
        }

        return result;
    }



    public static SemVer Convert(string sourceMember)
    {
        SemVer result = default;

        try
        {
            if (!string.IsNullOrEmpty(sourceMember))
            {
                result = new SemVer(sourceMember);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Version could not converted. See further Log for further Details");
            if (ex.InnerException != null)
                logger.Info(ex.InnerException.Message);

            throw;
        }

        return result;
    }

}
