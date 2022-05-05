using Microsoft.Extensions.Configuration;
using Npgsql;

namespace postgresql_azure_with_csharp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
             IConfiguration config= new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", true, true)
                                    .Build();

            string connectionString = config["connectionstring"];
            Console.WriteLine("Hello World!" + connectionString);

            using (var conn = new NpgsqlConnection(connectionString))
            {
                Console.Out.WriteLine("Conectando a postgres en azure...");
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO empleados (id, nombre, salario, fecha_nacimiento)
                        VALUES (@id1, @nombre1, @salario1, @fecha_nacimiento1);";
                    
                    command.Parameters.AddWithValue("@id1", 32);
                    command.Parameters.AddWithValue("@nombre1", "José Javier");
                    command.Parameters.AddWithValue("@salario1", 1530.23);
                    command.Parameters.AddWithValue("@fecha_nacimiento", new DateTime(1987,01,01));

                    int rowCount = command.ExecuteNonQuery();
                    Console.WriteLine("Se insertaron correctamente " + rowCount.ToString() + " filas");
                }
            }

            Console.WriteLine("Press return to EXIT");
            Console.ReadLine();
        }
    }
}