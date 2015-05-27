using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bloqs
{
    /// <summary></summary>
    public class Metadata : Dictionary<string, string>
    {
        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        internal void Add(Metadata metadata)
        {
            foreach (var meta in metadata)
            {
                Add(meta.Key, meta.Value);
            }
        }
    }
}
