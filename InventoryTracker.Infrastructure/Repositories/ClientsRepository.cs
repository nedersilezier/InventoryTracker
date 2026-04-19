using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Infrastructure.Repositories
{
    public class ClientsRepository: IClientsRepository
    {
        private readonly AppDbContext _context;
        public ClientsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ClientCodeExistsAsync(string clientCode, CancellationToken cancellationToken)
        {
            return await _context.Clients.AnyAsync(c => c.ClientCode == clientCode, cancellationToken);
        }
        public Task AddClient(Client client)
        {
            _context.Clients.Add(client);
            return Task.CompletedTask;
        }
        public async Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .Include(c => c.Address)
                .ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);
        }
    }
}
