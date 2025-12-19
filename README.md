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
// Sample list of clients data based on Client model
List<Client> clients = new List<Client>
{ 
    new Client ( "1", "Client A", "clienta@example.com", ClientTier.Premium, new DateTime(2023, 1, 15), new DateTime(2023,2,21), true ),
    new Client ( "2", "Client B", "clientb@example.com", ClientTier.Enterprise, new DateTime(2023, 3, 20), new DateTime(2023,2,21),true ),
    new Client ( "3", "Client C", "clientc@example.com", ClientTier.Standard, new DateTime(2023, 5, 10),new DateTime(2025,2,3), true ),
    new Client ( "4", "Client D", "clientd@example.com", ClientTier.Premium, new DateTime(2023, 2, 28), new DateTime(2023,2,21),false ),
    new Client ( "5", "Client E", "cliente@example.com", ClientTier.Standard, new DateTime(2023, 6, 5), new DateTime(2025,12,21),true ),
    new Client ( "6", "Client F", "clientf@example.com", ClientTier.Enterprise, new DateTime(2023, 4, 12), new DateTime(2023,2,21),true ),
    new Client ( "7", "Client G", "clientg@example.com", ClientTier.Premium, new DateTime(2023, 7, 22), new DateTime(2023,2,21),true ),
    new Client ( "8", "Client H", "clienth@example.com", ClientTier.Standard, new DateTime(2023, 8, 30), new DateTime(2023,2,21),false ),
    new Client ( "9", "Client I", "clienti@example.com", ClientTier.Enterprise, new DateTime(2023, 9, 18), new DateTime(2020,12,1),true ),
    new Client ( "10", "Client J", "clientj@example.com", ClientTier.Premium, new DateTime(2023, 10, 25), new DateTime(2025,9,5),false )
};

