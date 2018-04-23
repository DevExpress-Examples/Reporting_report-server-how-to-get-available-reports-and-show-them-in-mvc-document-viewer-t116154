Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.Threading.Tasks
Imports System.Web.Mvc
Imports DevExpress.ReportServer.ServiceModel.Client
Imports DevExpress.ReportServer.ServiceModel.DataContracts
Imports DevExpress.Web.Mvc
Imports MVCxDocumentViewerWithReportServerMVC3.Models

Namespace MVCxDocumentViewerWithReportServerMVC3.Controllers
	Public Class GalleryController
		Inherits Controller
		Public Function ReportList() As ActionResult
			Dim authEndpoint As New EndpointAddress("http://localhost:83/WindowsAuthentication/AuthenticationService.svc")
			Dim authClientFactory As New AuthenticationServiceClientFactory(authEndpoint, DevExpress.ReportServer.Printing.AuthenticationType.Windows)
			Dim behavior As FormsAuthenticationEndpointBehavior = New DevExpress.ReportServer.ServiceModel.Client.FormsAuthenticationEndpointBehavior()
			authClientFactory.ChannelFactory.Endpoint.Behaviors.Add(behavior)
			Dim authClient As IAuthenticationServiceClient = authClientFactory.Create()

			Dim authTcs As New TaskCompletionSource(Of Boolean)()
			authClient.UseSynchronizationContext = False
            authClient.Login(Nothing, Nothing, Nothing, Function(e) AnonymousMethod1(e, authTcs))
			If (Not authTcs.Task.Result) Then
				Throw New Exception("Failed to login to the Report Server")
			End If

			Dim serviceFacadeEndpoint As New EndpointAddress("http://localhost:83/ReportServerFacade.svc")
			Dim serverFacadeClientFactory As New ReportServerClientFactory(serviceFacadeEndpoint)
			serverFacadeClientFactory.ChannelFactory.Endpoint.Behaviors.Add(behavior)
			Dim reportServerClient As IReportServerClient = serverFacadeClientFactory.Create()
			Dim tcs As New TaskCompletionSource(Of IEnumerable(Of ReportModel))()
			reportServerClient.UseSynchronizationContext = False
            reportServerClient.GetReports(Nothing, Function(args) AnonymousMethod2(args, tcs))
			Dim taskResult As IEnumerable(Of ReportModel) = tcs.Task.Result
			Return View(taskResult)
		End Function
		
        Private Function AnonymousMethod1(ByVal e As Object, ByVal authTcs As TaskCompletionSource(Of Boolean)) As Boolean
            If e.Error IsNot Nothing Then
                authTcs.SetException(e.Error)
            Else
                authTcs.SetResult(e.Result)
            End If
            Return True
        End Function
		
        Private Function AnonymousMethod2(ByVal args As Object, ByVal tcs As TaskCompletionSource(Of IEnumerable(Of ReportModel))) As Boolean
            If args.Error IsNot Nothing Then
                tcs.SetException(args.Error)
            Else
                Dim reports As IEnumerable(Of ReportCatalogItemDto) = args.Result
                tcs.SetResult(If(reports Is Nothing, Nothing, reports.Select(Function(x) New ReportModel() With {.ReportID = x.Id, .ReportName = x.Name})))
            End If
            Return True
        End Function

		Public Function DocumentViewer(ByVal model As ReportModel) As ActionResult
			Return RedirectToAction("DocumentViewer", "DocumentViewer", model)
		End Function
	End Class
End Namespace
