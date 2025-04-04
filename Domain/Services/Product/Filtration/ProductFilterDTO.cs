namespace Domain
{
    public class ProductFilterDTO
    {
        public string? SearchQuery { get; set; }

        public Guid? CategoryId { get; set; }

        public bool? IsActive { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public ProductSortBy? SortBy { get; set; }

        public SortDirection? SortDirection { get; set; }
    }


}