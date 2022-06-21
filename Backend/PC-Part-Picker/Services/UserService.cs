using Microsoft.AspNetCore.Cors;
using Microsoft.Data.SqlClient;
using PC_Part_Picker.Models;
using System.Data;

namespace PC_Part_Picker.Services
{
    [EnableCors("cors_allow")]
    public class UserService
    {
        SqlConnection _connection;

        public UserService(SqlConnection connection)
        {
            _connection = connection;
        }

        [EnableCors("cors_allow")]
        public async Task<string> LogIn(string email, string password)
        {
            string query = "SELECT * FROM [AUTH].[USERS] WHERE email=\'" + email + "\'";
            SqlCommand command = new SqlCommand(query, _connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    string sys_password = reader.GetString("password");
                    if (sys_password.Equals(password))
                    {
                        return reader.GetGuid("User_ID") + "";
                    }
                    else
                    {
                        return "Password not matched";
                    }
                }
            }
            return "There is no such an email.";
        }

        [EnableCors("cors_allow")]
        public async Task<bool> SignIn(string email, string password)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO USERS (User_ID, email, password) VALUES (@guid, @email, @password)";
            command.Parameters.AddWithValue("@guid", Guid.NewGuid());
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            try
            {
                int recordsAffected = await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
        }


    }
}