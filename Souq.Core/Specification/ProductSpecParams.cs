using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
    public class ProductSpecParams
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? Sort { get; set; } // priceAsc, priceDesc, etc.
        public string? Search { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }

        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
