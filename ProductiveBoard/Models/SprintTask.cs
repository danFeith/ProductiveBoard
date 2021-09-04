using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductiveBoard.Models
{
    public class SprintTask
    {
        public long taskId { get; set; }
        public Task task { get; set; }

        public long sprintId { get; set; }
        public Sprint sprint { get; set; }
    }
}
