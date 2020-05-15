using System;

namespace WebAPI.Models
{
    public class TaskDetails
    {
        public string TaskName { get; set; } 
        public DateTime TimeToSchedule { get; set; }
        public string TaskPath { get; set; }
    }
}