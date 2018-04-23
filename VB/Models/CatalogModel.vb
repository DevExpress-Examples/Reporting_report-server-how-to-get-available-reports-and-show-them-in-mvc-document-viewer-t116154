Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace MVCxDocumentViewerWithReportServerMVC3.Models
	Public Class CatalogModel
		Private privateReportList As IEnumerable(Of ReportModel)
		Public Property ReportList() As IEnumerable(Of ReportModel)
			Get
				Return privateReportList
			End Get
			Set(ByVal value As IEnumerable(Of ReportModel))
				privateReportList = value
			End Set
		End Property
	End Class
End Namespace