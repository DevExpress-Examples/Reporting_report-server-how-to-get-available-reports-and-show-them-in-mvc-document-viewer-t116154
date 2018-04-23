using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCxDocumentViewerWithReportServerMVC3.Models {
    public class CatalogModel {
        public IEnumerable<ReportModel> ReportList { get; set; }
    }
}