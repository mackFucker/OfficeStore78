using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace WpfApp1.Service
{
    public class UserService
    {

        private readonly MySqlConnection connection;
        public static string LoggedInUserRole { get; private set; }


        public UserService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            connection = new MySqlConnection(connectionString);

            EnsureUsersTableExists();
        }

        internal void EnsureUsersTableExists()
        {
            try
            {
                connection.Open();
                MySqlCommand command = new("CREATE TABLE IF NOT EXISTS users (" +
                                           "id INT AUTO_INCREMENT PRIMARY KEY, " +
                                           "email VARCHAR(255) NOT NULL, " +
                                           "first_name VARCHAR(255), " +
                                           "last_name VARCHAR(255), " +
                                           "password VARCHAR(255) NOT NULL, " +
                                           "role VARCHAR(50) NOT NULL" +
                                           ");", connection);
                command.ExecuteNonQuery();

                // Check if the default admin user exists
                command.CommandText = "SELECT COUNT(*) FROM users WHERE email = 'admin';";
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    // Insert the default admin user
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");
                    command.CommandText = "INSERT INTO users (email, first_name, last_name, password, role) VALUES ('admin', 'Admin', 'User', @password, 'Admin');";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error ensuring users table exists: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }



        internal bool RegisterUser(string email, string firstName, string lastName, string password, string role)
        {
            try
            {
                connection.Open();

                // Check if the user already exists
                MySqlCommand command = new("SELECT COUNT(*) FROM users WHERE email = @email;", connection);
                command.Parameters.AddWithValue("@email", email);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("User with this email already exists.");
                    return false;
                }

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // Insert the user into the database                
                command.CommandText = "INSERT INTO users (email, first_name, last_name, password, role) VALUES (@email, @first_name, @last_name, @password, @role);";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@role", role);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@first_name", firstName);
                command.Parameters.AddWithValue("@last_name", lastName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.ExecuteNonQuery();

                MessageBox.Show("User registered successfully!");
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error registering user: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }


        private bool IsAdmin(string email)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new("SELECT role FROM users WHERE email = @userEmail;", connection);
                command.Parameters.AddWithValue("@userEmail", email);
                string role = (string)command.ExecuteScalar();
                return role == "Admin";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error checking user role: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        internal void DeleteUser(string adminEmail, string email)
        {
            if (!IsAdmin(adminEmail))
            {
                MessageBox.Show("Only admin can delete users.");
                return;
            }

            try
            {
                connection.Open();

                // Check if the user exists
                MySqlCommand command = new("SELECT COUNT(*) FROM users WHERE email = @email;", connection);
                command.Parameters.AddWithValue("@email", email);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("User with this email does not exist.");
                    return;
                }

                command.CommandText = "DELETE FROM users WHERE email = @email;";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();

                MessageBox.Show("User deleted successfully!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void DeleteUser(string adminEmail, int id)
        {
            if (!IsAdmin(adminEmail))
            {
                MessageBox.Show("Only admin can delete users.");
                return;
            }

            try
            {
                connection.Open();

                // Check if the user exists
                MySqlCommand command = new("SELECT COUNT(*) FROM users WHERE id = @id;", connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("User with this ID does not exist.");
                    return;
                }

                // Delete the user from the users table
                command.CommandText = "DELETE FROM users WHERE id = @id;";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                MessageBox.Show("User deleted successfully!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void DeleteAllUsers() // for test purposes?
        {
            try
            {
                connection.Open();

                MySqlCommand command = new("TRUNCATE TABLE users;", connection);
                command.ExecuteNonQuery();

                MessageBox.Show("All users deleted successfully!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error deleting all users: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        internal bool LoginUser(string email, string password)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new("SELECT password, role FROM users WHERE email = @userEmail;", connection);
                command.Parameters.AddWithValue("@userEmail", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashedPassword = reader.GetString(0);
                        string role = reader.GetString(1);

                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                        {
                            LoggedInUserRole = role;
                            MessageBox.Show("Login successful!");
                            return true;
                        }
                    }
                }

                MessageBox.Show("Invalid email or password.");
                return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error logging in: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
