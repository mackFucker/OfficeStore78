using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.db
{
    public class User(int id, string email, string firstName, string lastName, string password)
    {
        public int Id { get; set; } = id;
        public string Email { get; set; } = email;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string HashedPassword { get; set; } = BCrypt.Net.BCrypt.HashPassword(password);
    }
}
