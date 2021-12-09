namespace $safeprojectname$.DataService
{
    public interface IHttpCallResultCGHT<T> : IHttpCallResultCGH
    {
        T Data { get; set; }
    }
}