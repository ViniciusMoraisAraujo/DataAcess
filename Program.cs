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
                //GetCategories(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);
                //OneToOne(connection);
                OneToMany(connection);
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

        static void GetCategories(SqlConnection connection)
        {
            var category = connection.QueryFirstOrDefault<Category>(
                "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id] = @Id", 
                new {Id = "af3407aa-11ae-4621-a2ef-2028b85507c4"});
            Console.WriteLine($"{category.Id} - {category.Title}");
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

        static void ExecuteProcedure(SqlConnection connection) //leitura
        {
            var procedure = "[spDeleteStudent]";
            var pars = new {StudentId = "8df494de-9488-440a-b789-077df6e45ffe"};
            var affectedRows = connection.Execute(procedure, pars, commandType: System.Data.CommandType.StoredProcedure);
            Console.WriteLine($"Affected Rows: {affectedRows}");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
            var courses = connection.Query<Category>(procedure, pars, commandType: System.Data.CommandType.StoredProcedure);
            foreach (var category in courses)
            {
                Console.WriteLine($"{category.Id} - {category.Title}");
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "Amazon";
            category.Description = "Category for services AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSql = @"INSERT INTO [Category] OUTPUT inserted.[Id] VALUES(NEWID(), @Title, @Url, @Summary, @Order, @Description, @Featured)";
            var guid = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"Category Id: {guid}");
        }

        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void OneToOne(SqlConnection connection)
        {
            var sql = @"SELECT * FROM [CareerItem] INNER JOIN [Course] ON [CareerItem].[CourseId] = [Course].[Id]";
            var courses = connection.Query<CareerItem, Course, CareerItem>(sql, 
                (careerItem,course) => {careerItem.Course = course;return careerItem; }, splitOn:"Id");
            
            foreach (var item in courses)
            {
                Console.WriteLine($"Career: {item.Title} -  Course: {item.Course.Title}");
            }
        }

        static void OneToMany(SqlConnection connection)
        {
            var sql = @"SELECT [Career].[Id], [Career].[Title], [CareerItem].[CareerId], [CareerItem].[Title]
                        FROM [Career] INNER JOIN [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                        ORDER BY [Career].[Title]";

            var careers = new List<Career>();
            var  items = connection.Query<Career, CareerItem, Career>(sql,
                (career, careerItem) =>
                {
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if (car == null)
                    {
                         car = career;
                         car.Items.Add(careerItem);
                         careers.Add(car);
                    }
                    else
                    {
                        car.Items.Add(careerItem);
                    }
                    return career;
                }, splitOn:"CareerId");

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
        }
    }
}

