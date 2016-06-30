using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CH.Toolbox
{

    public class TranslationResult
    {

        [JsonProperty("translation")]
        public string[] Translation { get; set; }

        [JsonProperty("basic")]
        public Basic Basic { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("web")]
        public Web[] Web { get; set; }
    }

}
