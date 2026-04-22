using Microservicio.Booking.Business.DTOs;
using Microservicio.Booking.Business.DTOs.Servicio;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

public static class ServicioBusinessMapper
{
    public static ServicioFiltroDataModel AFiltroData(ServicioFiltroRequest request)
    {
        return new ServicioFiltroDataModel
        {
            Termino = request.Termino,
            GuidTipoServicio = request.GuidTipoServicio,
            PaginaActual = request.PaginaActual,
            TamanoPagina = request.TamanoPagina
        };
    }

    public static ServicioDataModel ACrearDataModel(CrearServicioRequest request)
    {
        return new ServicioDataModel
        {
            GuidTipoServicio = request.GuidTipoServicio,
            RazonSocial = request.RazonSocial.Trim(),
            NombreComercial = request.NombreComercial?.Trim(),
            TipoIdentificacion = request.TipoIdentificacion.Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion.Trim(),
            CorreoContacto = request.CorreoContacto.Trim(),
            TelefonoContacto = request.TelefonoContacto?.Trim(),
            Direccion = request.Direccion?.Trim(),
            SitioWeb = request.SitioWeb?.Trim(),
            LogoUrl = request.LogoUrl?.Trim(),
            Estado = request.Estado.Trim(),
            CreadoPorUsuario = request.CreadoPorUsuario,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    public static void AplicarActualizacion(ActualizarServicioRequest request, ServicioDataModel destino)
    {
        var rv = RowVersionMapper.DesdeBase64(request.RowVersionBase64);
        if (rv is not null)
            destino.RowVersion = rv;

        destino.GuidTipoServicio = request.GuidTipoServicio;
        destino.RazonSocial = request.RazonSocial.Trim();
        destino.NombreComercial = request.NombreComercial?.Trim();
        destino.TipoIdentificacion = request.TipoIdentificacion.Trim();
        destino.NumeroIdentificacion = request.NumeroIdentificacion.Trim();
        destino.CorreoContacto = request.CorreoContacto.Trim();
        destino.TelefonoContacto = request.TelefonoContacto?.Trim();
        destino.Direccion = request.Direccion?.Trim();
        destino.SitioWeb = request.SitioWeb?.Trim();
        destino.LogoUrl = request.LogoUrl?.Trim();
        destino.Estado = request.Estado.Trim();
        destino.ModificadoPorUsuario = request.ModificadoPorUsuario;
        destino.ModificacionIp = request.ModificacionIp;
        destino.ServicioOrigen = request.ServicioOrigen;
    }

    public static ServicioResponse ARespuesta(ServicioDataModel modelo)
    {
        return new ServicioResponse
        {
            IdServicio = modelo.IdServicio,
            GuidServicio = modelo.GuidServicio,
            GuidTipoServicio = modelo.GuidTipoServicio,
            TipoServicioNombre = modelo.TipoServicioNombre,
            RazonSocial = modelo.RazonSocial,
            NombreComercial = modelo.NombreComercial,
            TipoIdentificacion = modelo.TipoIdentificacion,
            NumeroIdentificacion = modelo.NumeroIdentificacion,
            CorreoContacto = modelo.CorreoContacto,
            TelefonoContacto = modelo.TelefonoContacto,
            Direccion = modelo.Direccion,
            SitioWeb = modelo.SitioWeb,
            LogoUrl = modelo.LogoUrl,
            Estado = modelo.Estado,
            EsEliminado = modelo.EsEliminado,
            CreadoPorUsuario = modelo.CreadoPorUsuario,
            FechaRegistroUtc = modelo.FechaRegistroUtc,
            ModificadoPorUsuario = modelo.ModificadoPorUsuario,
            FechaModificacionUtc = modelo.FechaModificacionUtc,
            RowVersionBase64 = RowVersionMapper.ABase64(modelo.RowVersion)
        };
    }

    public static ServicioResumenResponse AResumenRespuesta(ServicioResumenDataModel modelo)
    {
        return new ServicioResumenResponse
        {
            GuidServicio = modelo.GuidServicio,
            RazonSocial = modelo.RazonSocial,
            NombreComercial = modelo.NombreComercial,
            TipoServicioNombre = modelo.TipoServicioNombre,
            Estado = modelo.Estado
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
