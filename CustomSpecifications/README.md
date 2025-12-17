# CustomSpecifications Library

A comprehensive C# implementation of the **Specification Pattern** for .NET 8, enabling elegant composition of business rules through Boolean logic. This library provides a clean, type-safe way to encapsulate and combine complex business logic.

## ✨ Features

- **Type-Safe Specifications**: Generic specification interface `ISpecification<T>`
- **Fluent API**: Chain specifications using `And`, `Or`, `Not`, `AndNot`, `OrNot`
- **LINQ Integration**: Use specifications with LINQ queries and collections
- **Expression Tree Support**: `LinqSpecification<T>` for Entity Framework compatibility
- **Composable**: Build complex rules from simple specifications
- **Immutable**: Thread-safe specification objects
- **Zero Dependencies**: Pure .NET 8 implementation

## 📦 Installation

```bash
# Clone or download the project
git clone https://github.com/yourrepo/CustomSpecifications.git
```

## 🚀 Quick Start

### Basic Usage

```csharp
using CustomSpecifications.Core;
using CustomSpecifications.Extensions;

// Define a simple specification
public class IsAdultSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => candidate.Age >= 18;
}

// Use it
var isAdult = new IsAdultSpecification();
var isActive = new IsActiveUserSpecification();

// Combine specifications
var activeAdults = users.Where(isAdult.And(isActive));
```

### Combining Specifications

```csharp
// AND operation
var spec1 = new IsAdultSpecification();
var spec2 = new IsActiveSpecification();
var combined = spec1.And(spec2);

// OR operation
var eitherSpec = spec1.Or(spec2);

// NOT operation
var notAdult = spec1.Not();

// Complex combinations
var complexSpec = spec1.And(spec2).Or(spec3).AndNot(spec4);
```

## 📚 Core Concepts

### ISpecification<T> Interface

The foundation of the pattern:

```csharp
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T candidate);
    ISpecification<T> And(ISpecification<T> other);
    ISpecification<T> AndNot(ISpecification<T> other);
    ISpecification<T> Or(ISpecification<T> other);
    ISpecification<T> OrNot(ISpecification<T> other);
    ISpecification<T> Not();
}
```

### Specification<T> Base Class

Abstract base class providing default implementations:

```csharp
public abstract class Specification<T> : ISpecification<T>
{
    public abstract bool IsSatisfiedBy(T candidate);
    // Composition methods provided automatically
}
```

### LinqSpecification<T>

For database queries and expression trees:

```csharp
public abstract class LinqSpecification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> AsExpression();
    public bool IsSatisfiedBy(T candidate) => AsExpression().Compile()(candidate);
}
```

## 🔧 Extension Methods

The library provides LINQ-style extension methods:

```csharp
// Filter collections
var filtered = collection.Where(specification);

// Check existence
var hasAny = collection.Any(specification);
var allMatch = collection.All(specification);

// Count matches
var count = collection.Count(specification);

// Find elements
var first = collection.First(specification);
var single = collection.Single(specification);
```

## 🏭 Real-World Example: 3PL Warehouse Management System

This library includes comprehensive examples modeling a Third-Party Logistics (3PL) warehouse management system for clients like Tire Rack, Fenty Beauty, and Newegg.

### Domain Entities

- **Client**: Online retailers using 3PL services
- **Product**: Items stored in the warehouse
- **Inventory**: Stock levels and locations
- **Order**: Customer orders to fulfill
- **Shipment**: Outbound deliveries
- **Location**: Warehouse zones and bins

### Example: Low Stock Alerts for Premium Clients

```csharp
// Specifications
var isPremiumClient = new ClientSpecifications.IsPremiumOrEnterpriseSpecification();
var isActive = new ClientSpecifications.IsActiveSpecification();
var isBelowReorder = new InventorySpecifications.IsBelowReorderPointSpecification();

// Complex rule: Premium clients with active contracts having low stock
var premiumActiveClients = clients.Where(isPremiumClient.And(isActive));
var lowStockInventory = inventory.Where(isBelowReorder);

// Generate alerts
foreach (var client in premiumActiveClients)
{
    var clientLowStock = lowStockInventory
        .Where(inv => inv.ClientId == client.Id)
        .ToList();
    
    // Send notification...
}
```

### Example: Expedited Order Processing

```csharp
// Identify orders requiring immediate attention
var requiresExpedited = new OrderSpecifications.RequiresExpeditedProcessingSpecification();
var isPending = new OrderSpecifications.HasStatusSpecification(OrderStatus.Pending);

var urgentOrders = orders
    .Where(requiresExpedited.And(isPending))
    .OrderBy(o => o.RequiredDate)
    .ToList();

// Process urgently...
```

### Example: Special Handling Products

```csharp
// Products requiring special storage conditions
var isHazmat = new ProductSpecifications.IsHazmatSpecification();
var requiresRefrigeration = new ProductSpecifications.RequiresRefrigerationSpecification();
var isFragile = new ProductSpecifications.IsFragileSpecification();

var requiresSpecialHandling = isHazmat.Or(requiresRefrigeration).Or(isFragile);

var specialProducts = products.Where(requiresSpecialHandling);

// Assign to appropriate locations
foreach (var product in specialProducts)
{
    var suitableLocations = locations.Where(loc =>
        (!product.IsHazmat || loc.IsHazmatApproved) &&
        (!product.RequiresRefrigeration || loc.IsTemperatureControlled)
    );
}
```

### Example: Expiring Inventory Management

