﻿namespace ArtistSiteAAD.Shared.DataService
{
    public interface IPageDataT<T> : IPageData
    {
        T Data { get; set; }
    }
}