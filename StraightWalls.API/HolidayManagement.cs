//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StraightWalls.API
{
    using System;
    using System.Collections.Generic;
    
    public partial class HolidayManagement
    {
        public int id { get; set; }
        public int employee_id { get; set; }
        public System.DateTime from_date { get; set; }
        public System.DateTime to_date { get; set; }
        public bool is_canceled { get; set; }
        public Nullable<bool> is_approved { get; set; }
        public System.DateTime created_on { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> updated_on { get; set; }
        public string updated_by { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