// Sample list of inventory data based on Inventory model
List<Inventory> inventory = new List<Inventory>
{
    new Inventory ( "1", "1kas7-rew","1","ATL-12", 5090, 100, 7500, new DateTime(2023, 1, 15), InventoryStatus.Available, null ),
    new Inventory ( "2", "2mfg8-xyz","2","NYC-45", 3200, 250, 5000, new DateTime(2023, 2, 10), InventoryStatus.Available, null ),
    new Inventory ( "3", "3pqr9-abc","1","CHI-78", 150, 200, 800, new DateTime(2023, 3, 5), InventoryStatus.Reserved, null ),
    new Inventory ( "4", "4stu0-def","3","LAX-23", 8900, 500, 10000, new DateTime(2023, 4, 20), InventoryStatus.Available, null ),
    new Inventory ( "5", "5vwx1-ghi","2","MIA-56", 45, 100, 600, new DateTime(2023, 5, 12), InventoryStatus.InTransit, null ),
    new Inventory ( "6", "6yza2-jkl","1","SEA-89", 4500, 300, 6000, new DateTime(2023, 6, 18), InventoryStatus.Available, null ),
    new Inventory ( "7", "7bcd3-mno","4","DFW-34", 220, 150, 1000, new DateTime(2023, 7, 8), InventoryStatus.Reserved, null ),
    new Inventory ( "8", "8efg4-pqr","3","BOS-67", 6700, 400, 8000, new DateTime(2023, 8, 22), InventoryStatus.Available, null ),
    new Inventory ( "9", "9hij5-stu","5","DEN-90", 80, 120, 500, new DateTime(2023, 9, 15), InventoryStatus.Available, null ),
    new Inventory ( "10", "10klm6-vwx","2","PHX-12", 5500, 350, 7000, new DateTime(2023, 10, 3), InventoryStatus.Available, null ),
    new Inventory ( "11", "11nop7-yza","1","HOU-45", 95, 200, 700, new DateTime(2023, 11, 11), InventoryStatus.Available, null ),
    new Inventory ( "12", "12qrs8-bcd","6","SFO-78", 7800, 450, 9000, new DateTime(2023, 12, 1), InventoryStatus.Available, null ),
    new Inventory ( "13", "13tuv9-efg","3","PDX-23", 3400, 280, 5500, new DateTime(2024, 1, 7), InventoryStatus.Reserved, null ),
    new Inventory ( "14", "14wxy0-hij","4","MSP-56", 120, 180, 900, new DateTime(2024, 2, 14), InventoryStatus.Available, null ),
    new Inventory ( "15", "15zab1-klm","1","DTW-89", 4200, 320, 6500, new DateTime(2024, 3, 19), InventoryStatus.Available, null ),
    new Inventory ( "16", "16cde2-nop","2","SLC-34", 35, 90, 400, new DateTime(2024, 4, 25), InventoryStatus.Quarantine, null ),
    new Inventory ( "17", "17fgh3-qrs","5","CLT-67", 6100, 380, 8500, new DateTime(2024, 5, 30), InventoryStatus.Available, null ),
    new Inventory ( "18", "18ijk4-tuv","3","LAS-90", 2800, 220, 4500, new DateTime(2024, 6, 12), InventoryStatus.Available, null ),
    new Inventory ( "19", "19lmn5-wxy","1","PHL-12", 175, 250, 1200, new DateTime(2024, 7, 8), InventoryStatus.Reserved, null ),
    new Inventory ( "20", "20opq6-zab","4","MCO-45", 7200, 410, 9500, new DateTime(2024, 8, 16), InventoryStatus.Available, null ),
    new Inventory ( "21", "21rst7-cde","2","BNA-78", 65, 130, 550, new DateTime(2024, 9, 21), InventoryStatus.Available, null ),
    new Inventory ( "22", "22uvw8-fgh","6","AUS-23", 8500, 480, 10500, new DateTime(2024, 10, 5), InventoryStatus.Available, null ),
    new Inventory ( "23", "23xyz9-ijk","1","RDU-56", 3900, 290, 6200, new DateTime(2024, 11, 13), InventoryStatus.Available, null ),
    new Inventory ( "24", "24abc0-lmn","3","SAN-89", 190, 210, 1100, new DateTime(2024, 12, 20), InventoryStatus.Reserved, null ),
    new Inventory ( "25", "25def1-opq","5","IND-34", 5600, 360, 7800, new DateTime(2025, 1, 2), InventoryStatus.Available, null ),
    new Inventory ( "26", "26ghi2-rst","2","CMH-67", 50, 110, 450, new DateTime(2025, 2, 9), InventoryStatus.Damaged, null ),
    new Inventory ( "27", "27jkl3-uvw","1","JAX-90", 4800, 330, 6800, new DateTime(2025, 3, 17), InventoryStatus.Available, null ),
    new Inventory ( "28", "28mno4-xyz","4","MEM-12", 2500, 240, 4200, new DateTime(2025, 4, 23), InventoryStatus.Available, null ),
    new Inventory ( "29", "29pqr5-abc","3","OKC-45", 140, 190, 850, new DateTime(2025, 5, 28), InventoryStatus.Reserved, null ),
    new Inventory ( "30", "30stu6-def","6","RIC-78", 7500, 440, 9800, new DateTime(2025, 6, 4), InventoryStatus.Available, null ),
    new Inventory ( "31", "31vwx7-ghi","1","TPA-23", 4100, 310, 6400, new DateTime(2025, 7, 10), InventoryStatus.Available, null )
};

// Specifications
var isPremiumClient = new ClientSpecifications.IsPremiumOrEnterpriseSpecification();
var isActive = new ClientSpecifications.IsActiveSpecification();
var isBelowReorder = new InventorySpecifications.IsBelowReorderPointSpecification();


// Complex rule: Premium clients with active contracts having low stock
var premiumActiveClients = clients.Where(isPremiumClient.And(isActive));
var lowStockByClient = inventory
    .Where(isBelowReorder)
    .ToLookup(inv => inv.ClientId);

