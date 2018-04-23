Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraReports.Web.DocumentViewer
Imports MVCxDocumentViewerWithReportServerMVC3.Models

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
			Dim remoteSourceSettings As New MVCxDocumentViewerRemoteSourceSettings() With {.AuthenticationType = DevExpress.ReportServer.Printing.AuthenticationType.Windows, .ReportId = reportID, .ServerUri = "http://localhost:83/"}
			Return DocumentViewerExtension.ExportToAsync(remoteSourceSettings, Request).Result
		End Function
	End Class
End Namespace
