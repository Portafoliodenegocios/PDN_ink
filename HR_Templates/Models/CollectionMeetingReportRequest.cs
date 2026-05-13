
namespace HR_Templates.Models
{
    public class CollectionMeetingReportRequest
    {
        public string ReportName { get; set; }

        public string rpAsistentes { get; set; }

        public DateTime? rpFechaCarteraVencida { get; set; }

        public string rpReporteCarteraVencida { get; set; }

        public string rpProceso { get; set; }

        public string rpConvenios { get; set; }

        public string rpDesistimientos { get; set; }
    }
}