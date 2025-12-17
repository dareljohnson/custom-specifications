using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;

namespace CustomSpecifications.Examples.WMS.Specifications;

/// <summary>
/// Specifications for Product entities.
/// </summary>
public static class ProductSpecifications
{
    /// <summary>
    /// Specification for products belonging to a specific client.
    /// </summary>
    public class BelongsToClientSpecification : Specification<Product>
    {
        private readonly string _clientId;

        public BelongsToClientSpecification(string clientId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
            _clientId = clientId;
        }

        public override bool IsSatisfiedBy(Product candidate) => candidate.ClientId == _clientId;
    }

    /// <summary>
    /// Specification for hazardous materials.
    /// </summary>
    public class IsHazmatSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) => candidate.IsHazmat;
    }

    /// <summary>
    /// Specification for fragile products.
    /// </summary>
    public class IsFragileSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) => candidate.IsFragile;
    }

    /// <summary>
    /// Specification for products requiring refrigeration.
    /// </summary>
    public class RequiresRefrigerationSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) => candidate.RequiresRefrigeration;
    }

    /// <summary>
    /// Specification for products with expiration dates.
    /// </summary>
    public class IsPerishableSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) => candidate.ExpirationDate.HasValue;
    }

    /// <summary>
    /// Specification for products that have expired.
    /// </summary>
    public class IsExpiredSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) =>
            candidate.ExpirationDate.HasValue && candidate.ExpirationDate.Value < DateTime.UtcNow;
    }

    /// <summary>
    /// Specification for products expiring within a specified number of days.
    /// </summary>
    public class IsExpiringSpecification : Specification<Product>
    {
        private readonly int _daysUntilExpiration;

        public IsExpiringSpecification(int daysUntilExpiration = 30)
        {
            if (daysUntilExpiration < 0)
                throw new ArgumentException("Days until expiration must be non-negative.");

            _daysUntilExpiration = daysUntilExpiration;
        }

        public override bool IsSatisfiedBy(Product candidate)
        {
            if (!candidate.ExpirationDate.HasValue)
                return false;

            var daysRemaining = (candidate.ExpirationDate.Value - DateTime.UtcNow).Days;
            return daysRemaining >= 0 && daysRemaining <= _daysUntilExpiration;
        }
    }

    /// <summary>
    /// Specification for products in a specific category.
    /// </summary>
    public class IsCategorySpecification : Specification<Product>
    {
        private readonly ProductCategory _category;

        public IsCategorySpecification(ProductCategory category) => _category = category;

        public override bool IsSatisfiedBy(Product candidate) => candidate.Category == _category;
    }

    /// <summary>
    /// Specification for products exceeding a weight threshold.
    /// </summary>
    public class ExceedsWeightSpecification : Specification<Product>
    {
        private readonly decimal _weightThreshold;

        public ExceedsWeightSpecification(decimal weightThreshold)
        {
            if (weightThreshold < 0)
                throw new ArgumentException("Weight threshold must be non-negative.");

            _weightThreshold = weightThreshold;
        }

        public override bool IsSatisfiedBy(Product candidate) => candidate.Weight > _weightThreshold;
    }

    /// <summary>
    /// Specification for high-value products.
    /// </summary>
    public class IsHighValueSpecification : Specification<Product>
    {
        private readonly decimal _valueThreshold;

        public IsHighValueSpecification(decimal valueThreshold = 1000)
        {
            if (valueThreshold < 0)
                throw new ArgumentException("Value threshold must be non-negative.");

            _valueThreshold = valueThreshold;
        }

        public override bool IsSatisfiedBy(Product candidate) => candidate.UnitCost > _valueThreshold;
    }

    /// <summary>
    /// Specification for products requiring special handling (hazmat, fragile, or refrigerated).
    /// </summary>
    public class RequiresSpecialHandlingSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product candidate) =>
            candidate.IsHazmat || candidate.IsFragile || candidate.RequiresRefrigeration;
    }

    /// <summary>
    /// Specification for oversized products based on volume.
    /// </summary>
    public class IsOversizedSpecification : Specification<Product>
    {
        private readonly decimal _volumeThreshold;

        public IsOversizedSpecification(decimal volumeThreshold = 10000)
        {
            if (volumeThreshold < 0)
                throw new ArgumentException("Volume threshold must be non-negative.");

            _volumeThreshold = volumeThreshold;
        }

        public override bool IsSatisfiedBy(Product candidate) =>
            candidate.Dimensions.Volume > _volumeThreshold;
    }
}
