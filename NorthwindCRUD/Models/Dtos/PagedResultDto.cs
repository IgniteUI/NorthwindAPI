namespace NorthwindCRUD.Models.Dtos
{
    public class PagedResultDto<T>
    {
        public T[] Items { get; set; }

        public int TotalRecordsCount { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }
    }
}
