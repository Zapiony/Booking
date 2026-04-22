using Microservicio.Booking.Business.DTOs.Facturacion;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

public static class FacturacionBusinessMapper
{
    public static FacturacionDataModel ToDataModel(CrearFacturacionRequest request)
    {
        return new FacturacionDataModel
        {
            IdCliente = request.IdCliente,
            IdServicio = request.IdServicio,
            NumeroFactura = request.NumeroFactura,
            Subtotal = request.Subtotal,
            ValorIva = request.ValorIva,
            Total = request.Total,
            ObservacionesFactura = request.ObservacionesFactura,
            OrigenCanalFactura = request.OrigenCanalFactura,

            Estado = "ABI",
            FechaRegistroUtc = DateTimeOffset.UtcNow,
            CreadoPorUsuario = request.CreadoPorUsuario,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    public static FacturacionDataModel ToDataModel(ActualizarFacturacionRequest request, FacturacionDataModel existingModel)
    {
        existingModel.NumeroFactura = request.NumeroFactura;
        existingModel.Subtotal = request.Subtotal;
        existingModel.ValorIva = request.ValorIva;
        existingModel.Total = request.Total;
        existingModel.ObservacionesFactura = request.ObservacionesFactura;
        existingModel.OrigenCanalFactura = request.OrigenCanalFactura;

        existingModel.FechaModificacionUtc = DateTimeOffset.UtcNow;
        existingModel.ModificadoPorUsuario = request.ModificadoPorUsuario;
        existingModel.ModificacionIp = request.ModificacionIp;
        existingModel.ServicioOrigen = request.ServicioOrigen;

        return existingModel;
    }

    public static FacturacionResponse ToResponse(FacturacionDataModel model)
    {
        return new FacturacionResponse
        {
            GuidFactura = model.GuidFactura,
            IdCliente = model.IdCliente,
            IdServicio = model.IdServicio,
            NumeroFactura = model.NumeroFactura,
            FechaEmision = model.FechaEmision,
            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            Total = model.Total,
            ObservacionesFactura = model.ObservacionesFactura,
            OrigenCanalFactura = model.OrigenCanalFactura,
            Estado = model.Estado,
            FechaRegistroUtc = model.FechaRegistroUtc
        };
    }
}
