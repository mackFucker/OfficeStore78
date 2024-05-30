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
    internal class UserService
    {
        private readonly MySqlConnection connection;

        public UserService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            connection = new MySqlConnection(connectionString);
        }

        internal void RegisterUser(string email, string firstName, string lastName, string password)
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
                    return;
                }

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // Insert the user into the database
                command.CommandText = "INSERT INTO users (email, first_name, last_name, password) VALUES (@email, @first_name, @last_name, @password);";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@first_name", firstName);
                command.Parameters.AddWithValue("@last_name", lastName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.ExecuteNonQuery();

                MessageBox.Show("User registered successfully!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error registering user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void DeleteUser(string email)
        {
            try
            {
                connection.Open();

                // Check if the user exists
                MySqlCommand command = new ("SELECT COUNT(*) FROM users WHERE email = @email;", connection);
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

        internal void DeleteUser(int id)
        {
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

        internal void LoginUser(string email, string password)
        {
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

                // Get the user's hashed password from the database
                command.CommandText = "SELECT password FROM users WHERE email = @email;";
                string hashedPassword = command.ExecuteScalar().ToString();

                // Verify the password
                if (!BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                {
                    MessageBox.Show("Invalid password.");
                    return;
                }

                // Password is valid, open the main application window
                MessageBox.Show("Login successful!");
                // You can add code here to open the main application window and close the login window
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error logging in: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }


}
