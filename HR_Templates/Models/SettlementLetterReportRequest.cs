namespace HR_Templates.Models
{
    public class SettlementLetterReportRequest
    {
        public string ReportName { get; set; }

        public string rpNombref { get; set; }

        public string rpNombreEmpresa { get; set; }

        public DateTime rpFechaf { get; set; }

        public int rpNumero { get; set; }

        public string rpTexto { get; set; }

        public DateTime rpFechaFinal { get; set; }
    }
}
