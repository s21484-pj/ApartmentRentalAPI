using ApartmentRental.Infrastracture.Context;
using ApartmentRental.Infrastracture.Entities;
using ApartmentRental.Infrastracture.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastracture.Repository;

public class LandlordRepository : ILandlordRepository
{
    private readonly MainContext _mainContext;

    public LandlordRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Landlord>> GetAllAsync()
    {
        var landlords = await _mainContext.Landlord.ToListAsync();

        return landlords;
    }

    public async Task<Landlord> GetByIdAsync(int id)
    {
        var landlord = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == id);
        
        if (landlord != null)
        {
            return landlord;
        }
        
        throw new EntityNotFoundException();
    }

    public async Task AddAsync(Landlord entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Landlord entity)
    {
        var landlordToUpdate = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == entity.Id);

        if (landlordToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        landlordToUpdate.Apartments = entity.Apartments;
        landlordToUpdate.DateOfUpdate = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var landlordToDelete = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == id);
        if (landlordToDelete != null)
        {
            _mainContext.Landlord.Remove(landlordToDelete);
            await _mainContext.SaveChangesAsync();
        }

        throw new EntityNotFoundException();
    }
}