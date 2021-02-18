using System;
namespace BookRental.WebContracts.WebContracts
{
    public class UpdateItemRequest
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public string Price { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
