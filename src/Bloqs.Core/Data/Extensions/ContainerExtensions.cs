using Bloqs.Data.Models;

namespace Bloqs.Data.Extensions
{
    internal static class ContainerExtensions
    {
        public static ContainerDataModel ToDataModel(this Container container)
        {
            return new ContainerDataModel
            {
                Id = container.Id,
                AccountId = container.Account.Id,
                Name = container.Name,
                IsPublic = container.IsPublic,
                Metadata = container.Metadata.ToJson(),
                CreatedUtcDateTime = container.CreatedUtcDateTime,
                LastModifiedUtcDateTime = container.LastModifiedUtcDateTime,
            };
        }
    }
}