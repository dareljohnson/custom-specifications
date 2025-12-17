# Changelog

All notable changes to the CustomSpecifications library will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-01-09

### Added

#### Core Library
- **ISpecification<T>** interface defining the contract for all specifications
  - `IsSatisfiedBy(T candidate)` - Main evaluation method
  - `And(ISpecification<T> other)` - Logical AND composition
  - `Or(ISpecification<T> other)` - Logical OR composition
  - `Not()` - Logical NOT operation
  - `AndNot(ISpecification<T> other)` - Logical AND NOT composition
  - `OrNot(ISpecification<T> other)` - Logical OR NOT composition

- **Specification<T>** abstract base class
  - Default implementations of all composition methods
  - Template for creating concrete specifications

- **LinqSpecification<T>** abstract base class
  - Support for LINQ expression trees
  - Entity Framework query compatibility
  - `AsExpression()` method for obtaining expression trees

#### Composite Specifications
- **AndSpecification<T>** - Combines two specifications with logical AND
- **OrSpecification<T>** - Combines two specifications with logical OR
- **NotSpecification<T>** - Negates a specification
- **AndNotSpecification<T>** - Combines two specifications with logical AND NOT
- **OrNotSpecification<T>** - Combines two specifications with logical OR NOT

#### Extension Methods
- `Where<T>(IEnumerable<T>, ISpecification<T>)` - Filter collections
- `Any<T>(IEnumerable<T>, ISpecification<T>)` - Check if any element matches
- `All<T>(IEnumerable<T>, ISpecification<T>)` - Check if all elements match
- `Count<T>(IEnumerable<T>, ISpecification<T>)` - Count matching elements
- `First<T>(IEnumerable<T>, ISpecification<T>)` - Get first matching element
- `FirstOrDefault<T>(IEnumerable<T>, ISpecification<T>)` - Get first or default
- `Single<T>(IEnumerable<T>, ISpecification<T>)` - Get single matching element
- `SingleOrDefault<T>(IEnumerable<T>, ISpecification<T>)` - Get single or default

#### Domain Models (3PL Warehouse Management System)
- **Client** - Online retailer clients with tier levels (Standard, Premium, Enterprise)
- **Product** - Warehouse products with attributes (Hazmat, Fragile, Refrigerated, Expirable)
- **Inventory** - Stock levels with status tracking (Available, Reserved, Quarantine, Damaged, Expired)
- **Order** - Customer orders with priority levels and shipping methods
- **Shipment** - Outbound shipments with carrier tracking
- **Location** - Warehouse storage locations with special handling capabilities

#### Factory Classes
- ClientFactory - Create validated Client instances
- ProductFactory - Create validated Product instances
- InventoryFactory - Create validated Inventory instances
- OrderFactory - Create validated Order instances
- ShipmentFactory - Create validated Shipment instances
- LocationFactory - Create validated Location instances
- DimensionsFactory - Create validated Dimensions instances
- OrderLineFactory - Create validated OrderLine instances

#### Business Rule Specifications

**Client Specifications:**
- IsActiveSpecification - Active clients only
- IsTierSpecification - Filter by client tier
- HasExpiredContractSpecification - Expired contracts
- ContractExpiringSpecification - Contracts expiring soon
- IsPremiumOrEnterpriseSpecification - Premium/Enterprise tier clients
- HasLongTermContractSpecification - Long-term contracts (>1 year)

**Product Specifications:**
- BelongsToClientSpecification - Products for specific client
- IsHazmatSpecification - Hazardous materials
- IsFragileSpecification - Fragile items
- RequiresRefrigerationSpecification - Temperature-controlled items
- IsPerishableSpecification - Items with expiration dates
- IsExpiredSpecification - Expired products
- IsExpiringSpecification - Products expiring soon
- IsCategorySpecification - Filter by product category
- ExceedsWeightSpecification - Heavy items
- IsHighValueSpecification - High-value products
- RequiresSpecialHandlingSpecification - Special handling requirements
- IsOversizedSpecification - Oversized products by volume

**Inventory Specifications:**
- BelongsToClientSpecification - Inventory for specific client
- IsBelowReorderPointSpecification - Low stock alerts
- IsOutOfStockSpecification - Zero quantity items
- IsNearCapacitySpecification - Near maximum capacity
- HasStatusSpecification - Filter by inventory status
- IsInQuarantineSpecification - Quarantined inventory
- CanReleaseFromQuarantineSpecification - Ready for release
- NeedsCycleCountSpecification - Requires cycle count
- IsAvailableSpecification - Available inventory
- IsAtLocationSpecification - Filter by location
- RequiresImmediateAttentionSpecification - Critical inventory issues

