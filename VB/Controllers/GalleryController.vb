Imports System
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.Threading.Tasks
Imports DevExpress.ReportServer.ServiceModel.Client
Imports DevExpress.ReportServer.ServiceModel.DataContracts

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
            authClient.Login(Nothing, Nothing, Nothing, Sub(e)
                If e.Error IsNot Nothing Then
                    authTcs.SetException(e.Error)
                Else
                    authTcs.SetResult(e.Result)
                End If
            End Sub)
            If Not authTcs.Task.Result Then
                Throw New Exception("Failed to login to the Report Server")
            End If

            Dim serviceFacadeEndpoint As New EndpointAddress("http://localhost:83/ReportServerFacade.svc")
            Dim serverFacadeClientFactory As New ReportServerClientFactory(serviceFacadeEndpoint)
            serverFacadeClientFactory.ChannelFactory.Endpoint.Behaviors.Add(behavior)
            Dim reportServerClient As IReportServerClient = serverFacadeClientFactory.Create()
            Dim tcs As New TaskCompletionSource(Of IEnumerable(Of ReportModel))()
            reportServerClient.UseSynchronizationContext = False
            reportServerClient.GetReports(Nothing, Sub(args)
                If args.Error IsNot Nothing Then
                    tcs.SetException(args.Error)
                Else
                    Dim reports As IEnumerable(Of ReportCatalogItemDto) = args.Result
                    tcs.SetResult(If(reports Is Nothing, Nothing, reports.Select(Function(x) New ReportModel() With { _
                        .ReportID = x.Id, _
                        .ReportName = x.Name _
                    })))
                End If
            End Sub)
            Dim taskResult As IEnumerable(Of ReportModel) = tcs.Task.Result
            Return View(taskResult)
        End Function

        Public Function DocumentViewer(ByVal model As ReportModel) As ActionResult
            Return RedirectToAction("DocumentViewer", "DocumentViewer", model)
        End Function
    End Class
End Namespace
