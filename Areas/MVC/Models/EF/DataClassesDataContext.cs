namespace SSPWebUI.Areas.MVC.Models.EF
{
    using System;
    using System.Data.Entity;

    public partial class DataClassesDataContext : DbContext
    {
        public DataClassesDataContext()
            : base("name=eCCData")
        {
        }

        public virtual DbSet<Doc> Docs { get; set; }
    }
}