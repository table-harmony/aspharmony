using DataAccessLayer.Entities;
using System;
namespace PresentationLayer.Models {
    public class PaginatedList<T>(List<T> items, int count, int pageIndex, int pageSize) {
        public List<T> Items { get; } = items;
        public int PageIndex { get; } = pageIndex;
        public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
        public int TotalItems { get; } = count;
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize) {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static PaginatedList<T> Create(IOrderedEnumerable<T> source, int pageIndex, int pageSize) {
            return Create(source.AsEnumerable(), pageIndex, pageSize);
        }
    }
}