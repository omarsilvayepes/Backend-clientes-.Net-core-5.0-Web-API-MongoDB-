using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Models.DTO
{
    public class UserDTO
    { 

        [Required, MinLength(3)]
        public string UserName { get; set; }
        [Required, MinLength(3)]
        public string PassWord { get; set; }
        
    }
}
