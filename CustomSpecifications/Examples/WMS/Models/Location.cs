namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents a warehouse storage location.
/// </summary>
public record Location(
    string Id,
    string Zone,
    string Aisle,
    string Bay,
    string Level,
    LocationType Type,
    bool IsTemperatureControlled,
    decimal MaxWeight,
    bool IsHazmatApproved);

/// <summary>
/// Factory class for creating Location instances with validation.
/// </summary>
public static class LocationFactory
{
    public static Location Create(
        string id,
        string zone,
        string aisle,
        string bay,
        string level,
        LocationType type,
        bool isTemperatureControlled = false,
        decimal maxWeight = 5000,
        bool isHazmatApproved = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(zone);
        ArgumentException.ThrowIfNullOrWhiteSpace(aisle);
        ArgumentException.ThrowIfNullOrWhiteSpace(bay);
        ArgumentException.ThrowIfNullOrWhiteSpace(level);

        if (maxWeight <= 0)
            throw new ArgumentException("Max weight must be greater than zero.");

        return new Location(
            id,
            zone,
            aisle,
            bay,
            level,
            type,
            isTemperatureControlled,
            maxWeight,
            isHazmatApproved);
    }
}

/// <summary>
/// Types of warehouse locations.
/// </summary>
public enum LocationType
{
    Receiving,
    Storage,
    Picking,
    Packing,
    Shipping,
    Quarantine,
    Returns
}
