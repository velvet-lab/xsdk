using xSdk.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Demos.Data
{
    [Table("second")]
    public sealed class SecondEntity : EFEntity, ISampleEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }
    }
}
