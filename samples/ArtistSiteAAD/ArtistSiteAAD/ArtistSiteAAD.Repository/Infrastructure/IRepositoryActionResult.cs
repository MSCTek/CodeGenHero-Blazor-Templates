﻿using System;

namespace ArtistSiteAAD.Repository.Infrastructure
{
    public interface IRepositoryActionResult<T> where T : class
    {
        T Entity { get; }
        Exception Exception { get; }
        Enums.RepositoryActionStatus Status { get; }
    }
}