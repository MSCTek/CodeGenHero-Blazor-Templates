namespace ArtistSite.Shared.DataService
{
    public interface IPageDataT<T> : IPageData
    {
        T Data { get; set; }
    }
}