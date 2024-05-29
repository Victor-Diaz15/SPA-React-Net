
using Microsoft.EntityFrameworkCore;

public interface IBidRepository 
{
    Task<IEnumerable<BidDto>> GetAllAsync(int houseId);
    Task<BidDto> Add(BidDto dto);
}

public class BidRepository : IBidRepository
{
    private readonly MainDbContext _db;

    public BidRepository(MainDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<BidDto>> GetAllAsync(int houseId)
    {
        return await _db.Bids.Where(x => x.HouseId == houseId)
            .Select(x => new BidDto(x.Id, x.HouseId, x.Bidder, x.Amount))
            .ToListAsync();
    }

    public async Task<BidDto> Add(BidDto dto)
    {
        var entity = new Bid(){
            HouseId = dto.HouseId,
            Bidder = dto.Bidder,
            Amount = dto.Amount
        };

        _db.Bids.Add(entity);
        await _db.SaveChangesAsync();
        return new BidDto(entity.Id, entity.HouseId, entity.Bidder, entity.Amount);
    }

    
}