using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Web.DocumentViewer;
using MVCxDocumentViewerWithReportServerMVC3.Models;

namespace MVCxDocumentViewerWithReportServerMVC3.Controllers {
    public class DocumentViewerController : Controller {
        public ActionResult DocumentViewer(ReportModel model) {
            return View(model);
        }

        public ActionResult DocumentViewerPartial(int reportID) {
            return PartialView("DocumentViewerPartial", new ReportModel() { ReportID = reportID });
        }

        public ActionResult DocumentViewerExportTo(int reportID) {
            MVCxDocumentViewerRemoteSourceSettings remoteSourceSettings = new MVCxDocumentViewerRemoteSourceSettings() {
                AuthenticationType = DevExpress.ReportServer.Printing.AuthenticationType.Windows,
                ReportId = reportID,
                ServerUri = "http://localhost:83/"
            };
            return DocumentViewerExtension.ExportToAsync(remoteSourceSettings, Request).Result;
        }
    }
}
