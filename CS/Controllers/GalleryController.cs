using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.ReportServer.ServiceModel.Client;
using DevExpress.ReportServer.ServiceModel.DataContracts;
using DevExpress.Web.Mvc;
using MVCxDocumentViewerWithReportServerMVC3.Models;

namespace MVCxDocumentViewerWithReportServerMVC3.Controllers {
    public class GalleryController : Controller {
        public ActionResult ReportList() {
            EndpointAddress authEndpoint = new EndpointAddress("http://localhost:83/WindowsAuthentication/AuthenticationService.svc");
            AuthenticationServiceClientFactory authClientFactory = new AuthenticationServiceClientFactory(authEndpoint, DevExpress.ReportServer.Printing.AuthenticationType.Windows);
            FormsAuthenticationEndpointBehavior behavior = new DevExpress.ReportServer.ServiceModel.Client.FormsAuthenticationEndpointBehavior();
            authClientFactory.ChannelFactory.Endpoint.Behaviors.Add(behavior);
            IAuthenticationServiceClient authClient = authClientFactory.Create();

            TaskCompletionSource<bool> authTcs = new TaskCompletionSource<bool>();
            authClient.UseSynchronizationContext = false;
            authClient.Login(null, null, null, (e) => {
                if(e.Error != null)
                    authTcs.SetException(e.Error);
                else
                    authTcs.SetResult(e.Result);
            });
            if(!authTcs.Task.Result)
                throw new Exception("Failed to login to the Report Server");

            EndpointAddress serviceFacadeEndpoint = new EndpointAddress("http://localhost:83/ReportServerFacade.svc");
            ReportServerClientFactory serverFacadeClientFactory = new ReportServerClientFactory(serviceFacadeEndpoint);
            serverFacadeClientFactory.ChannelFactory.Endpoint.Behaviors.Add(behavior);
            IReportServerClient reportServerClient = serverFacadeClientFactory.Create();
            TaskCompletionSource<IEnumerable<ReportModel>> tcs = new TaskCompletionSource<IEnumerable<ReportModel>>();
            reportServerClient.UseSynchronizationContext = false;
            reportServerClient.GetReports(null, (args) => {
                if(args.Error != null)
                    tcs.SetException(args.Error);
                else {
                    IEnumerable<ReportCatalogItemDto> reports = args.Result;
                    tcs.SetResult(reports == null ? null : reports.Select(x => new ReportModel() { ReportID = x.Id, ReportName = x.Name }));
                }
            });
            IEnumerable<ReportModel> taskResult = tcs.Task.Result;
            return View(taskResult);
        }

        public ActionResult DocumentViewer(ReportModel model) {
            return RedirectToAction("DocumentViewer", "DocumentViewer", model);
        }
    }
}
