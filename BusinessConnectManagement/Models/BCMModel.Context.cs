﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BCMEntities : DbContext
    {
        public BCMEntities()
            : base("name=BCMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BusinessCooperationCategory> BusinessCooperationCategories { get; set; }
        public virtual DbSet<BusinessUser> BusinessUsers { get; set; }
        public virtual DbSet<CooperationCategory> CooperationCategories { get; set; }
        public virtual DbSet<InternshipResult> InternshipResults { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<MOU> MOUs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<VanLangUser> VanLangUsers { get; set; }
        public virtual DbSet<YearStudy> YearStudies { get; set; }
    }
}
