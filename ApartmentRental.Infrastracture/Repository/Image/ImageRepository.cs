using ApartmentRental.Infrastracture.Context;
using ApartmentRental.Infrastracture.Entities;
using ApartmentRental.Infrastracture.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastracture.Repository;

public class ImageRepository : IImageRepository
{
    private readonly MainContext _mainContext;

    public ImageRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Image>> GetAllAsync()
    {
        var images = await _mainContext.Image.ToListAsync();

        return images;
    }

    public async Task<Image> GetByIdAsync(int id)
    {
        var image = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == id);
        
        if (image != null)
        {
            return image;
        }
        
        throw new EntityNotFoundException();
    }

    public async Task AddAsync(Image entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Image entity)
    {
        var imageToUpdate = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == entity.Id);

        if (imageToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        imageToUpdate.Data = entity.Data;
        imageToUpdate.DateOfUpdate = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var imageToDelete = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == id);
        if (imageToDelete != null)
        {
            _mainContext.Image.Remove(imageToDelete);
            await _mainContext.SaveChangesAsync();
        }

        throw new EntityNotFoundException();
    }
}