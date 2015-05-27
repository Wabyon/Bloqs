using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bloqs.Data.Models
{
    internal class AccountDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string PrimaryAccessKey { get; set; }
        public string SecondaryAccessKey { get; set; }
        public string CryptKey { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime LastModifiedUtcDateTime { get; set; }
        public StorageType StorageType { get; set; }
        public string Storages { get; set; }

        public Account ToAccount()
        {
            var account = new Account
            {
                Id = Id,
                Name = Name,
                Owner = Owner,
                PrimaryAccessKey = PrimaryAccessKey,
                SecondaryAccessKey = SecondaryAccessKey,
                CryptKey = CryptKey,
                CreatedUtcDateTime = CreatedUtcDateTime,
                LastModifiedUtcDateTime = LastModifiedUtcDateTime,
                StorageType = StorageType,
            };

            if (Storages == null) return account;

            var xml = new XmlDocument();
            xml.LoadXml(Storages);
            var xmlJson = Regex.Replace(JsonConvert.SerializeXmlNode(xml), "(?<=\")(@)(?!.*\":\\s )", "", RegexOptions.IgnoreCase);
            var jObject = JObject.Parse(xmlJson);
            var targetNode = jObject["Storages"]["StorageDataModel"];
            if (targetNode.Type == JTokenType.Array)
            {
                var datamodels = JsonConvert.DeserializeObject<IEnumerable<StorageDataModel>>(targetNode.ToString());
                foreach (var datamodel in datamodels)
                {
                    account.Storages.Add(datamodel.ToStorage());
                }
            }
            else
            {
                var datamodel = JsonConvert.DeserializeObject<StorageDataModel>(targetNode.ToString());
                account.Storages.Add(datamodel.ToStorage());
            }
            return account;
        }
    }
}
