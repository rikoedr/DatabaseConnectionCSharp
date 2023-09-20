using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Employee
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public int Salary { get; set; }
    public double ComissionPCT { get; set; }
    public int ManagerID { get; set; }
    public string JobID { get; set; }
    public int DepartmentID { get; set; }
 

    public List<Employee> GetAll()
    {
        List<Employee> employees = new List<Employee>();

        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM employees";

        try
        {
            dbConnection.Open();

            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        ID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        HireDate = reader.GetDateTime(5),
                        Salary = reader.GetInt32(6),
                        ComissionPCT = reader.IsDBNull(7) ? 0.00 : Convert.ToDouble(reader.GetDecimal(7)),
                        ManagerID = reader.GetInt32(8),
                        JobID = reader.GetString(9),
                        DepartmentID = reader.GetInt32(10)
                    }); ;
                }

                reader.Close();
                dbConnection.Close();

                return employees;
            }

            reader.Close();
            dbConnection.Close();

            return employees;

        }
        catch (Exception error)
        {
            Console.WriteLine("Error : " + error.Message);
        }

        return employees;
    }

    public Employee GetById(int id)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "SELECT * FROM employees WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));

        try
        {
            dbConnection.Open();
            using SqlDataReader reader = dbCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return new Employee
                    {
                        ID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        HireDate = reader.GetDateTime(5),
                        Salary = reader.GetInt32(6),
                        ComissionPCT = reader.IsDBNull(7) ? 0.00 : Convert.ToDouble(reader.GetDecimal(7)),
                        ManagerID = reader.GetInt32(8),
                        JobID = reader.GetString(9),
                        DepartmentID = reader.GetInt32(10)
                    };
                }
            }
            reader.Close();
            dbConnection.Close();

            Console.WriteLine("Data dengan ID yang di masukkan tidak tersedia");

            return new Employee();
        }
        catch (Exception error)
        {
            Console.WriteLine($"Insertion Error : {error.Message}");
            return new Employee();
        }

    }

    public string Insert(int id, string firstName, string lastName, string email, string phoneNumber, DateTime hireDate, int salary, Decimal comissionPCT, int managerID, string jobID, int departmentID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "INSERT INTO employees VALUES(@id, @first_name, @last_name, @email, @phone_number, @hire_date, @salary, @comission_pct, @manager_id, @job_id, @department_id)";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@first_name", firstName));
        dbCommand.Parameters.Add(new SqlParameter("@last_name", lastName));
        dbCommand.Parameters.Add(new SqlParameter("@email", email));
        dbCommand.Parameters.Add(new SqlParameter("@phone_number", phoneNumber));
        dbCommand.Parameters.Add(new SqlParameter("@hire_date", hireDate));
        dbCommand.Parameters.Add(new SqlParameter("@salary", salary));
        dbCommand.Parameters.Add(new SqlParameter("@comission_pct", comissionPCT));
        dbCommand.Parameters.Add(new SqlParameter("@manager_id", managerID));
        dbCommand.Parameters.Add(new SqlParameter("@job_id", jobID));
        dbCommand.Parameters.Add(new SqlParameter("@department_id", departmentID));

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

    public string Update(int id, string firstName, string lastName, string email, string phoneNumber, DateTime hireDate, int salary, Decimal comissionPCT, int managerID, string jobID, int departmentID)
    {
        using SqlConnection dbConnection = new SqlConnection(DBString.Connection);
        using SqlCommand dbCommand = new SqlCommand();

        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = "UPDATE employees SET first_name = @first_name, last_name = @last_name, email = @email, phone_number = @phone_number, hire_date = @hire_date, salary = @salary, comission_pct = @comission_pct, manager_id = @manager_id, job_id = @job_id, department_id = @department_id WHERE id = @id";
        dbCommand.Parameters.Add(new SqlParameter("@id", id));
        dbCommand.Parameters.Add(new SqlParameter("@first_name", firstName));
        dbCommand.Parameters.Add(new SqlParameter("@last_name", lastName));
        dbCommand.Parameters.Add(new SqlParameter("@email", email));
        dbCommand.Parameters.Add(new SqlParameter("@phone_number", phoneNumber));
        dbCommand.Parameters.Add(new SqlParameter("@hire_date", hireDate));
        dbCommand.Parameters.Add(new SqlParameter("@salary", salary));
        dbCommand.Parameters.Add(new SqlParameter("@comission_pct", comissionPCT));
        dbCommand.Parameters.Add(new SqlParameter("@manager_id", managerID));
        dbCommand.Parameters.Add(new SqlParameter("@job_id", jobID));
        dbCommand.Parameters.Add(new SqlParameter("@department_id", departmentID));



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
        dbCommand.CommandText = "DELETE FROM employees WHERE id = id";
        dbCommand.Parameters.Add(new SqlParameter("id", id));

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
