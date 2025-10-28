using HerokuTest.Entities;

namespace HerokuTest.Services;

public interface ITrackingService
{
    Task<Tracking?> GetTrackingByCode(string code);
    Task SetTrackingRange(string[] trackingIds, DateTime receivedDate);
    Task SetReceivedTrackingRange(string[] trackingIds, DateTime receivedDate);
}