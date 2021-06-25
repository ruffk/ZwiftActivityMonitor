using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZwiftActivityMonitorV2
{
    public class Collector : ConfigItemBase, ICloneable
    {
        public string Name { get; set; }
        public string DurationDesc { get; set; }
        public int DurationSecs { get; set; }
        public string FieldAvgDesc { get; set; }
        public string FieldAvgMaxDesc { get; set; }
        public string FieldFtpDesc { get; set; }

        public Collector()
        {
        }

        [JsonIgnore]
        public DurationType DurationType { get { return Enum.Parse<DurationType>(this.DurationDesc); } }

        [JsonIgnore]
        public FieldUomType FieldAvgType { get { return Enum.Parse<FieldUomType>(this.FieldAvgDesc); } }

        [JsonIgnore]
        public FieldUomType FieldAvgMaxType { get { return Enum.Parse<FieldUomType>(this.FieldAvgMaxDesc); } }

        [JsonIgnore]
        public FieldUomType FieldFtpType { get { return Enum.Parse<FieldUomType>(this.FieldFtpDesc); } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
