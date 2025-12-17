namespace CustomSpecifications.Examples.WMS.Models;

/// <summary>
/// Represents an online retailer client using the 3PL logistics services.
/// </summary>
public record Client(
    string Id,
    string Name,
    string ContactEmail,
    ClientTier Tier,
    DateTime ContractStartDate,
    DateTime? ContractEndDate,
    bool IsActive);

/// <summary>
/// Factory class for creating Client instances with validation.
/// </summary>
public static class ClientFactory
{
    public static Client Create(
        string id,
        string name,
        string contactEmail,
        ClientTier tier,
        DateTime contractStartDate,
        DateTime? contractEndDate = null,
        bool isActive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(contactEmail);

        if (contractEndDate.HasValue && contractEndDate.Value <= contractStartDate)
            throw new ArgumentException("Contract end date must be after start date.");

        return new Client(id, name, contactEmail, tier, contractStartDate, contractEndDate, isActive);
    }
}

/// <summary>
/// Service tier levels for clients.
/// </summary>
public enum ClientTier
{
    Standard,
    Premium,
    Enterprise
}
