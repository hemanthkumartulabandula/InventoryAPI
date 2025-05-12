using InventoryAPI.Models;

namespace InventoryAPI.Data
{
    public static class DataSeeder
    {
        public static async Task SeedSampleDataAsync(InventoryContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Electronics" },
                    new Category { Name = "Food Items" },
                    new Category { Name = "Sporting Goods" },
                    new Category { Name = "Beverages" },
                    new Category { Name = "Clothing" },
                    new Category { Name = "Stationery" },
                    new Category { Name = "Home Appliances" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Suppliers.Any())
            {
                context.Suppliers.AddRange(
                    new Supplier { Name = "HP", ContactEmail = "support@hp.com", Phone = "1234567890" },
                    new Supplier { Name = "Nestle", ContactEmail = "info@nestle.com", Phone = "2345678901" },
                    new Supplier { Name = "Adidas", ContactEmail = "contact@adidas.com", Phone = "3456789012" },
                    new Supplier { Name = "Apple", ContactEmail = "hello@apple.com", Phone = "4567890123" },
                    new Supplier { Name = "Levis", ContactEmail = "support@levis.com", Phone = "5678901234" },
                    new Supplier { Name = "Classmate", ContactEmail = "sales@classmate.com", Phone = "6789012345" },
                    new Supplier { Name = "Samsung", ContactEmail = "care@samsung.com", Phone = "7890123456" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var electronics = context.Categories.FirstOrDefault(c => c.Name == "Electronics");
                var food = context.Categories.FirstOrDefault(c => c.Name == "Food Items");
                var sports = context.Categories.FirstOrDefault(c => c.Name == "Sporting Goods");

                var hp = context.Suppliers.FirstOrDefault(s => s.Name == "HP");
                var nestle = context.Suppliers.FirstOrDefault(s => s.Name == "Nestle");
                var adidas = context.Suppliers.FirstOrDefault(s => s.Name == "Adidas");

                if (electronics != null && food != null && sports != null &&
                    hp != null && nestle != null && adidas != null)
                {
                    context.Products.AddRange(
                        new Product
                        {
                            Name = "HP Laptop",
                            Description = "Pavilion 15, 8GB RAM",
                            Quantity = 20,
                            Price = 580,
                            CategoryId = electronics.CategoryId,
                            SupplierId = hp.SupplierId
                        },
                        new Product
                        {
                            Name = "Chocolate Bar",
                            Description = "Nestle Crunch 50g",
                            Quantity = 100,
                            Price = 1.50m,
                            CategoryId = food.CategoryId,
                            SupplierId = nestle.SupplierId
                        },
                        new Product
                        {
                            Name = "Soccer Ball",
                            Description = "FIFA-approved size 5",
                            Quantity = 30,
                            Price = 25,
                            CategoryId = sports.CategoryId,
                            SupplierId = adidas.SupplierId
                        }
                    );
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
