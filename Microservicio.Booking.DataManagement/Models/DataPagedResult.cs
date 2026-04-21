namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo paginado propio de la capa de Gestión de Datos.
/// Cada capa expone su propio modelo de paginación para no arrastrar
/// el PagedResult de DataAccess hacia las capas superiores.
/// </summary>
public class DataPagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}