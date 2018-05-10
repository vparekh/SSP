namespace SSPWebUI.Areas.MVC.Models.EF
{
    using System;

    public partial class Doc
    {
        public int Id { get; set; }
        public string DocBytes { get; set; }
        public string Comment { get; set; }
    }
}