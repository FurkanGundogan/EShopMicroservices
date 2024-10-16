using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
    public class BadRequestException : Exception
    {
        public string? Details { get; }

        public BadRequestException(string message) : base(message)
        {
        }
     
        public BadRequestException(string name, string details) : base(details)
        {
            Details = details;
        }
    }
}
