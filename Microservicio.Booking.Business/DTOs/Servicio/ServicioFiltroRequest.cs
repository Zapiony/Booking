namespace Microservicio.Booking.Business.DTOs.Servicio;

public sealed class ServicioFiltroRequest
{
    public string? Termino { get; set; }
    public Guid? GuidTipoServicio { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;
}
