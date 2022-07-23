using ArtistSite.Shared.Constants;

#nullable disable

namespace ArtistSite.Shared.DTO
{
    public partial class PageContentMatchItem
    {
        public PageContentMatchItem()
        {
        }

        public string PageContent { get; set; }

        public Enums.PageContentElement PageContentElement { get; set; }

        public int Score { get; set; }
    }
}