using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;

namespace CustomSpecifications.Examples.WMS.Specifications;

/// <summary>
/// Specifications for Shipment entities.
/// </summary>
public static class ShipmentSpecifications
{
    /// <summary>
    /// Specification for shipments belonging to a specific client.
    /// </summary>
    public class BelongsToClientSpecification : Specification<Shipment>
    {
        private readonly string _clientId;

        public BelongsToClientSpecification(string clientId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
            _clientId = clientId;
        }

        public override bool IsSatisfiedBy(Shipment candidate) => candidate.ClientId == _clientId;
    }

    /// <summary>
    /// Specification for shipments with a specific status.
    /// </summary>
    public class HasStatusSpecification : Specification<Shipment>
    {
        private readonly ShipmentStatus _status;

        public HasStatusSpecification(ShipmentStatus status) => _status = status;

        public override bool IsSatisfiedBy(Shipment candidate) => candidate.Status == _status;
    }

    /// <summary>
    /// Specification for shipments using a specific carrier.
    /// </summary>
    public class IsCarrierSpecification : Specification<Shipment>
    {
        private readonly string _carrier;

        public IsCarrierSpecification(string carrier)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(carrier);
            _carrier = carrier;
        }

        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Carrier.Equals(_carrier, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Specification for shipments that are delayed.
    /// </summary>
    public class IsDelayedSpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Status is ShipmentStatus.Delayed or ShipmentStatus.Exception;
    }

    /// <summary>
    /// Specification for shipments currently in transit.
    /// </summary>
    public class IsInTransitSpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Status is ShipmentStatus.InTransit or ShipmentStatus.OutForDelivery;
    }

    /// <summary>
    /// Specification for shipments that have been delivered.
    /// </summary>
    public class IsDeliveredSpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Status == ShipmentStatus.Delivered && candidate.DeliveryDate.HasValue;
    }

    /// <summary>
    /// Specification for shipments shipped within a date range.
    /// </summary>
    public class IsShippedInDateRangeSpecification : Specification<Shipment>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public IsShippedInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date must be on or after start date.");

            _startDate = startDate;
            _endDate = endDate;
        }

        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.ShipDate >= _startDate && candidate.ShipDate <= _endDate;
    }

    /// <summary>
    /// Specification for shipments that took longer than expected to deliver.
    /// </summary>
    public class HasLongDeliveryTimeSpecification : Specification<Shipment>
    {
        private readonly int _expectedDays;

        public HasLongDeliveryTimeSpecification(int expectedDays = 7)
        {
            if (expectedDays <= 0)
                throw new ArgumentException("Expected days must be positive.");

            _expectedDays = expectedDays;
        }

        public override bool IsSatisfiedBy(Shipment candidate)
        {
            if (!candidate.DeliveryDate.HasValue)
                return false;

            var actualDays = (candidate.DeliveryDate.Value - candidate.ShipDate).Days;
            return actualDays > _expectedDays;
        }
    }

    /// <summary>
    /// Specification for heavy shipments exceeding a weight threshold.
    /// </summary>
    public class IsHeavyShipmentSpecification : Specification<Shipment>
    {
        private readonly decimal _weightThreshold;

        public IsHeavyShipmentSpecification(decimal weightThreshold = 150)
        {
            if (weightThreshold <= 0)
                throw new ArgumentException("Weight threshold must be positive.");

            _weightThreshold = weightThreshold;
        }

        public override bool IsSatisfiedBy(Shipment candidate) => candidate.Weight > _weightThreshold;
    }

    /// <summary>
    /// Specification for shipments that have been returned.
    /// </summary>
    public class IsReturnedSpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Status == ShipmentStatus.Returned;
    }

    /// <summary>
    /// Specification for shipments shipped today.
    /// </summary>
    public class IsShippedTodaySpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.ShipDate.Date == DateTime.UtcNow.Date;
    }

    /// <summary>
    /// Specification for shipments with delivery issues (delayed, exception, or returned).
    /// </summary>
    public class HasDeliveryIssuesSpecification : Specification<Shipment>
    {
        public override bool IsSatisfiedBy(Shipment candidate) =>
            candidate.Status is ShipmentStatus.Delayed or ShipmentStatus.Exception or ShipmentStatus.Returned;
    }
}
