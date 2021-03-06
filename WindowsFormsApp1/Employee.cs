//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.HolidayManagements = new HashSet<HolidayManagement>();
            this.Users = new HashSet<User>();
        }
    
        public int employee_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> join_date { get; set; }
        public Nullable<int> department_id { get; set; }
        public Nullable<int> designation_id { get; set; }
        public Nullable<bool> is_active { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Designation Designation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HolidayManagement> HolidayManagements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
