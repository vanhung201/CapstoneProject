//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessConnectManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BusinessUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BusinessUser()
        {
            this.InternshipResults = new HashSet<InternshipResult>();
            this.MOUs = new HashSet<MOU>();
            this.Registrations = new HashSet<Registration>();
            this.CooperationCategories = new HashSet<CooperationCategory>();
        }
    
        public string Business_ID { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> Last_Access { get; set; }
        public Nullable<int> Status_ID { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string BusinessPhone { get; set; }
        public string Website { get; set; }
        public string Fanpage { get; set; }
        public string BusinessLogo { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone_1 { get; set; }
        public string ContactPhone_2 { get; set; }
        public string EmailContact { get; set; }
    
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternshipResult> InternshipResults { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOU> MOUs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registration> Registrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CooperationCategory> CooperationCategories { get; set; }
    }
}
