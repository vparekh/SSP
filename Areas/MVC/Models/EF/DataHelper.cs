using System;
using System.Linq;
using DevExpress.XtraRichEdit;
using SSPWebUI.Areas.MVC.Models.EF;

namespace SSPWebUI.Areas.MVC.Models
{
    public class DataHelper
    {
        public static byte[] GetDocument()
        {
            DataClassesDataContext context = new DataClassesDataContext();
            return System.Text.Encoding.Default.GetBytes(context.Docs.FirstOrDefault().DocBytes).ToArray();
           // return context.Docs.FirstOrDefault().DocBytes.ToArray();
        }

        public static void SaveDocument(byte[] bytes)
        {
            DataClassesDataContext context = new DataClassesDataContext();
            context.Docs.FirstOrDefault().DocBytes = System.Text.Encoding.Default.GetString( bytes);
            context.SaveChanges();
        }
    }
    public class RichEditData
    {
        public string DocumentId { get; set; }
        public DocumentFormat DocumentFormat { get; set; }
        public string Document { get; set; }
    }
}