namespace ArtistSiteAAD.Shared.DataService
{
    public interface IHttpCallResultCGHT<T> : IHttpCallResultCGH
    {
        T Data { get; set; }
    }
}