public class House 
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? Photo { get; set; }
    public List<Bid> Bids { get; set; } = [];
}