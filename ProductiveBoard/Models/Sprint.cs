using System.Collections.Generic;

namespace ProductiveBoard.Models
{
    public class Sprint
    {
        public long Id { get; set; }
        public string name { get; set; }
        public List<SprintTask> sprintTasks { get; set; }
    }
}