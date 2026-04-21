namespace Microservicio.Booking.DataManagement.Models;

public class FacturacionFiltroDataModel
{
    public string? Estado { get; set; }
    public int? IdCliente { get; set; }
    public DateTime? FechaEmisionInicio { get; set; }
    public DateTime? FechaEmisionFin { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TamanioPagina { get; set; } = 10;
}
