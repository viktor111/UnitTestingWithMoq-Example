namespace ProductsCatalog.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }
    }
}
