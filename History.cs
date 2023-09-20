using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class History
{
    public DateTime StartDate { get; set; }
    public int EmployeeID { get; set; }
    public DateTime EndDate { get; set; }
    public int DepartmentID { get; set; }
    public string JobID { get; set; }


    public History()
    {
        this.EmployeeID = 0;
        this.DepartmentID = 0;
        this.JobID = "Empty";
    }

    public List<History> GetAll()
    {
        List<History> histories = new List<History>();

        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM histories";

        try
        {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    histories.Add(new History
                    {
                        StartDate = reader.GetDateTime(0),
                        EmployeeID = reader.GetInt32(1),
                        EndDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        DepartmentID = reader.GetInt32(3),
                        JobID = reader.GetString(4)
                    }); ;
                }

                reader.Close();
                dbConnection.Close();
               
                return histories;
            }

            reader.Close();
            dbConnection.Close();

            return histories;

        }
        catch (Exception error)
        {
            Console.WriteLine("Error : " + error.Message);
        }

        return histories;
    }

    public History GetById(int employeeID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM histories WHERE employee_id = @employee_id";
        dbCommand.Parameters.Add(new SqlParameter("@employee_id", employeeID));

        try
        {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return new History
                    {
                        StartDate = reader.GetDateTime(0),
                        EmployeeID = reader.GetInt32(1),
                        EndDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        DepartmentID = reader.GetInt32(3),
                        JobID = reader.GetString(4)
                    };
                }
            }
            reader.Close();
            dbConnection.Close();

            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new History();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new History();
        }

    }

    public string Insert(int employeeID, string endDate, int departmentID, string jobID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO histories (employee_id, end_date, department_id, job_id) VALUES(@employee_id, @end_date, @department_id, @job_id)";
        dbCommand.Parameters.Add(new SqlParameter("@employee_id", employeeID));
        dbCommand.Parameters.Add(new SqlParameter("@end_date", endDate));
        dbCommand.Parameters.Add(new SqlParameter("@department_id", departmentID));
        dbCommand.Parameters.Add(new SqlParameter("@job_id", jobID));

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

    public string Update(int employeeID, string endDate, int departmentID, string jobID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "UPDATE histories SET end_date = @end_date, department_id = @department_id, job_id = @job_id WHERE employee_id = @employee_id";
        dbCommand.Parameters.Add(new SqlParameter("@employee_id", employeeID));
        dbCommand.Parameters.Add(new SqlParameter("@end_date", endDate));
        dbCommand.Parameters.Add(new SqlParameter("@department_id", departmentID));
        dbCommand.Parameters.Add(new SqlParameter("@job_id", jobID));


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

    public string Delete(int employeeID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "DELETE FROM histories WHERE employee_id = @employee_id";
        dbCommand.Parameters.Add(new SqlParameter("@employee_id", employeeID));

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
