namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents a customer order to be fulfilled from the warehouse.
/// </summary>
public record Order(
    string Id,
    string ClientId,
    DateTime OrderDate,
    DateTime RequiredDate,
    OrderPriority Priority,
    ShippingMethod ShippingMethod,
    string DestinationCountry,
    OrderStatus Status,
    IReadOnlyList<OrderLine> Lines);

/// <summary>
/// Factory class for creating Order instances with validation.
/// </summary>
public static class OrderFactory
{
    public static Order Create(
        string id,
        string clientId,
        DateTime orderDate,
        DateTime requiredDate,
        OrderPriority priority,
        ShippingMethod shippingMethod,
        string destinationCountry,
        OrderStatus status,
        IReadOnlyList<OrderLine> lines)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationCountry);

        if (lines == null || lines.Count == 0)
            throw new ArgumentException("Order must have at least one line item.");

        if (requiredDate < orderDate)
            throw new ArgumentException("Required date must be on or after order date.");

        return new Order(
            id,
            clientId,
            orderDate,
            requiredDate,
            priority,
            shippingMethod,
            destinationCountry,
            status,
            lines);
    }
}

/// <summary>
/// Represents a line item in an order.
/// </summary>
public record OrderLine(string Sku, int QuantityOrdered, int QuantityPicked);

/// <summary>
/// Factory for creating OrderLine instances with validation.
/// </summary>
public static class OrderLineFactory
{
    public static OrderLine Create(string sku, int quantityOrdered, int quantityPicked = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        if (quantityOrdered <= 0)
            throw new ArgumentException("Quantity ordered must be greater than zero.");

        if (quantityPicked < 0)
            throw new ArgumentException("Quantity picked cannot be negative.");

        if (quantityPicked > quantityOrdered)
            throw new ArgumentException("Quantity picked cannot exceed quantity ordered.");

        return new OrderLine(sku, quantityOrdered, quantityPicked);
    }
}

/// <summary>
/// Order priority levels.
/// </summary>
public enum OrderPriority
{
    Low,
    Normal,
    High,
    Rush,
    SameDay
}

/// <summary>
/// Shipping methods available.
/// </summary>
public enum ShippingMethod
{
    Ground,
    TwoDayAir,
    Overnight,
    International,
    Freight
}

/// <summary>
/// Order processing status.
/// </summary>
public enum OrderStatus
{
    Pending,
    InProgress,
    Picked,
    Packed,
    Shipped,
    Delivered,
    Cancelled,
    OnHold
}
