Imports System
Imports System.Web
Imports DevExpress.XtraReports.Web.DocumentViewer

Namespace MVCxDocumentViewerWithReportServerMVC3.Controllers
    Public Class DocumentViewerController
        Inherits Controller

        Public Function DocumentViewer(ByVal model As ReportModel) As ActionResult
            Return View(model)
        End Function

        Public Function DocumentViewerPartial(ByVal reportID As Integer) As ActionResult
            Return PartialView("DocumentViewerPartial", New ReportModel() With {.ReportID = reportID})
        End Function

        Public Function DocumentViewerExportTo(ByVal reportID As Integer) As ActionResult
            Dim remoteSourceSettings As New MVCxDocumentViewerRemoteSourceSettings() With { _
                .AuthenticationType = DevExpress.ReportServer.Printing.AuthenticationType.Windows, _
                .ReportId = reportID, _
                .ServerUri = "http://localhost:83/" _
            }
            Return DocumentViewerExtension.ExportToAsync(remoteSourceSettings, Request).Result
        End Function
    End Class
End Namespace
