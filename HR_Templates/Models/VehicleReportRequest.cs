namespace HR_Templates.Models
{
    public class VehicleReportRequest
    {
        public string ReportName { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public DateTime rpFechaDocumento { get; set; }

        public string rpNombreColaborador { get; set; }
        public string rpPuestoColaborador { get; set; }
        public int rpNEmpleado { get; set; }
        public string rpArea { get; set; }
        public bool rpentrega { get; set; }
        public bool rpDevolución { get; set; }
        public DateTime rpFechaEntrega { get; set; }
        public DateTime rpFechaDevolucion { get; set; }
        public bool rpPolizaSeguro { get; set; }
        public bool rpTarjetaCirculacion { get; set; }
        public bool rpVerificacion { get; set; }
        public bool rpManual { get; set; }
        public string rpMarcaTipo { get; set; }
        public string rpPlacas { get; set; }
        public int rpKilometrajeInicial { get; set; }
        public int? rpKilometrajeFinal { get; set; }
        public bool rpUsado { get; set; }
        public int rpModelo { get; set; }
        public string rpOtro { get; set; }
        public string rpObservacionCarroceria { get; set; }
        public string rpObservacionLlantas { get; set; }
        public string rpObservacionIntExt { get; set; }
        public string rpNombreEntrega { get; set; }
        public string rpNombreRecibePND { get; set; }
        public string rpNombreEntrego { get; set; }
    }
}
