using System;
namespace BookRental.WebContracts
{
    public interface IServiceError
    {
        string Code { get; }
        string Description { get; }
    }
}