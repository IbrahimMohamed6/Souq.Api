﻿namespace Souq.Api.Helper.Paginitions
{
    public class Pagination<T>
    {
        public Pagination(int pageIndex, int pageSize, IReadOnlyList<T> data, int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
