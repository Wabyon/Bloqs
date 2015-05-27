using AutoMapper;
using Bloqs.Internals;
using Bloqs.Models;

namespace Bloqs
{
    public class MappingConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<Container, ContainerResponseModel>();
            Mapper.CreateMap<ContainerRequestModel, Container>()
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });

            Mapper.CreateMap<BlobAttributes, BlobResponseModel>();
            Mapper.CreateMap<BlobRequestModel, BlobAttributes>()
                .ForMember(d => d.Properties, o => o.MapFrom(s => new BlobProperties
                {
                    CacheControl = s.Properties.CacheControl.EmptyToNull(),
                    ContentDisposition = s.Properties.ContentDisposition.EmptyToNull(),
                    ContentEncoding = s.Properties.ContentEncoding.EmptyToNull(),
                    ContentLanguage = s.Properties.ContentLanguage.EmptyToNull(),
                    ContentMD5 = s.Properties.ContentMD5.EmptyToNull(),
                    ContentType = s.Properties.ContentType.EmptyToNull(),
                    LastModifiedUtcDateTime = s.Properties.LastModifiedUtcDateTime,
                    Length = s.Properties.Length
                }))
                .AfterMap((s, d) =>
                {
                    d.Metadata.Clear();
                    foreach (var metadata in s.Metadata)
                    {
                        d.Metadata.Add(metadata.Key, metadata.Value);
                    }
                });
        }
    }
}