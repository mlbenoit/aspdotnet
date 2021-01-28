using System;
using System.Collections.Generic;
using System.Net;
using BookRental.WebContracts;

namespace BookRental.Services.Contracts
{
    public class ServiceResult
    {
        public bool isOk { get; protected set; }
        public bool IsError => !isOk;
        public HttpStatusCode Status {get; protected set;}
        public List<IServiceError> ErrorMessages { get; set; } = new List<IServiceError>();

        public ServiceResult(HttpStatusCode status, bool isOk)
        {
            Status = status;
            isOk = isOk;
        }

        public virtual void AddError(string code, string description)
        {
            ServiceError error = new ServiceError(code, description);
            ErrorMessages.Add(error);
        }
    }
}
