using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Bloqs.Models
{
    public class ContainerCreateModel
    {
        [Required]
        [RegularExpression(Container.UsableNamePattern, ErrorMessage = "Is only half-width alphanumeric characters and hyphens (-) can be used")]
        [DisplayName("Container Name")]
        public string Name { get; set; }

        [DisplayName("Anyone can download blobs in this container.")]
        public bool IsPublic { get; set; }

        public Container ToContainer(string owner)
        {
            var container = new Container
            {
                Name = Name,
                IsPublic = IsPublic,
                Owner = owner,
                CreatedUtcDateTime = DateTime.UtcNow,
                LastModifiedUtcDateTime = DateTime.UtcNow,
                PrimaryAccessKey = ContainerKeyEditModel.CreateNewAccessKeyString(),
                SecondaryAccessKey = ContainerKeyEditModel.CreateNewAccessKeyString(),
            };

            return container;
        }
    }
    
    public class ContainerEditModel
    {
        [ReadOnly(true)]
        [DisplayName("Container Name")]
        public string Name { get; set; }

        [DisplayName("Anyone can download blobs in this container.")]
        public bool IsPublic { get; set; }
    }

    public class ContainerKeyEditModel
    {
        [ReadOnly(true)]
        [DisplayName("Container Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Primary Access Key")]
        public string PrimaryAccessKey { get; set; }

        [Required]
        [DisplayName("Secondary Access Key")]
        public string SecondaryAccessKey { get; set; }

        public static string CreateNewAccessKeyString()
        {
            var sha = SHA256.Create();
            var byteValue = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            var hash = sha.ComputeHash(byteValue);

            var buf = new StringBuilder();

            foreach (var t in hash)
            {
                buf.AppendFormat("{0:x2}", t);
            }

            return buf.ToString();
        }
    }

    public class ContainerViewModel
    {
        [DisplayName("Container Name")]
        public string Name { get; set; }

        [DisplayName("Accessibility")]
        public bool IsPublic { get; set; }

        [DisplayName("Accessibility")]
        public string Accessibility
        {
            get { return IsPublic ? "Public" : "Private"; }
        }

        [DisplayName("Owner")]
        public string Owner { get; set; }

        [DisplayName("Created Date (UTC)")]
        public DateTime CreatedUtcDateTime { get; set; }

        [DisplayName("Last Updated Date (UTC)")]
        public DateTime LastModifiedUtcDateTime { get; set; }

        public ContainerViewModel(Container container)
        {
            Name = container.Name;
            IsPublic = container.IsPublic;
            Owner = container.Owner;
            CreatedUtcDateTime = container.CreatedUtcDateTime;
            LastModifiedUtcDateTime = container.LastModifiedUtcDateTime;
        }
    }

    public class ContainerDeleteModel : ContainerViewModel
    {
        public ContainerDeleteModel() : base(new Container())
        {
        }

        public ContainerDeleteModel(Container container) : base(container)
        {
        }

        [Required]
        [DisplayName("Please type in the name of the container to confirm.")]
        [Compare("Name", ErrorMessage = "Container name and confirm name do not match.")]
        public string ConfirmName { get; set; }
    }
}