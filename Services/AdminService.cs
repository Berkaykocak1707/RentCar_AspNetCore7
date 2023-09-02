using Microsoft.AspNetCore.Identity;
using RentCar_AspNetCore7.Models;
using System;

namespace RentCar_AspNetCore7.Services
{
    public class AdminService
    {
        private readonly PasswordHasher<Admin> _passwordHasher;

        public AdminService()
        {
            _passwordHasher = new PasswordHasher<Admin>();
        }

        public Admin CreateAdmin(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username, email and password are required.");
            }

            Admin admin = new Admin
            {
                Username = username,
                Email = email,
                Password = _passwordHasher.HashPassword(null, password)  // Hash the password
            };

            // Save the admin object to the database
            // NOTE: You'll need to implement this part according to your data storage strategy

            return admin;
        }

        public bool VerifyPassword(Admin admin, string providedPassword)
        {
            if (admin == null || string.IsNullOrWhiteSpace(providedPassword))
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(null, admin.Password, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
