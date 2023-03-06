namespace Domain.Aggregates.ProductAggregate
{
    public class ProductChangeDto
    {
        public string? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }
    }
}
