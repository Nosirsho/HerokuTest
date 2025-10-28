using HerokuTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace HerokuTest.Services;

public class TrackingService: ITrackingService
{
    private readonly DataContext _context;

    public TrackingService(DataContext context)
    {
        _context = context;
    }

    public async Task<Tracking?> GetTrackingByCode(string code)
    {
        var tracking = await _context.Trackings.FirstOrDefaultAsync(t=>t.Code == code);
        return tracking;
    }

    public async Task SetTrackingRange(string[] trackingIds, DateTime receivedDate)
    {
        var trackingsList = new List<Tracking>();
        foreach (var trackingId in trackingIds)
        {
            trackingsList.Add(new Tracking()
            {
                Code = trackingId,
                ReceivedDate = receivedDate.Date.ToUniversalTime()
            });
        }
        await _context.Trackings.AddRangeAsync(trackingsList);
        await _context.SaveChangesAsync();
    }

    public async Task SetReceivedTrackingRange(string[] trackingIds, DateTime receivedDate)
    {
        foreach (var trackingId in trackingIds)
        {
            var tracking = await _context.Trackings.FirstOrDefaultAsync(t => t.Code == trackingId && !t.GoodsReceived);
            if (tracking != null)
            {
                tracking.GoodsReceived = true;
                DateTime utcReceivedDate = DateTime.SpecifyKind(receivedDate, DateTimeKind.Local).ToUniversalTime();
                tracking.GoodReceivedDate = utcReceivedDate;
            }
        }
        await _context.SaveChangesAsync();
    }
}