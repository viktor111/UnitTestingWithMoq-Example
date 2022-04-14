﻿namespace ProductsCatalog.Dtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }
    }
}
