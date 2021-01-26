using System;
using System.ComponentModel.DataAnnotations;

namespace BookRental.WebContracts
{

    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserLoginRequest()
        {

        }

        public UserLoginRequest(string email, string password)
        {
            Email= email;
            Password = password;
        }
    }
}
