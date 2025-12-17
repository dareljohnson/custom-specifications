namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents inventory stock for a product at a specific warehouse location.
/// </summary>
public record Inventory(
    string Id,
    string Sku,
    string ClientId,
    string LocationId,
    int Quantity,
    int ReorderPoint,
    int MaxQuantity,
    DateTime LastCountDate,
    InventoryStatus Status,
    DateTime? QuarantineUntil);

/// <summary>
/// Factory class for creating Inventory instances with validation.
/// </summary>
public static class InventoryFactory
{
    public static Inventory Create(
        string id,
        string sku,
        string clientId,
        string locationId,
        int quantity,
        int reorderPoint,
        int maxQuantity,
        DateTime lastCountDate,
        InventoryStatus status = InventoryStatus.Available,
        DateTime? quarantineUntil = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(locationId);

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        if (reorderPoint < 0)
            throw new ArgumentException("Reorder point cannot be negative.");

        if (maxQuantity < reorderPoint)
            throw new ArgumentException("Max quantity must be greater than or equal to reorder point.");

        return new Inventory(
            id,
            sku,
            clientId,
            locationId,
            quantity,
            reorderPoint,
            maxQuantity,
            lastCountDate,
            status,
            quarantineUntil);
    }
}

/// <summary>
/// Status of inventory items.
/// </summary>
public enum InventoryStatus
{
    Available,
    Reserved,
    Quarantine,
    Damaged,
    Expired,
    InTransit
}
