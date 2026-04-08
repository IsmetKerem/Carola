using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.EntityLayer.Entites;

namespace Carola.BusinessLayer.Concrete;

public class LocationManager: ILocationService
{
    private readonly ILocationDal _locationDal;

    public LocationManager(ILocationDal locationDal)
    {
        _locationDal = locationDal;
    }

    public async Task TInsertAsync(Location entity)
    {
        await _locationDal.InsertAsync(entity);
    }

    public async Task TUpdateAsync(Location entity)
    {
        await _locationDal.UpdateAsync(entity);
    }

    public async Task TDeleteAsync(int id)
    {
        await _locationDal.DeleteAsync(id);
    }

    public async Task<List<Location>> TGetAllAsync()
    {
        return await _locationDal.GetAllAsync();
    }

    public async Task<Location> TGetByIdAsync(int id)
    {
        return await _locationDal.GetByIdAsync(id);
    }
}