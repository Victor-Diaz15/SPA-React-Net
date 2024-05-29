
using Microsoft.EntityFrameworkCore;

public interface IHouseRepository {
    Task<IEnumerable<HouseDto>> GetAllAsync();
    Task<HouseDetailDto?> Get(int id);
    Task<HouseDetailDto> Add(HouseDetailDto dto);
    Task<HouseDetailDto> Update(HouseDetailDto dto);
    Task Delete(int id);
}

public class HouseRepository : IHouseRepository
{
    private readonly MainDbContext _dbContext;

    public HouseRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<HouseDto>> GetAllAsync()
    {
        return await _dbContext.Houses
            .Select(x => new HouseDto(x.Id, x.Address, x.Country, x.Price))
            .ToListAsync();
    }

    public async Task<HouseDetailDto?> Get(int id)
    {
        var house = await _dbContext.Houses
            .FindAsync(id);
        
        if(house != null) return EntityToDto(house);
        
        return null;
    }

    public async Task<HouseDetailDto> Add(HouseDetailDto dto)
    {
        var entity = new House();
        DtoToEntity(dto, entity);
        _dbContext.Houses.Add(entity);
        await _dbContext.SaveChangesAsync();
        return EntityToDto(entity);
    }

    public async Task<HouseDetailDto> Update(HouseDetailDto dto)
    {
        var entity = await _dbContext.Houses.FindAsync(dto.Id) ?? throw new ArgumentException($"Error updating house {dto.Id}.");
        DtoToEntity(dto, entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return EntityToDto(entity);
    }

    public async Task Delete(int id)
    {
        var entity = await _dbContext.Houses.FindAsync(id) ?? throw new ArgumentException($"Error updating house {id}.");
        _dbContext.Houses.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }


    //Private functions for mapping

    private static void DtoToEntity(HouseDetailDto dto, House entity)
    {
        entity.Address = dto.Address;
        entity.Country = dto.Country;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.Photo = dto.Photo;
    }

    private static HouseDetailDto EntityToDto(House entity)
    {
        return new HouseDetailDto(entity.Id, entity.Address, entity.Country, entity.Price, 
                entity.Description, entity.Photo);
    }
}