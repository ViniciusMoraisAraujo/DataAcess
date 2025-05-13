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
                UpdateCategory(connection);
                ListCategories(connection);
                // CreateCategory(connection);
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
            var insertSql = @"INSERT INTO 
                                [Category] 
                            VALUES(
                                @Id, 
                                @Title, 
                                @Url, 
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured
                                )";
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
            var updateQuery = "UPDATE [Category] SET Title = @Title WHERE Id = @Id";
            var rows = connection.Execute(updateQuery, new { id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4 "), title = "Front-end 2021" });
            Console.WriteLine($"Rows att {rows} affected");
        }
    }
}

