
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationHouseExtension
{
    public static void MapHouseEndpoints(this WebApplication app)
    {
        //Get All houses.
        app.MapGet("/houses", (IHouseRepository repo) =>
            repo.GetAllAsync())
            .Produces<IEnumerable<HouseDto>>(StatusCodes.Status200OK);

        //Get house by id.
        app.MapGet("/house/{houseId:int}", async (int houseId, IHouseRepository repo) => 
        {
            var house = await repo.Get(houseId);

            if(house != null) return Results.Ok(house);

            return Results.Problem($"House with Id: {houseId} not found.",
                statusCode: StatusCodes.Status404NotFound);
        })
        .Produces<HouseDetailDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        //Create a new house
        app.MapPost("/houses", async ([FromBody] HouseDetailDto dto, 
            IHouseRepository repo) => 
        {
            if(!MiniValidator.TryValidate(dto, out var errors))
                return Results.ValidationProblem(errors);

            HouseDetailDto newHouse = await repo.Add(dto);
            return Results.Created($"house/{newHouse.Id}", newHouse);
        })
        .Produces<HouseDetailDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        //Update a house
        app.MapPut("/houses", async ([FromBody] HouseDetailDto dto,
            IHouseRepository repo) => 
        {
            if(!MiniValidator.TryValidate(dto, out var errors))
                return Results.ValidationProblem(errors);

            if(await repo.Get(dto.Id) == null) return
                Results.Problem($"House {dto.Id} not found", statusCode: StatusCodes.Status404NotFound);
            
            var updatedHouse = await repo.Update(dto);
            return Results.Ok(updatedHouse);
        })
        .ProducesProblem(StatusCodes.Status404NotFound)
        .Produces<HouseDetailDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem();

        //Delete a house
        app.MapDelete("/house/{houseId}", async (int houseId, IHouseRepository repo) =>  
        {
            if(await repo.Get(houseId) == null) return
                Results.Problem($"House {houseId} not found", statusCode: StatusCodes.Status404NotFound);

            await repo.Delete(houseId);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK);
    }
}