// Generate alerts
premiumActiveClients
    .Where(client => lowStockByClient.Contains(client.Id))
    .SelectMany(client => lowStockByClient[client.Id]
        .Select(item => $"Alert: {client.Name} at location {item.LocationId} has low stock on Sku: {item.Sku} and has only {item.Quantity} items in stock."))
    .ToList()
    .ForEach(Console.WriteLine); //Push notifications to console
```

### Example: Expedited Order Processing

```csharp
// A sample list of orders data based on Order model
List<Order> orders = new List<Order>
{
    new Order("1", "1", new DateTime(2025, 7, 10), new DateTime(2025, 7, 18), OrderPriority.Rush, ShippingMethod.Ground, "USA", OrderStatus.Pending, new List<OrderLine>{new OrderLine("1kas7-rew", 50, 25)}),
    new Order("2", "2", new DateTime(2025, 7, 11), new DateTime(2025, 7, 20), OrderPriority.Normal, ShippingMethod.TwoDayAir, "USA", OrderStatus.InProgress, new List<OrderLine>{new OrderLine("2mfg8-xyz", 100, 50)}),
    new Order("3", "1", new DateTime(2025, 7, 12), new DateTime(2025, 7, 15), OrderPriority.Rush, ShippingMethod.Overnight, "USA", OrderStatus.Pending, new List<OrderLine>{new OrderLine("3pqr9-abc", 75, 30)}),
    new Order("4", "3", new DateTime(2025, 7, 13), new DateTime(2025, 7, 25), OrderPriority.Normal, ShippingMethod.Ground, "Canada", OrderStatus.Shipped, new List<OrderLine>{new OrderLine("4stu0-def", 200, 100)}),
    new Order("5", "2", new DateTime(2025, 7, 14), new DateTime(2025, 7, 16), OrderPriority.Rush, ShippingMethod.TwoDayAir, "USA", OrderStatus.Pending, new List<OrderLine>{new OrderLine("5vwx1-ghi", 40, 20)}),
    new Order("6", "1", new DateTime(2025, 7, 15), new DateTime(2025, 7, 28), OrderPriority.Low, ShippingMethod.Ground, "Mexico", OrderStatus.Delivered, new List<OrderLine>{new OrderLine("6yza2-jkl", 150, 75)}),
    new Order("7", "4", new DateTime(2025, 7, 16), new DateTime(2025, 7, 17), OrderPriority.Rush, ShippingMethod.Overnight, "USA", OrderStatus.OnHold, new List<OrderLine>{new OrderLine("7bcd3-mno", 80, 40)}),
    new Order("8", "3", new DateTime(2025, 7, 17), new DateTime(2025, 7, 30), OrderPriority.Normal, ShippingMethod.Ground, "USA", OrderStatus.Pending, new List<OrderLine>{new OrderLine("8efg4-pqr", 120, 60)}),
    new Order("9", "5", new DateTime(2025, 7, 18), new DateTime(2025, 7, 22), OrderPriority.Normal, ShippingMethod.TwoDayAir, "Canada", OrderStatus.Packed, new List<OrderLine>{new OrderLine("9hij5-stu", 60, 25)}),
    new Order("10", "2", new DateTime(2025, 7, 19), new DateTime(2025, 7, 21), OrderPriority.Rush, ShippingMethod.TwoDayAir, "USA", OrderStatus.Pending, new List<OrderLine>{new OrderLine("10klm6-vwx", 90, 45)})
};

// Identify orders requiring immediate attention
var requiresExpedited = new OrderSpecifications.RequiresExpeditedProcessingSpecification();
var isPending = new OrderSpecifications.HasStatusSpecification(OrderStatus.Pending);

var urgentOrders = orders
    .Where(requiresExpedited.And(isPending))
    .OrderBy(o => o.RequiredDate)
    .ToList();

// Generate alerts
urgentOrders.ForEach(order => 
    Console.WriteLine($"Alert: Order {order.Id} for product {order.Lines.First().Sku} shipping status is {order.Status} and is estimated to be delivered by {order.RequiredDate.ToShortDateString()}."));
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

 Save
