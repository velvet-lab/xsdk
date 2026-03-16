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

using NLog;

namespace xSdk.Data.Converters.Mapper;

public static class SemVerConverter
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
            _logger.Error(ex, "Version could not converted. See further Log for further Details");
            if (ex.InnerException != null)
                _logger.Info(ex.InnerException.Message);

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
            _logger.Error(ex, "Version could not converted. See further Log for further Details");
            if (ex.InnerException != null)
                _logger.Info(ex.InnerException.Message);

            throw;
        }

        return result;
    }

}
