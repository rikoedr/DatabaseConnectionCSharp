using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Location
{
    public int ID { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string StateProvince { get; set; }
    public string CountryID { get; set; }



    public Location()
    {
        this.ID = 0;
        this.StreetAddress = "Empty";
        this.PostalCode = "Empty";
        this.City = "Empty";
        this.StateProvince = "Empty";
        this.CountryID = "Empty";
    }

    public List<Location> GetAll()
    {
        List<Location> locations = new List<Location>();

        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM locations";

        try
        {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    locations.Add(new Location
                    {
                        ID = reader.GetInt32(0),
                        StreetAddress = reader.GetString(1),
                        PostalCode = reader.GetString(2),
                        City = reader.GetString(3),
                        StateProvince = reader.GetString(4),
                        CountryID = reader.GetString(5)
                    });
                }

                reader.Close();
                dbConnection.Close();

                return locations;
            }

            reader.Close();
            dbConnection.Close();

            return locations;

        }
        catch (Exception error)
        {
            Console.WriteLine("Error : " + error.Message);
        }

        return locations;
    }

    public Location GetById(int id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM locations WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try
        {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return new Location
                    {
                        ID = reader.GetInt32(0),
                        StreetAddress = reader.GetString(1),
                        PostalCode = reader.GetString(2),
                        City = reader.GetString(3),
                        StateProvince = reader.GetString(4),
                        CountryID = reader.GetString(5)
                    };
                }
            }
            reader.Close();
            dbConnection.Close();

            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new Location();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new Location();
        }

    }

    public string Insert(string streetAddress, string postalCode, string city, string stateProvince, string countryID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO locations VALUES(@id, @street_address, @postal_code, @city, @state_province, @country_id)";
        dbCommand.Parameters.Add(new SqlParameter("@id", getLastID() + 1));
        dbCommand.Parameters.Add(new SqlParameter("@street_address", streetAddress));
        dbCommand.Parameters.Add(new SqlParameter("@postal_code", postalCode));
        dbCommand.Parameters.Add(new SqlParameter("@city", city));
        dbCommand.Parameters.Add(new SqlParameter("@state_province", stateProvince));
        dbCommand.Parameters.Add(new SqlParameter("@country_id", countryID));

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

    public string Update(int id, string streetAddress, string postalCode, string city, string stateProvince, string countryID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "UPDATE locations SET street_address = @street_address, postal_code = @postal_code, city = @city, state_province = @state_province, country_id = @country_id WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@street_address", streetAddress));
        dbCommand.Parameters.Add(new SqlParameter("@postal_code", postalCode));
        dbCommand.Parameters.Add(new SqlParameter("@city", city));
        dbCommand.Parameters.Add(new SqlParameter("@state_province", stateProvince));
        dbCommand.Parameters.Add(new SqlParameter("@country_id", countryID));


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

    public string Delete(int id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "DELETE FROM locations WHERE id = @id";
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

    public int getLastID()
    {
        List<Location> locations = GetAll();
        int lastIndex = locations.Count - 1;

        return locations[lastIndex].ID;
    }

}
