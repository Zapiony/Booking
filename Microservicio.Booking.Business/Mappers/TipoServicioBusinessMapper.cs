using Microservicio.Booking.Business.DTOs;
using Microservicio.Booking.Business.DTOs.TipoServicio;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

public static class TipoServicioBusinessMapper
{
    public static TipoServicioDataModel ACrearDataModel(CrearTipoServicioRequest request)
    {
        return new TipoServicioDataModel
        {
            Nombre = request.Nombre.Trim(),
            Descripcion = request.Descripcion?.Trim(),
            Estado = request.Estado.Trim(),
            CreadoPorUsuario = request.CreadoPorUsuario,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    public static void AplicarActualizacion(ActualizarTipoServicioRequest request, TipoServicioDataModel destino)
    {
        var rv = RowVersionMapper.DesdeBase64(request.RowVersionBase64);
        if (rv is not null)
            destino.RowVersion = rv;

        destino.Nombre = request.Nombre.Trim();
        destino.Descripcion = request.Descripcion?.Trim();
        destino.Estado = request.Estado.Trim();
        destino.ModificadoPorUsuario = request.ModificadoPorUsuario;
        destino.ModificacionIp = request.ModificacionIp;
        destino.ServicioOrigen = request.ServicioOrigen;
    }

    public static TipoServicioResponse ARespuesta(TipoServicioDataModel modelo)
    {
        return new TipoServicioResponse
        {
            GuidTipoServicio = modelo.GuidTipoServicio,
            Nombre = modelo.Nombre,
            Descripcion = modelo.Descripcion,
            Estado = modelo.Estado,
            EsEliminado = modelo.EsEliminado,
            CreadoPorUsuario = modelo.CreadoPorUsuario,
            FechaRegistroUtc = modelo.FechaRegistroUtc,
            ModificadoPorUsuario = modelo.ModificadoPorUsuario,
            FechaModificacionUtc = modelo.FechaModificacionUtc,
            RowVersionBase64 = RowVersionMapper.ABase64(modelo.RowVersion)
        };
    }

    public static PagedResultado<TDestino> APaginado<TOrigen, TDestino>(
        DataPagedResult<TOrigen> origen,
        Func<TOrigen, TDestino> mapearItem)
    {
        return new PagedResultado<TDestino>
        {
            Items = origen.Items.Select(mapearItem).ToList(),
            PaginaActual = origen.PaginaActual,
            TamanoPagina = origen.TamanoPagina,
            TotalRegistros = origen.TotalRegistros,
            TotalPaginas = origen.TotalPaginas,
            TienePaginaAnterior = origen.TienePaginaAnterior,
            TienePaginaSiguiente = origen.TienePaginaSiguiente
        };
    }
}
