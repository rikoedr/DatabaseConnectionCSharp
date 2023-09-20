using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Country
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }

    public Country()
    {
        this.Id = "Empty";
        this.Name = "Empty";
        this.RegionId = 0;
    }

    public List<Country> GetAll()
    {
        List<Country> countries = new List<Country>();

        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM countries";

        try
        {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    countries.Add(new Country
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        RegionId = reader.GetInt32(2)
                    });
                }

                reader.Close();
                dbConnection.Close();

                return countries;
            }

            reader.Close();
            dbConnection.Close();

            return countries;

        }
        catch (Exception error)
        {
            Console.WriteLine("Error : " + error.Message);
        }

        return countries;
    }

    public Country GetById(string id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM countries WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try
        {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return new Country
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        RegionId = reader.GetInt32(2)
                    };
                }
            }
            reader.Close();
            dbConnection.Close();

            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new Country();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new Country();
        }

    }

    public string Insert(string id, string name, int regionId)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO countries VALUES(@id, @name, @region_id)";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@name", name));
        dbCommand.Parameters.Add(new SqlParameter("@region_id", regionId));


        try
        {
            dbConnection.Open();

            using SqlTransaction dbTransaction = dbConnection.BeginTransaction();

            try
            {
                dbCommand.Transaction = dbTransaction;
                int result = dbCommand.ExecuteNonQuery();
                dbTransaction.Commit();
                dbConnection.Close();

                return result.ToString();
            }
            catch (Exception error)
            {
                dbTransaction.Rollback();
                return $"Transaction Error : {error.Message}";
            }
        }
        catch (Exception error)
        {
            return $"Insertion Error : {error.Message}";
        }

    }

    public string Update(string id, string name, int regionId)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "UPDATE countries SET name = @name, region_id = @region_id WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@name", name));
        dbCommand.Parameters.Add(new SqlParameter("@region_id", regionId));


        try
        {
            dbConnection.Open();
            using SqlTransaction dbTransaction = dbConnection.BeginTransaction();

            try
            {
                dbCommand.Transaction = dbTransaction;
                int result = dbCommand.ExecuteNonQuery();

                dbTransaction.Commit();
                dbConnection.Close();

                return result.ToString();
            }
            catch (Exception error)
            {
                dbTransaction.Rollback();
                return $"Transaction Error : {error.Message}";
            }
        }
        catch (Exception error)
        {
            return $"Update Proccess : {error.Message}";
        }
    }

    public string Delete(string id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "DELETE FROM countries WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try
        {
            dbConnection.Open();
            using SqlTransaction dbTransaction = dbConnection.BeginTransaction();

            try
            {
                dbCommand.Transaction = dbTransaction;
                int result = dbCommand.ExecuteNonQuery();

                dbTransaction.Commit();
                dbConnection.Close();

                return result.ToString();
            }
            catch (Exception error)
            {
                dbTransaction.Rollback();
                return $"Transaction Error : {error.Message}";
            }
        }
        catch (Exception error)
        {
            return $"Update Proccess : {error.Message}";
        }
    }

}
