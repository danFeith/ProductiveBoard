using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductiveBoard.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public TaskStatus Status { get; set; }
        public long TypeId { get; set; }
        public TaskType Type { get; set; }

    }
}