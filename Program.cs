using Dapper;
using DataAcess.Models;
using Microsoft.Data.SqlClient;

namespace DataAcess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$; TrustServerCertificate=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connecting to database...");
                //UpdateCategory(connection);
                //ListCategories(connection);
                //CreateCategory(connection);
                //DeleteCategory(connection);
                //CreateManyCategory(connection);
                //ListCategories(connection);
            }
            
        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "Amazon";
            category.Description = "Category for services AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSql = @"INSERT INTO[Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";
            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"Rows affected: {rows}");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] = @Title WHERE [Id] = @id";
            var rows = connection.Execute(updateQuery, new { id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"), title = "Front-end 2021" });
            Console.WriteLine($"Rows att {rows} affected");
        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE FROM [Category] WHERE [Id] = @id";
            var rows = connection.Execute(deleteQuery, new { id = new Guid("a86d3f41-7e8a-4d9d-94ea-1b2a48d9f9ba"), });
            Console.WriteLine($"Rows deleted {rows} excluido affected");
        }
        
        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "Amazon";
            category.Description = "Category for services AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;  
            
            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Category new";
            category2.Url = "Category new";
            category2.Description = "Category new";
            category2.Order = 9;
            category2.Summary = "Category-new";
            category2.Featured = true;
            
            
            var insertSql = @"INSERT INTO[Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";
            var rows = connection.Execute(insertSql, new []
            { 
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            Console.WriteLine($"Rows affected: {rows}");
        }
    }
}

