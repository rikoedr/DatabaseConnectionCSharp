using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Region {
    public int Id { get; set; }
    public string Name { get; set; }

    public Region() {
        this.Id = 0;
        this.Name = "Empty";
    }

    public List<Region> GetAll() {
        List<Region> regionList = new List<Region>();
        
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM regions";

        try {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if(reader.HasRows) {
                while(reader.Read()) {
                    regionList.Add(new Region {
                       Id = reader.GetInt32(0),
                       Name = reader.GetString(1)
                    });
                }

                reader.Close();
                dbConnection.Close();

                return regionList;
            }

            reader.Close();
            dbConnection.Close();

            return regionList;

        } catch(Exception error) {
            Console.WriteLine("Error : " + error.Message);
        }

        return regionList;
    }

    public Region GetById(int id) {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM regions WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    return new Region {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                }
            }
            reader.Close();
            dbConnection.Close();
            
            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new Region();
        }
        catch (Exception error) {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new Region();
        }

    }

    public string Insert(string name) {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO regions VALUES(@name)";
        dbCommand.Parameters.Add(new SqlParameter("@name", name));

        try {
            dbConnection.Open();

            using SqlTransaction dbTransaction = dbConnection.BeginTransaction();

            try {
                dbCommand.Transaction = dbTransaction;
                int result = dbCommand.ExecuteNonQuery();
                dbTransaction.Commit();
                dbConnection.Close();

                return result.ToString();
            }
            catch(Exception error) {
                dbTransaction.Rollback();
                return $"Transaction Error : {error.Message}";
            }
        }
        catch(Exception error) {
            return $"Insertion Error : {error.Message}";
        }

    }

    public string Update(int id, string name) {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "UPDATE regions SET name = @name WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@name", name));

        try {
            dbConnection.Open();
            using SqlTransaction dbTransaction = dbConnection.BeginTransaction();
            
            try {
                dbCommand.Transaction = dbTransaction;
                int result = dbCommand.ExecuteNonQuery();

                dbTransaction.Commit();
                dbConnection.Close();

                return result.ToString();
            }
            catch(Exception error) {
                dbTransaction.Rollback();
                return $"Transaction Error : {error.Message}";
            }
        }
        catch (Exception error){
            return $"Update Proccess : {error.Message}";
        }  
    }

    public string Delete(int id) {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "DELETE FROM regions WHERE id = @id";
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
