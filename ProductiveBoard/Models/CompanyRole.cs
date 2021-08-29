using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMan.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Boolean IsManager { get; set; }
    }
}
