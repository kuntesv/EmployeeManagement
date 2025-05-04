using EmployeeManagement.Models;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace EmployeeManagement.Services
{
    public class EmployeeService
    {

        private string connectionString = "Data Source=DESKTOP-IBQA4NC;Initial Catalog=Test;Integrated Security=True;MultipleActiveResultSets=True";

        public EmployeeService() 
        {
        }

        public async Task<bool> AddEmployee(EmployeeDetails employeeDetails)
        {

                string sql = @"
            INSERT INTO Employees (FirstName, LastName, Email, Salary, IsActive)
            VALUES (@FirstName, @LastName, @Email, @Salary, @IsActive);";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", employeeDetails.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", employeeDetails.LastName);
                        cmd.Parameters.AddWithValue("@Email", (object)employeeDetails.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Salary", employeeDetails.Salary);
                        cmd.Parameters.AddWithValue("@IsActive", employeeDetails.IsActive);


                        int rowAffected = await cmd.ExecuteNonQueryAsync();
                        await sqlConnection.CloseAsync();

                    return rowAffected > 0;
                    }

                }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            string sql = @"DELETE FROM Employees WHERE EmployeeID = @Id;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                {

                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowAffected = await cmd.ExecuteNonQueryAsync();
                    return rowAffected > 0;
                }
            }


        }



        public async Task<EmployeeDetails> GetEmployeeDetailsById(int id)
        {
            string sql = @"SELECT *  FROM Employees WHERE EmployeeID = @Id;";


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                await sqlConnection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                {

                    cmd.Parameters.AddWithValue("@Id", id);


                    using (SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            await sqlDataReader.ReadAsync();

                            EmployeeDetails emp = new EmployeeDetails();
                            emp.FirstName = sqlDataReader.GetString(1);
                            emp.LastName = sqlDataReader.GetString(2);
                            emp.Email = sqlDataReader.GetString(3);
                            emp.Salary = sqlDataReader.GetDecimal(4);
                            emp.IsActive = sqlDataReader.GetBoolean(5);

                            return emp;

                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

        }


        public async Task<List<EmployeeDetails>> GetAllEmployeeDetails()
        {
            List<EmployeeDetails> emps = new List<EmployeeDetails>();

            string sql = @"SELECT *  FROM Employees;";


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                await sqlConnection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                {

                    using (SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            EmployeeDetails emp = new EmployeeDetails();
                            emp.FirstName = sqlDataReader.GetString(1);
                            emp.LastName = sqlDataReader.GetString(2);
                            emp.Email = sqlDataReader.GetString(3);
                            emp.Salary = sqlDataReader.GetDecimal(4);
                            emp.IsActive = sqlDataReader.GetBoolean(5);

                            emps.Add(emp);

                        }

                        return emps;
                    }
                }
            }
        }


        public async Task<bool> UpdateEmployeeDetails(int id, EmployeeDetails employeeDetails)
        {
            string sqlStmt = "UPDATE Employees SET FirstName = @firstName , LastName = @lastName , Email = @email, Salary = @salary, IsActive = @isActive WHERE EmployeeID = @empId;";

            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using(SqlCommand cmd = new SqlCommand(sqlStmt, sqlConnection))
                {

                    cmd.Parameters.AddWithValue("@firstName", employeeDetails.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", employeeDetails.LastName);
                    cmd.Parameters.AddWithValue("@email", employeeDetails.Email);
                    cmd.Parameters.AddWithValue("@salary", employeeDetails.Salary);
                    cmd.Parameters.AddWithValue("@isActive", employeeDetails.IsActive);
                    cmd.Parameters.AddWithValue("@empId", id);


                   int rowAffected =  await cmd.ExecuteNonQueryAsync();

                    return rowAffected > 0;

                }
            }
        }










    }
}
