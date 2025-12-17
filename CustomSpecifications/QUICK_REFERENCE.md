# CustomSpecifications Quick Reference Guide

## Table of Contents
- [Basic Usage](#basic-usage)
- [Composition Operators](#composition-operators)
- [Extension Methods](#extension-methods)
- [Common Patterns](#common-patterns)
- [Cheat Sheet](#cheat-sheet)

## Basic Usage

### 1. Create a Specification

```csharp
public class IsAdultSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => candidate.Age >= 18;
}
```

### 2. Use a Specification

```csharp
var spec = new IsAdultSpecification();
var isAdult = spec.IsSatisfiedBy(user);
```

### 3. Filter Collections

```csharp
var adults = users.Where(new IsAdultSpecification());
```

## Composition Operators

### AND - Both Must Be True

```csharp
var isAdult = new IsAdultSpecification();
var isActive = new IsActiveSpecification();

// Both conditions must be satisfied
var activeAdults = users.Where(isAdult.And(isActive));
```

**Truth Table:**
| Left | Right | Result |
|------|-------|--------|
| T    | T     | T      |
| T    | F     | F      |
| F    | T     | F      |
| F    | F     | F      |

### OR - Either Can Be True

```csharp
var isPremium = new IsPremiumSpecification();
var isEnterprise = new IsEnterpriseSpecification();

// Either condition can be satisfied
var highTierClients = clients.Where(isPremium.Or(isEnterprise));
```

**Truth Table:**
| Left | Right | Result |
|------|-------|--------|
| T    | T     | T      |
| T    | F     | T      |
| F    | T     | T      |
| F    | F     | F      |

### NOT - Negation

```csharp
var isExpired = new IsExpiredSpecification();

// Opposite of the original specification
var notExpired = isExpired.Not();
var validProducts = products.Where(notExpired);
```

**Truth Table:**
| Input | Result |
|-------|--------|
| T     | F      |
| F     | T      |

### AND NOT - Left True, Right False

```csharp
var isEligible = new IsEligibleSpecification();
var isBanned = new IsBannedSpecification();

// Left must be true, right must be false
var allowedUsers = users.Where(isEligible.AndNot(isBanned));
```

**Truth Table:**
| Left | Right | Result |
|------|-------|--------|
| T    | T     | F      |
| T    | F     | T      |
| F    | T     | F      |
| F    | F     | F      |

### OR NOT - Left True OR Right False

```csharp
var isApproved = new IsApprovedSpecification();
var requiresApproval = new RequiresApprovalSpecification();

// Left is true OR right is false
var canProceed = items.Where(isApproved.OrNot(requiresApproval));
```

**Truth Table:**
| Left | Right | Result |
|------|-------|--------|
| T    | T     | T      |
| T    | F     | T      |
| F    | T     | F      |
| F    | F     | T      |

## Extension Methods

### Filtering

```csharp
var spec = new IsActiveSpecification();

// Standard LINQ methods work with specifications
var results = collection.Where(spec);
```

### Checking Existence

```csharp
// Check if any element satisfies the specification
bool hasActive = users.Any(new IsActiveSpecification());

// Check if all elements satisfy the specification
bool allActive = users.All(new IsActiveSpecification());
```

### Counting

```csharp
// Count elements that satisfy the specification
int activeCount = users.Count(new IsActiveSpecification());
```

### Finding Elements

```csharp
var spec = new IsAdminSpecification();

// Get first match
var firstAdmin = users.First(spec);

// Get first or default
var admin = users.FirstOrDefault(spec);

// Get single match (throws if more than one)
var onlyAdmin = users.Single(spec);

// Get single or default
var singleAdmin = users.SingleOrDefault(spec);
```

## Common Patterns

### Pattern 1: Simple Validation

```csharp
public class MinimumAgeSpecification : Specification<User>
{
    private readonly int _minimumAge;

    public MinimumAgeSpecification(int minimumAge) => _minimumAge = minimumAge;

    public override bool IsSatisfiedBy(User candidate) => candidate.Age >= _minimumAge;
}

// Usage
var canDrink = new MinimumAgeSpecification(21);
if (canDrink.IsSatisfiedBy(user))
{
    // Allow purchase
}
```

### Pattern 2: Complex Business Rule

```csharp
// Individual rules
var isOverdue = new IsOverdueSpecification();
var noticeSent = new NoticeSentSpecification();
var inCollection = new InCollectionSpecification();

// Combine into complex rule
var sendToCollection = isOverdue
    .And(noticeSent)
    .AndNot(inCollection);

// Apply to collection
foreach (var invoice in invoices.Where(sendToCollection))
{
    invoice.SendToCollection();
}
```

### Pattern 3: Dynamic Rule Building

```csharp
public ISpecification<Product> BuildProductFilter(FilterCriteria criteria)
{
    ISpecification<Product> spec = new TrueSpecification<Product>();

    if (criteria.Category.HasValue)
        spec = spec.And(new IsCategorySpecification(criteria.Category.Value));

    if (criteria.MaxPrice.HasValue)
        spec = spec.And(new MaxPriceSpecification(criteria.MaxPrice.Value));

    if (criteria.InStock)
        spec = spec.And(new IsInStockSpecification());

    return spec;
}
```

### Pattern 4: Specification Repository

```csharp
public static class CommonSpecifications
{
    public static ISpecification<User> ActiveUsers => new IsActiveSpecification();
    public static ISpecification<User> AdminUsers => new IsAdminSpecification();
    public static ISpecification<User> ActiveAdmins => ActiveUsers.And(AdminUsers);
}

// Usage
var activeAdmins = users.Where(CommonSpecifications.ActiveAdmins);
```

### Pattern 5: Parameterized Specifications

```csharp
public class DateRangeSpecification : Specification<Order>
{
    private readonly DateTime _start;
    private readonly DateTime _end;

    public DateRangeSpecification(DateTime start, DateTime end)
    {
        _start = start;
        _end = end;
    }

    public override bool IsSatisfiedBy(Order candidate) =>
        candidate.OrderDate >= _start && candidate.OrderDate <= _end;
}

// Usage
var thisMonth = new DateRangeSpecification(
    new DateTime(2025, 1, 1),
    new DateTime(2025, 1, 31)
);
```

## Cheat Sheet

### Creating Specifications

```csharp
// Simple specification
public class MySpec : Specification<MyType>
{
    public override bool IsSatisfiedBy(MyType candidate) 
        => /* your logic */;
}

// Parameterized specification
public class MySpec : Specification<MyType>
{
    private readonly int _threshold;
    
    public MySpec(int threshold) => _threshold = threshold;
    
    public override bool IsSatisfiedBy(MyType candidate) 
        => candidate.Value > _threshold;
}

// LINQ/EF specification
public class MyLinqSpec : LinqSpecification<MyType>
{
    public override Expression<Func<MyType, bool>> AsExpression() 
        => x => x.IsActive;
}
```

### Combining Specifications

```csharp
var result = spec1.And(spec2);           // Both true
var result = spec1.Or(spec2);            // Either true
var result = spec1.Not();                // Opposite
var result = spec1.AndNot(spec2);        // First true, second false
var result = spec1.OrNot(spec2);         // First true OR second false

// Complex combination
var result = spec1.And(spec2).Or(spec3).AndNot(spec4);
```

### Using with Collections

```csharp
// Filter
var filtered = collection.Where(spec);

// Check
var hasAny = collection.Any(spec);
var hasAll = collection.All(spec);

// Count
var count = collection.Count(spec);

// Find
var item = collection.First(spec);
var item = collection.FirstOrDefault(spec);
var item = collection.Single(spec);
var item = collection.SingleOrDefault(spec);
```

### Best Practices

? **DO:**
- Keep specifications simple and focused
- Use descriptive names
- Make specifications immutable
- Compose complex rules from simple ones
- Test specifications independently

? **DON'T:**
- Put multiple responsibilities in one specification
- Mutate specification state
- Use specifications for I/O operations
- Create deep inheritance hierarchies
- Forget to validate constructor parameters

### Common Mistakes to Avoid

```csharp
// ? BAD: Multiple responsibilities
public class ComplexSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) =>
        candidate.Age >= 18 && candidate.IsActive && candidate.HasValidEmail;
}

// ? GOOD: Single responsibility, composable
var isAdult = new IsAdultSpecification();
var isActive = new IsActiveSpecification();
var hasValidEmail = new HasValidEmailSpecification();
var eligible = isAdult.And(isActive).And(hasValidEmail);
```

```csharp
// ? BAD: Mutable state
public class BadSpecification : Specification<User>
{
    public int Threshold { get; set; }  // Mutable!
}

// ? GOOD: Immutable
public class GoodSpecification : Specification<User>
{
    private readonly int _threshold;  // Readonly
    public GoodSpecification(int threshold) => _threshold = threshold;
}
```

## Performance Tips

1. **Reuse Specifications**: Create once, use many times
   ```csharp
   // Good
   var spec = new IsActiveSpecification();
   var result1 = collection1.Where(spec);
   var result2 = collection2.Where(spec);
   ```

2. **Short-Circuit Evaluation**: Place most restrictive specs first
   ```csharp
   // Evaluates isExpensive first (more restrictive)
   var result = isExpensive.And(isInStock);
   ```

3. **Use LinqSpecification for Databases**: Translates to SQL
   ```csharp
   public class ActiveUserSpec : LinqSpecification<User>
   {
       public override Expression<Func<User, bool>> AsExpression() 
           => u => u.IsActive;
   }
   ```

## Quick Reference Card

| Operation | Syntax | When to Use |
|-----------|--------|-------------|
| **AND** | `spec1.And(spec2)` | Both conditions required |
| **OR** | `spec1.Or(spec2)` | Either condition acceptable |
| **NOT** | `spec.Not()` | Opposite condition needed |
| **AND NOT** | `spec1.AndNot(spec2)` | Include first, exclude second |
| **OR NOT** | `spec1.OrNot(spec2)` | Include first or exclude second |

---

**For complete examples, see:**
- `SimpleExamples.cs` - Basic usage patterns
- `AdvancedWMSExamples.cs` - Real-world warehouse scenarios
- `README.md` - Comprehensive documentation
