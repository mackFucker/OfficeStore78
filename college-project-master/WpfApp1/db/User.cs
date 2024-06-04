using System;

namespace WpfApp1.db
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }

        public User(int id, string email, string firstName, string lastName, string password, string role)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Role = role;
        }
    }
}
