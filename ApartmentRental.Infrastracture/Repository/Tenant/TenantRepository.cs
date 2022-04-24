using ApartmentRental.Infrastracture.Context;
using ApartmentRental.Infrastracture.Entities;
using ApartmentRental.Infrastracture.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastracture.Repository;

public class TenantRepository : ITenantRepository
{
    private readonly MainContext _mainContext;

    public TenantRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        var tenants = await _mainContext.Tenant.ToListAsync();

        return tenants;
    }

    public async Task<Tenant> GetByIdAsync(int id)
    {
        var tenant = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == id);
        
        if (tenant != null)
        {
            return tenant;
        }
        
        throw new EntityNotFoundException();
    }

    public async Task AddAsync(Tenant entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tenant entity)
    {
        var tenantToUpdate = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == entity.Id);

        if (tenantToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        tenantToUpdate.Apartment = entity.Apartment;
        tenantToUpdate.DateOfUpdate = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var tenantToDelete = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == id);
        if (tenantToDelete != null)
        {
            _mainContext.Tenant.Remove(tenantToDelete);
            await _mainContext.SaveChangesAsync();
        }

        throw new EntityNotFoundException();
    }
}