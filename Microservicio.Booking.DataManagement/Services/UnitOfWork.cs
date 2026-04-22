using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataAccess.Queries;
using Microservicio.Booking.DataAccess.Queries.Interfaces;
using Microservicio.Booking.DataAccess.Repositories;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microservicio.Booking.DataManagement.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingDbContext _context;

    // Usuarios
    public IUsuarioRepository UsuarioRepository { get; }
    public IRolRepository RolRepository { get; }

    // Clientes
    public IClienteRepository ClienteRepository { get; }

    // Servicios
    public IServicioRepository ServicioRepository { get; }
    public ITipoServicioRepository TipoServicioRepository { get; }

    // Facturación
    public IFacturacionRepository FacturacionRepository { get; }

    // Auditoría
    public ILogAuditoriaRepository LogAuditoriaRepository { get; }

    // Queries (solo lectura — AsNoTracking)
    public UsuarioQueryRepository UsuarioQueryRepository { get; }
    public ClienteQueryRepository ClienteQueryRepository { get; }
    public ServicioQueryRepository ServicioQueryRepository { get; }
    public IFacturacionQueryRepository FacturacionQueryRepository { get; }
    public ILogAuditoriaQueryRepository LogAuditoriaQueryRepository { get; }

    public UnitOfWork(BookingDbContext context)
    {
        _context = context;

        UsuarioRepository = new UsuarioRepository(_context);
        RolRepository = new RolRepository(_context);
        ClienteRepository = new ClienteRepository(_context);
        ServicioRepository = new ServicioRepository(_context);
        TipoServicioRepository = new TipoServicioRepository(_context);
        FacturacionRepository = new FacturacionRepository(_context);
        LogAuditoriaRepository = new LogAuditoriaRepository(_context);

        UsuarioQueryRepository = new UsuarioQueryRepository(_context);
        ClienteQueryRepository = new ClienteQueryRepository(_context);
        ServicioQueryRepository = new ServicioQueryRepository(_context);
        FacturacionQueryRepository = new FacturacionQueryRepository(_context);
        LogAuditoriaQueryRepository = new LogAuditoriaQueryRepository(_context);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}