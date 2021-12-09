namespace $safeprojectname$.DataService
{
    public interface IPageDataT<T> : IPageData
    {
        T Data { get; set; }
    }
}