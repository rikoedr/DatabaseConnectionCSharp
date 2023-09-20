using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Department
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int LocationID { get; set; }
    public int ManagerID { get; set; }




    public Department()
    {
        this.ID = 0;
        this.Name = "Empty";
        this.LocationID = 0;
        this.ManagerID = 0;
    }

    public List<Department> GetAll()
    {
        List<Department> departments = new List<Department>();

        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM departments";

        try
        {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        LocationID = reader.GetInt32(2),
                        ManagerID = reader.GetInt32(3),
                    });
                }

                reader.Close();
                dbConnection.Close();

                return departments;
            }

            reader.Close();
            dbConnection.Close();

            return departments;

        }
        catch (Exception error)
        {
            Console.WriteLine("Error : " + error.Message);
        }

        return departments;
    }

    public Department GetById(int id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM departments WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try
        {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return new Department
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        LocationID = reader.GetInt32(2),
                        ManagerID = reader.GetInt32(3),
                    };
                }
            }
            reader.Close();
            dbConnection.Close();

            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new Department();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new Department();
        }

    }

    public string Insert(string name, int locationID, int managerID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO departments VALUES(@id, @name, @location_id, @manager_id)";
        dbCommand.Parameters.Add(new SqlParameter("@id", getLastID() + 1));
        dbCommand.Parameters.Add(new SqlParameter("@name", name));
        dbCommand.Parameters.Add(new SqlParameter("@location_id", locationID));
        dbCommand.Parameters.Add(new SqlParameter("@manager_id", managerID));

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
        dbCommand.CommandText = "UPDATE departments SET street_address = @street_address, postal_code = @postal_code, city = @city, state_province = @state_province, country_id = @country_id WHERE id = @id";
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
        dbCommand.CommandText = "DELETE FROM departments WHERE id = @id";
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
        List<Department> departments = GetAll();
        int lastIndex = departments.Count - 1;

        return departments[lastIndex].ID;
    }

}
