using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Program
{
    static string connectionString = "Data Source=ENRICHO;Initial Catalog=db_hr_dts;Integrated Security=True;Connect Timeout=30;Database=db_hr_dts";
    private static void Main()
    {
        //GetAllRegions();
        GetRegionById(1);
        UpdateRegion(1, "Afrika Utara");
        GetRegionById(1);

        UpdateRegion(100, "Afrika Utara");
        GetAllRegions();
        DeleteRegion(11);
        GetAllRegions();

    }

    // GET ALL: Region
    public static void GetAllRegions()
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions";

        try
        {
            connection.Open();

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // GET BY ID: Region
    public static void GetRegionById(int id)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions WHERE id = @id";

        var pID = new SqlParameter();
        pID.ParameterName = "@id";
        pID.Value = id;
        pID.SqlDbType = SqlDbType.Int;
        command.Parameters.Add(pID);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            }
            else
            {
                Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            }

            reader.Close();
            connection.Close();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Error : {error.Message}");
        }
    }

    // INSERT: Region
    public static void InsertRegion(string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "INSERT INTO regions VALUES (@name);";

        try
        {
            var pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(pName);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Insert Success");
                        break;
                    default:
                        Console.WriteLine("Insert Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error Transaction: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // UPDATE: Region
    public static void UpdateRegion(int id, string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.CommandText = "UPDATE regions SET name = @name WHERE id = @id";
        command.Connection = connection;

        try
        {
            SqlParameter pID = new SqlParameter("@id", SqlDbType.Int);
            pID.Value = id;
            command.Parameters.Add(pID);
            SqlParameter pName = new SqlParameter("@name", SqlDbType.VarChar);
            pName.Value = name;
            command.Parameters.Add(pName);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Update Success");
                        break;
                    default:
                        Console.WriteLine("Update Failed");
                        break;
                }

            }
            catch (Exception error)
            {
                transaction.Rollback();
                Console.WriteLine($"Error : {error.Message}");
            }
        }
        catch (Exception error)
        {
            Console.WriteLine($"Error : {error.Message}");
        }
    }

    // DELETE: Region
    public static void DeleteRegion(int id)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.CommandText = "DELETE FROM regions WHERE id = @id";
        command.Connection = connection;
        try
        {
            SqlParameter pID = new SqlParameter("@id", SqlDbType.Int);
            pID.Value = id;
            command.Parameters.Add(pID);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Delete Success");
                        break;
                    default:
                        Console.WriteLine("Delete Failed");
                        break;
                }

            }
            catch (Exception error)
            {
                transaction.Rollback();
                Console.WriteLine($"Error : {error.Message}");
            }
        }
        catch (Exception error)
        {
            Console.WriteLine($"Error : {error.Message}");
        }
    }
}