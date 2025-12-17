using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;

namespace CustomSpecifications.Examples.WMS.Specifications;

/// <summary>
/// Specifications for Order entities.
/// </summary>
public static class OrderSpecifications
{
    /// <summary>
    /// Specification for orders belonging to a specific client.
    /// </summary>
    public class BelongsToClientSpecification : Specification<Order>
    {
        private readonly string _clientId;

        public BelongsToClientSpecification(string clientId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
            _clientId = clientId;
        }

        public override bool IsSatisfiedBy(Order candidate) => candidate.ClientId == _clientId;
    }

    /// <summary>
    /// Specification for orders with a specific priority.
    /// </summary>
    public class HasPrioritySpecification : Specification<Order>
    {
        private readonly OrderPriority _priority;

        public HasPrioritySpecification(OrderPriority priority) => _priority = priority;

        public override bool IsSatisfiedBy(Order candidate) => candidate.Priority == _priority;
    }

    /// <summary>
    /// Specification for rush or same-day orders.
    /// </summary>
    public class IsUrgentSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.Priority is OrderPriority.Rush or OrderPriority.SameDay;
    }

    /// <summary>
    /// Specification for orders with a specific status.
    /// </summary>
    public class HasStatusSpecification : Specification<Order>
    {
        private readonly OrderStatus _status;

        public HasStatusSpecification(OrderStatus status) => _status = status;

        public override bool IsSatisfiedBy(Order candidate) => candidate.Status == _status;
    }

    /// <summary>
    /// Specification for orders that are overdue.
    /// </summary>
    public class IsOverdueSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.RequiredDate < DateTime.UtcNow &&
            candidate.Status is not (OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled);
    }

    /// <summary>
    /// Specification for orders due within a specified timeframe.
    /// </summary>
    public class IsDueSoonSpecification : Specification<Order>
    {
        private readonly int _hoursUntilDue;

        public IsDueSoonSpecification(int hoursUntilDue = 24)
        {
            if (hoursUntilDue < 0)
                throw new ArgumentException("Hours until due must be non-negative.");

            _hoursUntilDue = hoursUntilDue;
        }

        public override bool IsSatisfiedBy(Order candidate)
        {
            var hoursRemaining = (candidate.RequiredDate - DateTime.UtcNow).TotalHours;
            return hoursRemaining > 0 && hoursRemaining <= _hoursUntilDue;
        }
    }

    /// <summary>
    /// Specification for international orders.
    /// </summary>
    public class IsInternationalSpecification : Specification<Order>
    {
        private readonly string _domesticCountry;

        public IsInternationalSpecification(string domesticCountry = "USA")
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(domesticCountry);
            _domesticCountry = domesticCountry;
        }

        public override bool IsSatisfiedBy(Order candidate) =>
            !candidate.DestinationCountry.Equals(_domesticCountry, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Specification for orders using a specific shipping method.
    /// </summary>
    public class HasShippingMethodSpecification : Specification<Order>
    {
        private readonly ShippingMethod _shippingMethod;

        public HasShippingMethodSpecification(ShippingMethod shippingMethod) =>
            _shippingMethod = shippingMethod;

        public override bool IsSatisfiedBy(Order candidate) => candidate.ShippingMethod == _shippingMethod;
    }

    /// <summary>
    /// Specification for orders ready to ship (picked and packed).
    /// </summary>
    public class IsReadyToShipSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.Status == OrderStatus.Packed;
    }

    /// <summary>
    /// Specification for orders that are complete (all items picked).
    /// </summary>
    public class IsCompletelyPickedSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.Lines.All(line => line.QuantityPicked >= line.QuantityOrdered);
    }

    /// <summary>
    /// Specification for orders with partial picks.
    /// </summary>
    public class HasPartialPicksSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.Lines.Any(line => line.QuantityPicked > 0 && line.QuantityPicked < line.QuantityOrdered);
    }

    /// <summary>
    /// Specification for orders exceeding a certain line item count.
    /// </summary>
    public class IsLargeOrderSpecification : Specification<Order>
    {
        private readonly int _lineItemThreshold;

        public IsLargeOrderSpecification(int lineItemThreshold = 10)
        {
            if (lineItemThreshold <= 0)
                throw new ArgumentException("Line item threshold must be positive.");

            _lineItemThreshold = lineItemThreshold;
        }

        public override bool IsSatisfiedBy(Order candidate) => candidate.Lines.Count > _lineItemThreshold;
    }

    /// <summary>
    /// Specification for orders requiring expedited processing.
    /// </summary>
    public class RequiresExpeditedProcessingSpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate)
        {
            var hoursUntilDue = (candidate.RequiredDate - DateTime.UtcNow).TotalHours;
            return candidate.Priority is OrderPriority.Rush or OrderPriority.SameDay ||
                   candidate.ShippingMethod is ShippingMethod.Overnight or ShippingMethod.TwoDayAir ||
                   hoursUntilDue < 8;
        }
    }

    /// <summary>
    /// Specification for orders placed today.
    /// </summary>
    public class IsPlacedTodaySpecification : Specification<Order>
    {
        public override bool IsSatisfiedBy(Order candidate) =>
            candidate.OrderDate.Date == DateTime.UtcNow.Date;
    }
}
