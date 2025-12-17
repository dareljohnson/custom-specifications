namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents a product stored in the warehouse for a specific client.
/// </summary>
public record Product(
    string Sku,
    string ClientId,
    string Name,
    string Description,
    ProductCategory Category,
    decimal Weight,
    Dimensions Dimensions,
    bool IsFragile,
    bool IsHazmat,
    bool RequiresRefrigeration,
    decimal UnitCost,
    DateTime? ExpirationDate);

/// <summary>
/// Factory class for creating Product instances with validation.
/// </summary>
public static class ProductFactory
{
    public static Product Create(
        string sku,
        string clientId,
        string name,
        string description,
        ProductCategory category,
        decimal weight,
        Dimensions dimensions,
        bool isFragile = false,
        bool isHazmat = false,
        bool requiresRefrigeration = false,
        decimal unitCost = 0,
        DateTime? expirationDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (weight <= 0)
            throw new ArgumentException("Weight must be greater than zero.");

        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative.");

        return new Product(
            sku,
            clientId,
            name,
            description,
            category,
            weight,
            dimensions,
            isFragile,
            isHazmat,
            requiresRefrigeration,
            unitCost,
            expirationDate);
    }
}

/// <summary>
/// Product dimensions in inches.
/// </summary>
public record Dimensions(decimal Length, decimal Width, decimal Height)
{
    public decimal Volume => Length * Width * Height;
}

/// <summary>
/// Factory for creating Dimensions instances with validation.
/// </summary>
public static class DimensionsFactory
{
    public static Dimensions Create(decimal length, decimal width, decimal height)
    {
        if (length <= 0 || width <= 0 || height <= 0)
            throw new ArgumentException("All dimensions must be greater than zero.");

        return new Dimensions(length, width, height);
    }
}

/// <summary>
/// Product category classifications.
/// </summary>
public enum ProductCategory
{
    Automotive,
    Beauty,
    Electronics,
    Food,
    Apparel,
    Industrial,
    General
}
