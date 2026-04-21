using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Mappers;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Implementación del servicio de gestión de datos para facturación.
/// Coordina repositorios a través de la UoW, mapea entidades a modelos
/// y confirma cambios. No expone EF Core a capas superiores.
/// </summary>
public class FacturacionDataService : IFacturacionDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public FacturacionDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // =========================================================================
    // Consultas simples
    // =========================================================================

    public async Task<FacturacionDataModel?> ObtenerPorIdAsync(int idFactura, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacturacionRepository.ObtenerPorIdAsync(idFactura, cancellationToken);
        return entity is null ? null : FacturacionDataMapper.ToDataModel(entity);
    }

    public async Task<FacturacionDataModel?> ObtenerPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacturacionRepository.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        return entity is null ? null : FacturacionDataMapper.ToDataModel(entity);
    }

    public async Task<FacturacionDataModel?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacturacionRepository.ObtenerPorNumeroAsync(numeroFactura, cancellationToken);
        return entity is null ? null : FacturacionDataMapper.ToDataModel(entity);
    }

    // =========================================================================
    // Consultas paginadas y filtradas
    // =========================================================================

    public async Task<DataPagedResult<FacturacionDataModel>> BuscarAsync(FacturacionFiltroDataModel filtro, CancellationToken cancellationToken = default)
    {
        var resultado = await _unitOfWork.FacturacionQueryRepository.ListarFacturacionesAsync(filtro.PaginaActual, filtro.TamanioPagina, cancellationToken);

        return new DataPagedResult<FacturacionDataModel>
        {
            Items = resultado.Items.Select(dto => new FacturacionDataModel
            {
                GuidFactura = dto.GuidFactura,
                NumeroFactura = dto.NumeroFactura,
                Total = dto.Total,
                Estado = dto.Estado,
                FechaRegistroUtc = dto.FechaRegistroUtc
            }).ToList(),
            PageNumber = resultado.PaginaActual,
            PageSize = resultado.TamanoPagina,
            TotalRecords = resultado.TotalRegistros
        };
    }

    // =========================================================================
    // Escritura
    // =========================================================================

    public async Task<FacturacionDataModel> CrearAsync(FacturacionDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = FacturacionDataMapper.ToEntity(model);
        await _unitOfWork.FacturacionRepository.AgregarAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FacturacionDataMapper.ToDataModel(entity);
    }

    public async Task<FacturacionDataModel?> ActualizarAsync(FacturacionDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacturacionRepository.ObtenerPorGuidAsync(model.GuidFactura, cancellationToken);
        if (entity is null) return null;

        entity.IdCliente = model.IdCliente;
        entity.IdServicio = model.IdServicio;
        entity.NumeroFactura = model.NumeroFactura;
        entity.FechaEmision = model.FechaEmision;
        entity.Subtotal = model.Subtotal;
        entity.ValorIva = model.ValorIva;
        entity.Total = model.Total;
        entity.ObservacionesFactura = model.ObservacionesFactura;
        entity.OrigenCanalFactura = model.OrigenCanalFactura;
        entity.Estado = model.Estado;
        entity.FechaInhabilitacionUtc = model.FechaInhabilitacionUtc;
        entity.MotivoInhabilitacion = model.MotivoInhabilitacion;
        entity.ModificadoPorUsuario = model.ModificadoPorUsuario;
        entity.ModificacionIp = model.ModificacionIp;
        entity.ServicioOrigen = model.ServicioOrigen;

        _unitOfWork.FacturacionRepository.Actualizar(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return FacturacionDataMapper.ToDataModel(entity);
    }

    public async Task<bool> EliminarLogicoAsync(Guid guidFactura, string eliminadoPorUsuario, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.FacturacionRepository.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        if (entity is null) return false;

        _unitOfWork.FacturacionRepository.EliminarLogico(entity);
        entity.ModificadoPorUsuario = eliminadoPorUsuario;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
