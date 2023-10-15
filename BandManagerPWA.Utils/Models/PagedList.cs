﻿using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.Utils.Models
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 0;
        public bool HasNext => CurrentPage < TotalPages - 1;

        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageIndex, pageSize);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}