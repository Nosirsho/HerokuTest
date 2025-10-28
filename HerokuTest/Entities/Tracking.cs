namespace HerokuTest.Entities;

public class Tracking : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public AppUser? AppUser { get; set; }
    public long? AppUserId { get; set; }
    public bool GoodsReceived { get; set; } = false;
    public DateTime GoodReceivedDate { get; set; }
}