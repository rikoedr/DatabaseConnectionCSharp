using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DatabaseConnection;

public class Program
{
    private static void Main(string[] args) {
        Employee employee = new Employee();

        Console.WriteLine(employee.GetById(1).FirstName);
    }

    

    
}