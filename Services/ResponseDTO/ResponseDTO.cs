using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ResponseDTO
{
    public class ResponseDTO<T>
    {
        public int statusCode { get; set; } = 200;
        public string? message { get; set; }
        public T? payload { get; set; } 
    }
}
