using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;

namespace CustomSpecifications.Examples.WMS.Specifications;

/// <summary>
/// Specifications for Inventory entities.
/// </summary>
public static class InventorySpecifications
{
    /// <summary>
    /// Specification for inventory belonging to a specific client.
    /// </summary>
    public class BelongsToClientSpecification : Specification<Inventory>
    {
        private readonly string _clientId;

        public BelongsToClientSpecification(string clientId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
            _clientId = clientId;
        }

        public override bool IsSatisfiedBy(Inventory candidate) => candidate.ClientId == _clientId;
    }

    /// <summary>
    /// Specification for inventory below reorder point.
    /// </summary>
    public class IsBelowReorderPointSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) =>
            candidate.Quantity <= candidate.ReorderPoint && candidate.Status == InventoryStatus.Available;
    }

    /// <summary>
    /// Specification for inventory with zero quantity.
    /// </summary>
    public class IsOutOfStockSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) => candidate.Quantity == 0;
    }

    /// <summary>
    /// Specification for inventory at or near capacity.
    /// </summary>
    public class IsNearCapacitySpecification : Specification<Inventory>
    {
        private readonly decimal _thresholdPercentage;

        public IsNearCapacitySpecification(decimal thresholdPercentage = 0.9m)
        {
            if (thresholdPercentage is < 0 or > 1)
                throw new ArgumentException("Threshold percentage must be between 0 and 1.");

            _thresholdPercentage = thresholdPercentage;
        }

        public override bool IsSatisfiedBy(Inventory candidate)
        {
            if (candidate.MaxQuantity == 0)
                return false;

            var utilizationRate = (decimal)candidate.Quantity / candidate.MaxQuantity;
            return utilizationRate >= _thresholdPercentage;
        }
    }

    /// <summary>
    /// Specification for inventory with a specific status.
    /// </summary>
    public class HasStatusSpecification : Specification<Inventory>
    {
        private readonly InventoryStatus _status;

        public HasStatusSpecification(InventoryStatus status) => _status = status;

        public override bool IsSatisfiedBy(Inventory candidate) => candidate.Status == _status;
    }

    /// <summary>
    /// Specification for inventory in quarantine.
    /// </summary>
    public class IsInQuarantineSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) =>
            candidate.Status == InventoryStatus.Quarantine &&
            candidate.QuarantineUntil.HasValue &&
            candidate.QuarantineUntil.Value > DateTime.UtcNow;
    }

    /// <summary>
    /// Specification for inventory that can be released from quarantine.
    /// </summary>
    public class CanReleaseFromQuarantineSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) =>
            candidate.Status == InventoryStatus.Quarantine &&
            candidate.QuarantineUntil.HasValue &&
            candidate.QuarantineUntil.Value <= DateTime.UtcNow;
    }

    /// <summary>
    /// Specification for inventory that hasn't been counted recently.
    /// </summary>
    public class NeedsCycleCountSpecification : Specification<Inventory>
    {
        private readonly int _daysSinceLastCount;

        public NeedsCycleCountSpecification(int daysSinceLastCount = 30)
        {
            if (daysSinceLastCount < 0)
                throw new ArgumentException("Days since last count must be non-negative.");

            _daysSinceLastCount = daysSinceLastCount;
        }

        public override bool IsSatisfiedBy(Inventory candidate)
        {
            var daysSinceCount = (DateTime.UtcNow - candidate.LastCountDate).Days;
            return daysSinceCount > _daysSinceLastCount;
        }
    }

    /// <summary>
    /// Specification for available inventory (not reserved, quarantined, or damaged).
    /// </summary>
    public class IsAvailableSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) =>
            candidate.Status == InventoryStatus.Available && candidate.Quantity > 0;
    }

    /// <summary>
    /// Specification for inventory at a specific location.
    /// </summary>
    public class IsAtLocationSpecification : Specification<Inventory>
    {
        private readonly string _locationId;

        public IsAtLocationSpecification(string locationId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(locationId);
            _locationId = locationId;
        }

        public override bool IsSatisfiedBy(Inventory candidate) => candidate.LocationId == _locationId;
    }

    /// <summary>
    /// Specification for inventory requiring immediate attention (damaged, expired, or critically low).
    /// </summary>
    public class RequiresImmediateAttentionSpecification : Specification<Inventory>
    {
        public override bool IsSatisfiedBy(Inventory candidate) =>
            candidate.Status is InventoryStatus.Damaged or InventoryStatus.Expired ||
            (candidate.Quantity == 0 && candidate.Status == InventoryStatus.Available);
    }
}
