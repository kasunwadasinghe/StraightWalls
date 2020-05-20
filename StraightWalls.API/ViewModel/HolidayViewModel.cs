using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StraightWalls.API.ViewModel
{
    public class HolidayViewModel
    {
        public int HolidayId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double NoDays { get { return (To - From).TotalDays; } }
        public string Status { get; set; }
        public bool isCanceled { get; set; }
    }
}