using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

public interface IFacturacionDataService
{
    Task<FacturacionDataModel?> ObtenerPorIdAsync(int idFactura, CancellationToken cancellationToken = default);
    Task<FacturacionDataModel?> ObtenerPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default);
    Task<FacturacionDataModel?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default);
    Task<DataPagedResult<FacturacionDataModel>> BuscarAsync(FacturacionFiltroDataModel filtro, CancellationToken cancellationToken = default);
    Task<FacturacionDataModel> CrearAsync(FacturacionDataModel model, CancellationToken cancellationToken = default);
    Task<FacturacionDataModel?> ActualizarAsync(FacturacionDataModel model, CancellationToken cancellationToken = default);
    Task<bool> EliminarLogicoAsync(Guid guidFactura, string eliminadoPorUsuario, CancellationToken cancellationToken = default);
}
