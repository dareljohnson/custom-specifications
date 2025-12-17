namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents an outbound shipment from the warehouse.
/// </summary>
public record Shipment(
    string Id,
    string OrderId,
    string ClientId,
    DateTime ShipDate,
    string Carrier,
    string TrackingNumber,
    decimal Weight,
    ShipmentStatus Status,
    DateTime? DeliveryDate);

/// <summary>
/// Factory class for creating Shipment instances with validation.
/// </summary>
public static class ShipmentFactory
{
    public static Shipment Create(
        string id,
        string orderId,
        string clientId,
        DateTime shipDate,
        string carrier,
        string trackingNumber,
        decimal weight,
        ShipmentStatus status = ShipmentStatus.Created,
        DateTime? deliveryDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(orderId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(carrier);
        ArgumentException.ThrowIfNullOrWhiteSpace(trackingNumber);

        if (weight <= 0)
            throw new ArgumentException("Weight must be greater than zero.");

        if (deliveryDate.HasValue && deliveryDate.Value < shipDate)
            throw new ArgumentException("Delivery date cannot be before ship date.");

        return new Shipment(
            id,
            orderId,
            clientId,
            shipDate,
            carrier,
            trackingNumber,
            weight,
            status,
            deliveryDate);
    }
}

/// <summary>
/// Shipment processing status.
/// </summary>
public enum ShipmentStatus
{
    Created,
    PickedUp,
    InTransit,
    OutForDelivery,
    Delivered,
    Delayed,
    Exception,
    Returned
}
