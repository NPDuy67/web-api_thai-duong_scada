using Microsoft.EntityFrameworkCore.Storage;
using ThaiDuongScada.Domain.SeedWork;
using System;
using ThaiDuongScada.Infrastructure.EntityConfigurations;
using ThaiDuongScada.Domain.AggregateModels.MouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

namespace ThaiDuongScada.Infrastructure;
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "application";

    private IDbContextTransaction? _currentTransaction;
    private readonly IMediator _mediator;

    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceMould> DeviceMoulds { get; set; }
    public DbSet<Mould> Moulds { get; set; }
    public DbSet<ShiftReport> ShiftReports { get; set; }
    public DbSet<MachineStatus> MachineStatus { get; set; } 

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ApplicationDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DeviceMouldEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MouldEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ShiftReportEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MachineStatusEntityTypeConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync();

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
