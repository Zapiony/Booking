namespace Microservicio.Booking.Business.DTOs;

/// <summary>
/// Resultado paginado expuesto por la capa de negocio (equivalente funcional a DataPagedResult con DTOs).
/// </summary>
public sealed class PagedResultado<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int PaginaActual { get; init; }
    public int TamanoPagina { get; init; }
    public int TotalRegistros { get; init; }
    public int TotalPaginas { get; init; }
    public bool TienePaginaAnterior { get; init; }
    public bool TienePaginaSiguiente { get; init; }
}
