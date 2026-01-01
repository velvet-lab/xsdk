using AutoMapper;
using NLog;

namespace xSdk.Data.Converters.Mapper
{
    public static class SemVerConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public sealed class ToModelProperty : IValueConverter<SemVer, string>
        {
            public string Convert(SemVer sourceMember, ResolutionContext context)
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
        }

        public sealed class ToEntityProperty : IValueConverter<string, SemVer>
        {
            public SemVer Convert(string sourceMember, ResolutionContext context)
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
    }
}
