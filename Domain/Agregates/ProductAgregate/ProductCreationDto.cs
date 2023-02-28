namespace Domain.Agregates.ProductAgregate
{
    public class ProductChangeDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }
    }
}
