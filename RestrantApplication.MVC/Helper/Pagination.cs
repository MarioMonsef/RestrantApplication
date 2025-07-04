﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestrantApplication.MVC.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageNumber, int pageSize, int? totalCount, IReadOnlyList<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
        public Pagination() {  }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalCount { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public bool HasNextPage => (PageNumber * PageSize) < TotalCount;
        public bool HasPreviousPage => PageNumber > 1;
    }
}
