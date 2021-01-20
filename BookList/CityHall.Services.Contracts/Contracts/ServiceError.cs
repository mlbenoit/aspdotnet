using System;
using BookRental.WebContracts;
namespace BookRental.Services.Contracts
{
    public class ServiceError:IServiceError
    {
        /// <summary>Gets or sets the code for this error
        /// 
        /// </summary>
        /// <value>The code for this error </value>

        public string Code { get; private set; }

        /// <summary>
        /// Gets or sets the description for this error
        /// </summary>
        /// <value>The Description for this error</value>

        public string Description { get; private set; }

        public ServiceError(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
