using System;
namespace BookRental.WebContracts
{
    public class CreateAdminRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode{ get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
