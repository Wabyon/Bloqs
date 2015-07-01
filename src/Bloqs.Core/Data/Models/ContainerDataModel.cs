using System;
using Newtonsoft.Json;

namespace Bloqs.Data.Models
{
    internal class ContainerDataModel
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string Metadata { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime LastModifiedUtcDateTime { get; set; }
        
        public Container ToContainer(Account account)
        {
            var ret = new Container(account)
            {
                Id = Id,
                Name = Name,
                IsPublic = IsPublic,
                CreatedUtcDateTime = CreatedUtcDateTime,
                LastModifiedUtcDateTime = LastModifiedUtcDateTime,
            };
            ret.Metadata.Add(JsonConvert.DeserializeObject<Metadata>(Metadata));

            return ret;
        }
    }
}
