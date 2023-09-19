using System.Data.SqlClient;

namespace DatabaseConnection;

public class Program
{
    static string connectionString = "Data Source=ENRICHO;Initial Catalog=db_hr_dts;Integrated Security=True;Connect Timeout=30;Database=db_hr_dts";
    static SqlConnection conn;
    public static void Main(string[] args)
    {
        conn = new SqlConnection(connectionString);

        try
        {
            conn.Open();
            Console.WriteLine("Database connected");

            SqlCommand command = new SqlCommand("SELECT * FROM employees", conn);

            Console.WriteLine(command.ExecuteReader());

            conn.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
        }
    }
}