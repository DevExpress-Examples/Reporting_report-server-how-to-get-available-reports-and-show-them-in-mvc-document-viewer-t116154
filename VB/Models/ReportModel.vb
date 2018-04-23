Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace MVCxDocumentViewerWithReportServerMVC3.Models
	Public Class ReportModel
		Private privateReportID As Integer
		Public Property ReportID() As Integer
			Get
				Return privateReportID
			End Get
			Set(ByVal value As Integer)
				privateReportID = value
			End Set
		End Property
		Private privateReportName As String
		Public Property ReportName() As String
			Get
				Return privateReportName
			End Get
			Set(ByVal value As String)
				privateReportName = value
			End Set
		End Property
	End Class
End Namespace