**Order Specifications:**
- BelongsToClientSpecification - Orders for specific client
- HasPrioritySpecification - Filter by priority level
- IsUrgentSpecification - Rush/SameDay orders
- HasStatusSpecification - Filter by order status
- IsOverdueSpecification - Overdue orders
- IsDueSoonSpecification - Orders due within timeframe
- IsInternationalSpecification - International shipments
- HasShippingMethodSpecification - Filter by shipping method
- IsReadyToShipSpecification - Packed orders ready to ship
- IsCompletelyPickedSpecification - All items picked
- HasPartialPicksSpecification - Partially picked orders
- IsLargeOrderSpecification - Orders with many line items
- RequiresExpeditedProcessingSpecification - Needs immediate processing
- IsPlacedTodaySpecification - Orders placed today

**Shipment Specifications:**
- BelongsToClientSpecification - Shipments for specific client
- HasStatusSpecification - Filter by shipment status
- IsCarrierSpecification - Filter by carrier
- IsDelayedSpecification - Delayed shipments
- IsInTransitSpecification - Currently in transit
- IsDeliveredSpecification - Delivered shipments
- IsShippedInDateRangeSpecification - Filter by ship date range
- HasLongDeliveryTimeSpecification - Longer than expected delivery
- IsHeavyShipmentSpecification - Heavy shipments
- IsReturnedSpecification - Returned shipments
- IsShippedTodaySpecification - Shipped today
- HasDeliveryIssuesSpecification - Delivery problems

#### Examples

**Simple Examples (5):**
1. User Validation - Age and status checks
2. Email Validation - Multi-rule email verification
3. Number Range Validation - Numeric range checks
4. String Content Validation - Password strength
5. NOT Operator Examples - Negation usage

**Advanced WMS Examples (8):**
1. Low Stock Alerts for Premium Clients - Priority inventory notifications
2. Expedited Order Processing - Rush order identification
3. Special Handling Location Assignment - Hazmat/refrigerated product placement
4. Expiring Inventory Management - Multi-tier expiration tracking
5. Order Batching Logic - Multi-client batching restrictions
6. SLA Compliance Monitoring - Delivery performance tracking
7. Cycle Count Prioritization - Prioritized inventory audits
8. International Shipment Compliance - Cross-border compliance checks

#### Demo Program
- Interactive console application with menu system
- Run all examples or select specific examples
- Comprehensive pattern explanation
- User-friendly interface with clear output formatting

#### Documentation
- Comprehensive README with examples and best practices
- XML documentation comments on all public APIs
- Inline code examples
- Architecture diagrams
- Pattern explanation and use cases
- References to Domain-Driven Design literature

### Technical Details

**Target Framework:** .NET 8.0  
**Language Features:** C# 12 with latest language version  
**Design Patterns:** Specification, Composite, Factory  
**Principles:** SOLID, DRY, Immutability  
**Dependencies:** None (zero external dependencies)

### Quality Attributes

- **Type Safety:** Full generic type support
- **Null Safety:** Nullable reference types enabled
- **Immutability:** All specifications are immutable
- **Thread Safety:** Specifications are thread-safe
- **Performance:** Lazy evaluation with expression trees
- **Testability:** Each specification can be unit tested independently

### Use Cases

- Filtering collections based on complex business rules
- Building dynamic database queries
- Implementing authorization rules
- Creating reusable validation logic
- Modeling domain-specific business rules
- Supporting Domain-Driven Design architectures

### Real-World Application

The library includes a complete 3PL (Third-Party Logistics) Warehouse Management System
example featuring:
- Multiple online retailers (Tire Rack, Fenty Beauty, Newegg)
- Complex inventory management rules
- Multi-tier client service levels
- Special handling requirements (Hazmat, Refrigerated, Fragile)
- Expiration tracking for perishable goods
- Order priority and batching logic
- SLA compliance monitoring
- International shipping compliance

## [Unreleased]

### Future Enhancements

- Add async specification support for I/O-bound operations
- Implement specification visitor pattern for advanced scenarios
- Add specification serialization/deserialization
- Create fluent builder API for complex specification construction
- Add specification performance profiling tools
- Implement specification caching strategies
- Add specification composition validation
- Create specification visualization tools

---

**Note:** This is the initial release of the CustomSpecifications library. All features
are production-ready and fully tested. Future versions will maintain backward compatibility
following semantic versioning principles.
