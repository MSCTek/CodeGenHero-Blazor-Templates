﻿using System;
using static ArtistSiteAAD.Repository.Infrastructure.Enums;

namespace ArtistSiteAAD.Repository.Infrastructure
{
    public class RepositoryActionResult<T> : IRepositoryActionResult<T> where T : class
    {
        public RepositoryActionResult(T entity, RepositoryActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        public RepositoryActionResult(T entity, RepositoryActionStatus status, Exception exception) : this(entity, status)
        {
            Exception = exception;
        }

        public T Entity { get; private set; }
        public Exception Exception { get; private set; }
        public RepositoryActionStatus Status { get; private set; }
    }
}