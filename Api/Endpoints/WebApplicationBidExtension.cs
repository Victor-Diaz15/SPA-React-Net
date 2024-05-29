
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationBidExtension
{
    public static void MapBidEndpoints(this WebApplication app)
    {
        //Get all bids
        app.MapGet("house/{houseId:int}/bids", async (int houseId,
            IHouseRepository houseRepo, IBidRepository bidRepo) => 
        {
            if(await houseRepo.Get(houseId) == null)
                return Results.Problem($"House {houseId} not found",
                statusCode: StatusCodes.Status404NotFound);

            var bids = await bidRepo.GetAllAsync(houseId);
            return Results.Ok(bids);
        })
        .ProducesProblem(StatusCodes.Status404NotFound)
        .Produces<IEnumerable<BidDto>>(StatusCodes.Status200OK);

        //Add a new bid
        app.MapPost("house/{houseId:int}/bids", async (int houseId, [FromBody] BidDto dto,
            IHouseRepository houseRepo, IBidRepository bidRepo) => 
        {
            if(dto.HouseId != houseId)
                return Results.Problem("No match", statusCode: StatusCodes.Status400BadRequest);
            
            if(!MiniValidator.TryValidate(dto, out var errors))
                return Results.ValidationProblem(errors);

            var newBid = await bidRepo.Add(dto);
            return Results.Created($"/houses/{newBid.HouseId}/bids", newBid);
        })
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesValidationProblem()
        .Produces<BidDto>(StatusCodes.Status201Created);
    }
}