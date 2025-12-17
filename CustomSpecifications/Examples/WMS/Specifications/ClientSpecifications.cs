using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;

namespace CustomSpecifications.Examples.WMS.Specifications;

/// <summary>
/// Specifications for Client entities.
/// </summary>
public static class ClientSpecifications
{
    /// <summary>
    /// Specification for active clients.
    /// </summary>
    public class IsActiveSpecification : Specification<Client>
    {
        public override bool IsSatisfiedBy(Client candidate) => candidate.IsActive;
    }

    /// <summary>
    /// Specification for clients of a specific tier.
    /// </summary>
    public class IsTierSpecification : Specification<Client>
    {
        private readonly ClientTier _tier;

        public IsTierSpecification(ClientTier tier) => _tier = tier;

        public override bool IsSatisfiedBy(Client candidate) => candidate.Tier == _tier;
    }

    /// <summary>
    /// Specification for clients whose contract has expired.
    /// </summary>
    public class HasExpiredContractSpecification : Specification<Client>
    {
        public override bool IsSatisfiedBy(Client candidate) =>
            candidate.ContractEndDate.HasValue && candidate.ContractEndDate.Value < DateTime.UtcNow;
    }

    /// <summary>
    /// Specification for clients whose contract is expiring soon.
    /// </summary>
    public class ContractExpiringSpecification : Specification<Client>
    {
        private readonly int _daysUntilExpiration;

        public ContractExpiringSpecification(int daysUntilExpiration = 30)
        {
            if (daysUntilExpiration < 0)
                throw new ArgumentException("Days until expiration must be non-negative.");

            _daysUntilExpiration = daysUntilExpiration;
        }

        public override bool IsSatisfiedBy(Client candidate)
        {
            if (!candidate.ContractEndDate.HasValue)
                return false;

            var daysRemaining = (candidate.ContractEndDate.Value - DateTime.UtcNow).Days;
            return daysRemaining >= 0 && daysRemaining <= _daysUntilExpiration;
        }
    }

    /// <summary>
    /// Specification for premium or enterprise tier clients.
    /// </summary>
    public class IsPremiumOrEnterpriseSpecification : Specification<Client>
    {
        public override bool IsSatisfiedBy(Client candidate) =>
            candidate.Tier is ClientTier.Premium or ClientTier.Enterprise;
    }

    /// <summary>
    /// Specification for clients with long-term contracts (more than 1 year).
    /// </summary>
    public class HasLongTermContractSpecification : Specification<Client>
    {
        public override bool IsSatisfiedBy(Client candidate)
        {
            if (!candidate.ContractEndDate.HasValue)
                return false;

            var contractDuration = candidate.ContractEndDate.Value - candidate.ContractStartDate;
            return contractDuration.TotalDays >= 365;
        }
    }
}