```csharp
// Multi-level expiration alerts
var isExpired = new ProductSpecifications.IsExpiredSpecification();
var isExpiring7Days = new ProductSpecifications.IsExpiringSpecification(7);
var isExpiring30Days = new ProductSpecifications.IsExpiringSpecification(30);

// Critical: Already expired
var expiredProducts = products.Where(isExpired);

// High: Expiring within 7 days
var expiringSoon = products.Where(isExpiring7Days.AndNot(isExpired));

// Medium: Expiring within 30 days but not within 7
var expiringMedium = products.Where(isExpiring30Days.AndNot(isExpiring7Days));
```

## 🏗️ Architecture

```
CustomSpecifications/
├── Core/
│   ├── ISpecification.cs                 # Main interface
│   ├── Specification.cs                  # Base implementation
│   ├── LinqSpecification.cs             # Expression tree support
│   └── CompositeSpecifications/
│       ├── AndSpecification.cs          # Logical AND
│       ├── OrSpecification.cs           # Logical OR
│       ├── NotSpecification.cs          # Logical NOT
│       ├── AndNotSpecification.cs       # Logical AND NOT
│       └── OrNotSpecification.cs        # Logical OR NOT
├── Extensions/
│   └── SpecificationExtensions.cs       # LINQ integration
└── Examples/
    ├── Simple/
    │   └── SimpleExamples.cs            # Basic examples
    └── WMS/
        ├── Models/                       # Domain models
        ├── Specifications/               # Business rules
        └── AdvancedWMSExamples.cs       # Complex scenarios
```

## 📖 Examples Included

### Simple Examples
1. **User Validation**: Age and status checks
2. **Email Validation**: Multi-rule email verification
3. **Number Ranges**: Numeric validation with ranges
4. **String Content**: Password strength validation
5. **NOT Operator**: Negation examples

### Advanced WMS Examples
1. **Low Stock Alerts**: Priority notifications for premium clients
2. **Expedited Processing**: Rush order identification
3. **Location Assignment**: Special handling requirements
4. **Expiring Inventory**: Multi-tier expiration management
5. **Order Batching**: Multi-client batching restrictions
6. **SLA Monitoring**: Compliance tracking by client tier
7. **Cycle Counting**: Prioritized inventory counts
8. **International Compliance**: Cross-border shipping rules

## ▶️ Running the Examples

```csharp
using CustomSpecifications.Examples.Simple;
using CustomSpecifications.Examples.WMS;

// Run simple examples
SimpleExamples.RunAll();

// Run advanced WMS examples
AdvancedWMSExamples.RunAll();

// Or run individual examples
SimpleExamples.Example1_SimpleUserValidation();
AdvancedWMSExamples.Example1_LowStockPremiumClients();
```

## 💡 Best Practices

### 1. Single Responsibility
Each specification should encapsulate ONE business rule:

```csharp
// Good ✅
public class IsAdultSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => candidate.Age >= 18;
}

// Avoid ❌
public class IsAdultAndActiveSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => 
        candidate.Age >= 18 && candidate.IsActive;  // Too much responsibility
}
```

### 2. Compose Complex Rules
Build complex specifications from simple ones:

```csharp
var isEligible = isAdult.And(isActive).And(hasValidEmail).AndNot(isBanned);
```

### 3. Parameterized Specifications
Use constructor parameters for flexibility:

```csharp
public class IsExpiringSpecification : Specification<Product>
{
    private readonly int _daysUntilExpiration;

    public IsExpiringSpecification(int daysUntilExpiration = 30)
    {
        _daysUntilExpiration = daysUntilExpiration;
    }

    public override bool IsSatisfiedBy(Product candidate) =>
        candidate.ExpirationDate.HasValue &&
        (candidate.ExpirationDate.Value - DateTime.UtcNow).Days <= _daysUntilExpiration;
}
```

### 4. Naming Conventions
Use descriptive names that read like English:

```csharp
// Good ✅
var activeAdultUsers = users.Where(isAdult.And(isActive));

// Avoid ❌
var result = users.Where(spec1.And(spec2));
```

### 5. Immutability
Keep specifications immutable and stateless:

```csharp
// Good ✅
public class HasMinimumBalanceSpecification : Specification<Account>
{
    private readonly decimal _minimumBalance;  // Readonly
    
    public HasMinimumBalanceSpecification(decimal minimumBalance)
    {
        _minimumBalance = minimumBalance;
    }
}
```

## 🧪 Testing Specifications

Specifications are highly testable:

```csharp
[Fact]
public void IsAdultSpecification_WithAge18_ReturnsTrue()
{
    // Arrange
    var spec = new IsAdultSpecification();
    var user = new User("test", "test@test.com", 18, true);

    // Act
    var result = spec.IsSatisfiedBy(user);

    // Assert
    Assert.True(result);
}

[Fact]
public void CombinedSpecification_WithAndOperator_ReturnsTrueWhenBothSatisfied()
{
    // Arrange
    var isAdult = new IsAdultSpecification();
    var isActive = new IsActiveSpecification();
    var combined = isAdult.And(isActive);
    var user = new User("test", "test@test.com", 25, true);

    // Act
    var result = combined.IsSatisfiedBy(user);

    // Assert
    Assert.True(result);
}
```

## 📄 References

- [Wikipedia: Specification Pattern](https://en.wikipedia.org/wiki/Specification_pattern)
- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Martin Fowler: Specifications](https://martinfowler.com/apsupp/spec.pdf)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📜 License

This project is licensed under the MIT License.

## 🙏 Acknowledgments

- Eric Evans and Martin Fowler for the Specification Pattern
- The Domain-Driven Design community
- 3PL logistics industry for real-world inspiration

---

**Built with ❤️ using .NET 8 and modern C# features**

