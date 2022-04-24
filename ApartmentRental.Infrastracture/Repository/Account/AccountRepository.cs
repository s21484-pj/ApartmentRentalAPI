using ApartmentRental.Infrastracture.Context;
using ApartmentRental.Infrastracture.Entities;
using ApartmentRental.Infrastracture.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastracture.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly MainContext _mainContext;

    public AccountRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        var accounts = await _mainContext.Account.ToListAsync();

        foreach (var account in accounts)
        {
            await _mainContext.Entry(account).Reference(x => x.Address).LoadAsync();
        }

        return accounts;
    }

    public async Task<Account> GetByIdAsync(int id)
    {
        var account = await _mainContext.Account.SingleOrDefaultAsync(x => x.Id == id);
        
        if (account != null)
        {
            await _mainContext.Entry(account).Reference(x => x.Address).LoadAsync();
            return account;
        }

        throw new EntityNotFoundException();
    }

    public async Task AddAsync(Account entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account entity)
    {
        var accountToUpdate = await _mainContext.Account.SingleOrDefaultAsync(x => x.Id == entity.Id);

        if (accountToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        accountToUpdate.Name = entity.Name;
        accountToUpdate.LastName = entity.LastName;
        accountToUpdate.Email = entity.Email;
        accountToUpdate.PhoneNumber = entity.PhoneNumber;
        accountToUpdate.IsActive = entity.IsActive;
        accountToUpdate.DateOfUpdate = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var accountToDelete = await _mainContext.Account.SingleOrDefaultAsync(x => x.Id == id);
        if (accountToDelete != null)
        {
            _mainContext.Account.Remove(accountToDelete);
            await _mainContext.SaveChangesAsync();
        }

        throw new EntityNotFoundException();
    }
}