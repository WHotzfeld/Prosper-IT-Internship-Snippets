using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleUsers.Models
{
    public class TimeOffEvent : Event
    {
        [DataType(DataType.Date)]
        [Display(Name = "Submitted")]
        public DateTime? Submitted { get; set; }
        [Display(Name = "Request Status")]
        public bool? ActiveSchedule { get; set; }
    }
}