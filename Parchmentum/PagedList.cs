using System;
using System.Collections.ObjectModel;

namespace Parchmentum;

public class PagedList<T>
{
    public static PagedList<T> For<TQuery>(IQueryable<TQuery> query, int page, int pageSize, Func<TQuery, T> mappingMethod)
    {
        if (page < 1)
            throw new ArgumentException("Page cannot be less than one.", nameof(page));
        if (pageSize < 1)
            throw new ArgumentException("Page size cannot be less than one.", nameof(pageSize));

        var totalItemCount = query.Count();

        var list = new List<T>(Math.Min(pageSize, totalItemCount));

        if (totalItemCount > 0)
        {
            // avoid the Skip(0) performance hit problem
            var skipCount = (page - 1) * pageSize;
            var pagedQuery = skipCount == 0 ? query.Take(pageSize) : query.Skip(skipCount).Take(pageSize);

            foreach (var result in pagedQuery)
                list.Add(mappingMethod(result));
        }

        return new PagedList<T>(page, pageSize, totalItemCount, list);
    }

    public static PagedList<T> For(IList<T> source, int page, int pageSize, int totalItemCount)
    {
        if (page < 1)
            throw new ArgumentException("Page cannot be less than one.", nameof(page));
        if (pageSize < 1)
            throw new ArgumentException("Page size cannot be less than one.", nameof(pageSize));

        return new PagedList<T>(page, pageSize, totalItemCount, source);
    }

    private PagedList(int page, int pageSize, int totalItemCount, IList<T> list)
    {
        Page = page;
        PageSize = pageSize;
        TotalItemCount = totalItemCount;
        List = new ReadOnlyCollection<T>(list);
    }

    public int TotalItemCount { get; }

    /// <summary>Page number starting at 1</summary>
    public int Page { get; }

    /// <summary>Number of items returned per page</summary>
    public int PageSize { get; }

    public int PageCount => (int)Math.Ceiling(TotalItemCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < PageCount;

    public IReadOnlyList<T> List { get; }
}

public static class PagedListExtensions
{
    /// <param name="source"></param>
    /// <param name="page">1 to n</param>
    /// <param name="pageSize">1 to n</param>
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int? pageSize = 25)
    {
        return PagedList<T>.For(source, page, pageSize ?? 25, x => x);
    }

    /// <param name="source"></param>
    /// <param name="page">1 to n</param>
    /// <param name="mappingMethod">map to a different result type - AFTER executing paged query</param>
    public static PagedList<T> ToPagedList<TQuery, T>(this IQueryable<TQuery> source, int page, Func<TQuery, T> mappingMethod)
    {
        return PagedList<T>.For(source, page, 25, mappingMethod);
    }

    /// <param name="source"></param>
    /// <param name="page">1 to n</param>
    /// <param name="pageSize">1 to n</param>
    /// <param name="mappingMethod">map to a different result type - AFTER executing paged query</param>
    public static PagedList<T> ToPagedList<TQuery, T>(this IQueryable<TQuery> source, int page, int pageSize, Func<TQuery, T> mappingMethod)
    {
        return PagedList<T>.For(source, page, pageSize, mappingMethod);
    }

    /// <param name="source"></param>
    /// <param name="page">1 to n</param>
    /// <param name="pageSize">1 to n</param>
    /// <param name="totalItemCount">Total items including all pages</param>
    public static PagedList<T> ToPagedList<T>(this IList<T> source, int page = 1, int? pageSize = 25, int? totalItemCount = null)
    {
        return PagedList<T>.For(source, page, pageSize ?? 25, totalItemCount ?? source.Count);
    }

    /// <param name="source"></param>
    /// <param name="page">1 to n</param>
    /// <param name="pageSize">1 to n</param>
    /// <param name="totalItemCount">Total items including all pages</param>
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int page = 1, int? pageSize = 25, int? totalItemCount = null)
    {
        var list = source.ToList();
        return PagedList<T>.For(list, page, pageSize ?? 25, totalItemCount ?? list.Count);
    }
}
