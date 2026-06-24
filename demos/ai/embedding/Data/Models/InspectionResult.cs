using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Demos.Data.Models;

public class InspectionResult
{
    public bool IsReady { get; set; }
    public string Reason { get; set; } = string.Empty;
}
