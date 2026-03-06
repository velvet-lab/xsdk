using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewer56.Update.Interfaces;

namespace xSdk.Extensions.Package
{
    public interface IPackageProvider
    {
        IPackageResolver GetResolver();
    }
}
