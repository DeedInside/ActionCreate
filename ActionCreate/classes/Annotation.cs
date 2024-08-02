using Newtonsoft.Json;
using System.Collections.Generic;

namespace ActionCreate.classes
{
    public class Annotation
    {
        public Annotation(List<double> segment)
        {
            Segment = segment;
        }

        [JsonProperty("label")]
        public string Label { get; set; } = "Crash";

        [JsonProperty("segment")]
        public List<double> Segment { get; set; } = new List<double>() { 0, 0 };
    }
}
