using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

namespace ListenTogether.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Room> Rooms { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}