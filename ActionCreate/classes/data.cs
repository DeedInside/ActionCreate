using ActionCreate.classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCreate
{
    public class data
    {
        public data(List<Annotation> annotations, double duration, string subset, string url)
        {
            Annotations = annotations;
            Duration = duration;
            Subset = subset;
            Url = url;
        }

        [JsonProperty("annotations")]
        public List<Annotation> Annotations { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; } = 0;

        [JsonProperty("subset")]
        public string Subset { get; set; } = "training";

        [JsonProperty("url")]
        public string Url { get; set; } = "";
    }

    /*


            {
                "annotations": [
                    {
                        "label": "some_label",
                        "segment": [a, b],
                    },
                    ...
                ],
                "duration": "some_duration_in_seconds",
                "subset": "train_val_test",
                "url": "some_url",
            }

    */
}
