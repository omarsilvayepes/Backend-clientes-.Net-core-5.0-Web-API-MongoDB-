using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object result { get; set; }
        public string DisplayMessage { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
