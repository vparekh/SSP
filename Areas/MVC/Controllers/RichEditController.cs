using System;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraRichEdit;
using SSPWebUI.Areas.MVC.Models;

namespace SSPWebUI.Areas.MVC.Controllers
{
    public class RichEditController : Controller
    {
        public ActionResult Index()
        {
            var model = new RichEditData()
            {
                DocumentId = Guid.NewGuid().ToString(),
                DocumentFormat = DocumentFormat.Rtf,
                Document = System.Text.Encoding.Default.GetString(DataHelper.GetDocument())
            };
            return View(model);
        }

        public ActionResult RichEditPartial()
        {
            return RichEditExtension.GetCallbackResult("RichEditName", p =>
            {
                p.Saving(e =>
                {
                    byte[] docBytes = RichEditExtension.SaveCopy("RichEditName", DocumentFormat.Html);
                    DataHelper.SaveDocument(docBytes);
                    e.Handled = true;
                });
            });
        }
    }
}