using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductiveBoard.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}