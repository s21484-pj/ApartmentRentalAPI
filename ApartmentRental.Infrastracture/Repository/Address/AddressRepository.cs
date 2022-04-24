using ApartmentRental.Infrastracture.Context;
using ApartmentRental.Infrastracture.Entities;
using ApartmentRental.Infrastracture.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastracture.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly MainContext _mainContext;

    public AddressRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        var addresses = await _mainContext.Address.ToListAsync();

        return addresses;
    }

    public async Task<Address> GetByIdAsync(int id)
    {
        var address = await _mainContext.Address.SingleOrDefaultAsync(x => x.Id == id);
        
        if (address != null)
        {
            return address;
        }
        
        throw new EntityNotFoundException();
    }

    public async Task AddAsync(Address entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Address entity)
    {
        var addressToUpdate = await _mainContext.Address.SingleOrDefaultAsync(x => x.Id == entity.Id);

        if (addressToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        addressToUpdate.Street = entity.Street;
        addressToUpdate.FlatNumber = entity.FlatNumber;
        addressToUpdate.BuildingNumber = entity.BuildingNumber;
        addressToUpdate.City = entity.City;
        addressToUpdate.Postcode = entity.Postcode;
        addressToUpdate.Country = entity.Country;
        addressToUpdate.DateOfUpdate = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var addressToDelete = await _mainContext.Address.SingleOrDefaultAsync(x => x.Id == id);
        if (addressToDelete != null)
        {
            _mainContext.Address.Remove(addressToDelete);
            await _mainContext.SaveChangesAsync();
        }

        throw new EntityNotFoundException();
    }
}