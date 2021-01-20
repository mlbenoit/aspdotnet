using System;
using System.ComponentModel.DataAnnotations;

namespace BookRental.WebContracts
{
    public class AdminLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public AdminLoginRequest()
        {
        }

        public AdminLoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
