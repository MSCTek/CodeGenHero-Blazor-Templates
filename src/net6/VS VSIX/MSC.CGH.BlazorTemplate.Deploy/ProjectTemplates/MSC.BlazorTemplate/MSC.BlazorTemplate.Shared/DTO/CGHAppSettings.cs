#nullable disable

using System;

namespace CodeGenHeroBlazor6.Shared.DTO
{
    public class CGHAppSettings
    {
        public string ApiBaseAddress { get; set; }
        public string ImagesBaseAddress { get; set; }
        public bool IsWebAssembly { get; set; }
        public Guid SiteIndexSiteId { get; set; }
    }
}