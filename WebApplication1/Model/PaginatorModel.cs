namespace WebApplication1.Model
{
    public class PaginatorModel<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }

        public PaginatorModel(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalCount = count;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PaginatorModel<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatorModel<T>(items, count, pageIndex, pageSize);
        }
    }
}
