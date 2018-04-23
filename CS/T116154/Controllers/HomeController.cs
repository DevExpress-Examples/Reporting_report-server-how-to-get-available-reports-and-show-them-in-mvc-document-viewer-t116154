using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevExpress.ReportServer.ServiceModel.Client;
using DevExpress.ReportServer.ServiceModel.ConnectionProviders;
using T116154.Models;

namespace T116154.Controllers {
    public class HomeController : Controller {
        public const string ServerAddress = "http://reportserver.devexpress.com/";

        public ActionResult Index() {
            IReportServerClient client = new GuestConnectionProvider(ServerAddress).ConnectAsync().Result;
            IEnumerable<ReportModel> model = client.GetReportsAsync(null).Result.Select(x => new ReportModel { Id = x.Id, Name = x.Name });
            return View(model);
        }

        public ActionResult DocumentViewerPartial(int reportID) {
            return PartialView("DocumentViewerPartial", reportID);
        }

        public ActionResult Display(int id) {
            return View(id);
        }

        //[NonAction]
        //public void OnRequestCredentials(object sender, WebAuthenticatorLoginEventArgs e) {
        //    e.Credential = new WebCredential(UserName, Password);
        //    e.Handled = true;
        //}
    }
}
