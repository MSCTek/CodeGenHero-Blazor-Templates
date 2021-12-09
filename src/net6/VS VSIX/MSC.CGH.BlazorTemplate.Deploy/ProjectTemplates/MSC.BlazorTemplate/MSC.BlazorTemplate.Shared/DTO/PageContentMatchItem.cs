using $safeprojectname$.Constants;

#nullable disable

namespace $safeprojectname$.DTO
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