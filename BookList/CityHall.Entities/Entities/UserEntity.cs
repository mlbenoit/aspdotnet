using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookList.Entities
{
    public class UserEntity:IdentityUser
    {
        public override string Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override string NormalizedUserName { get; set; }
        public override string NormalizedEmail { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NIF { get; set; }
        public string Gender { get; set; }

        // Note: overriding properties in IdentityUser base class, so that EF generates the columns in right order
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool TwoFactorEnabled { get; set; }
        public override bool PhoneNumberConfirmed { get; set; }
        public override string PhoneNumber { get; set; }
        public override string ConcurrencyStamp { get; set; }
        public override string SecurityStamp { get; set; }
        public override string PasswordHash { get; set; }
        public override bool EmailConfirmed { get; set; }
        public override bool LockoutEnabled { get; set; }
        public override int AccessFailedCount { get; set; }


    }
}
