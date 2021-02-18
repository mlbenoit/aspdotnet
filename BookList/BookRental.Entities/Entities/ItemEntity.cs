using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookRental.Entities
{
    public class ItemEntity
    {
        [Key]
        // TODO:Add auto Generated ID
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Price { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }


        public DateTime UpdatedOn { get; set; }

    }
}
