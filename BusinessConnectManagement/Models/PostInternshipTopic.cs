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
    
    public partial class PostInternshipTopic
    {
        public int ID { get; set; }
        public Nullable<int> Post_ID { get; set; }
        public Nullable<int> InternshipTopic_ID { get; set; }
        public Nullable<int> Business_ID { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual BusinessUser BusinessUser { get; set; }
        public virtual InternshipTopic InternshipTopic { get; set; }
        public virtual Post Post { get; set; }
    }
}
