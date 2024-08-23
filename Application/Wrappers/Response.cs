using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public Response(){}
        
        public Response(T data , string message = null) 
        {
            Succeeded = true;
            Message = message ?? string.Empty;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
    }
}
