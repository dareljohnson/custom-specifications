using CustomSpecifications.Core;
using CustomSpecifications.Extensions;

namespace CustomSpecifications.Examples.Simple;

/// <summary>
/// Simple examples demonstrating basic specification pattern usage.
/// </summary>
public static class SimpleExamples
{
    /// <summary>
    /// Example 1: Basic user validation specification.
    /// </summary>
    public static void Example1_SimpleUserValidation()
    {
        Console.WriteLine("=== Example 1: Simple User Validation ===\n");

        var users = new List<User>
        {
            new("user1", "john@example.com", 25, true),
            new("user2", "jane@example.com", 17, true),
            new("user3", "bob@example.com", 30, false),
            new("user4", "alice@example.com", 22, true)
        };

        // Create specifications
        var isAdult = new IsAdultSpecification();
        var isActive = new IsActiveUserSpecification();

        // Find active adult users
        var activeAdults = users.Where(isAdult.And(isActive)).ToList();

        Console.WriteLine("Active adult users:");
        foreach (var user in activeAdults)
        {
            Console.WriteLine($"  - {user.Username} ({user.Email}), Age: {user.Age}");
        }

        Console.WriteLine($"\nTotal: {activeAdults.Count}\n");
    }

    /// <summary>
    /// Example 2: Email validation with multiple rules.
    /// </summary>
    public static void Example2_EmailValidation()
    {
        Console.WriteLine("=== Example 2: Email Validation ===\n");

        var emails = new List<string>
        {
            "valid@example.com",
            "invalid-email",
            "test@spam.com",
            "admin@company.com",
            ""
        };

        var hasAtSymbol = new HasAtSymbolSpecification();
        var hasDomain = new HasDomainSpecification();
        var notSpam = new NotSpamDomainSpecification();

        // Combined specification: valid AND not spam
        var validNonSpamEmail = hasAtSymbol.And(hasDomain).And(notSpam);

        Console.WriteLine("Valid non-spam emails:");
        foreach (var email in emails)
        {
            if (validNonSpamEmail.IsSatisfiedBy(email))
            {
                Console.WriteLine($"  ? {email}");
            }
            else
            {
                Console.WriteLine($"  ? {email}");
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Example 3: Number range specifications.
    /// </summary>
    public static void Example3_NumberRanges()
    {
        Console.WriteLine("=== Example 3: Number Range Validation ===\n");

        var numbers = new List<int> { -5, 0, 15, 25, 50, 75, 101 };

        var isPositive = new IsPositiveSpecification();
        var inRange = new IsInRangeSpecification(1, 100);

        // Numbers that are positive AND in range [1, 100]
        var validNumbers = numbers.Where(isPositive.And(inRange));

        Console.WriteLine("Numbers that are positive and in range [1, 100]:");
        Console.WriteLine($"  {string.Join(", ", validNumbers)}");

        // Numbers that are positive OR in range [1, 100]
        var relaxedNumbers = numbers.Where(isPositive.Or(inRange));

        Console.WriteLine("\nNumbers that are positive OR in range [1, 100]:");
        Console.WriteLine($"  {string.Join(", ", relaxedNumbers)}");

        Console.WriteLine();
    }

    /// <summary>
    /// Example 4: String content specifications.
    /// </summary>
    public static void Example4_StringContent()
    {
        Console.WriteLine("=== Example 4: String Content Validation ===\n");

        var passwords = new List<string>
        {
            "short",
            "longbutnosymbols",
            "Long@WithSymbol",
            "NoNum@Symbol",
            "ValidP@ssw0rd"
        };

        var minLength = new MinLengthSpecification(8);
        var hasSpecialChar = new HasSpecialCharacterSpecification();
        var hasDigit = new HasDigitSpecification();

        // Strong password: min length AND special char AND digit
        var strongPassword = minLength.And(hasSpecialChar).And(hasDigit);

        Console.WriteLine("Password strength validation:");
        foreach (var password in passwords)
        {
            var isStrong = strongPassword.IsSatisfiedBy(password);
            Console.WriteLine($"  {password,-20} ? {(isStrong ? "Strong ?" : "Weak ?")}");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Example 5: Using NOT operator.
    /// </summary>
    public static void Example5_NotOperator()
    {
        Console.WriteLine("=== Example 5: NOT Operator ===\n");

        var numbers = Enumerable.Range(1, 20).ToList();

        var isEven = new IsEvenSpecification();
        var isOdd = isEven.Not(); // Negation using NOT

        Console.WriteLine("Even numbers:");
        Console.WriteLine($"  {string.Join(", ", numbers.Where(isEven))}");

        Console.WriteLine("\nOdd numbers (using NOT):");
        Console.WriteLine($"  {string.Join(", ", numbers.Where(isOdd))}");

        Console.WriteLine();
    }

    /// <summary>
    /// Run all simple examples.
    /// </summary>
    public static void RunAll()
    {
        Example1_SimpleUserValidation();
        Example2_EmailValidation();
        Example3_NumberRanges();
        Example4_StringContent();
        Example5_NotOperator();
    }
}

// Supporting types and specifications for simple examples

public record User(string Username, string Email, int Age, bool IsActive);

public class IsAdultSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => candidate.Age >= 18;
}

public class IsActiveUserSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User candidate) => candidate.IsActive;
}

public class HasAtSymbolSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string candidate) =>
        !string.IsNullOrEmpty(candidate) && candidate.Contains('@');
}

public class HasDomainSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string candidate) =>
        !string.IsNullOrEmpty(candidate) &&
        candidate.Contains('@') &&
        candidate.Split('@').Length == 2 &&
        candidate.Split('@')[0].Length > 0 &&
        candidate.Split('@')[1].Contains('.') &&
        !candidate.Split('@')[1].StartsWith('.') &&
        !candidate.Split('@')[1].EndsWith('.');
}

public class NotSpamDomainSpecification : Specification<string>
{
    private readonly string[] _spamDomains = { "spam.com", "junk.com", "trash.com" };

    public override bool IsSatisfiedBy(string candidate)
    {
        if (string.IsNullOrEmpty(candidate) || !candidate.Contains('@'))
            return true;

        var domain = candidate.Split('@')[1];
        return !_spamDomains.Contains(domain, StringComparer.OrdinalIgnoreCase);
    }
}

public class IsPositiveSpecification : Specification<int>
{
    public override bool IsSatisfiedBy(int candidate) => candidate > 0;
}

public class IsInRangeSpecification : Specification<int>
{
    private readonly int _min;
    private readonly int _max;

    public IsInRangeSpecification(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public override bool IsSatisfiedBy(int candidate) =>
        candidate >= _min && candidate <= _max;
}

public class MinLengthSpecification : Specification<string>
{
    private readonly int _minLength;

    public MinLengthSpecification(int minLength) => _minLength = minLength;

    public override bool IsSatisfiedBy(string candidate) =>
        !string.IsNullOrEmpty(candidate) && candidate.Length >= _minLength;
}

public class HasSpecialCharacterSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string candidate) =>
        !string.IsNullOrEmpty(candidate) &&
        candidate.Any(c => !char.IsLetterOrDigit(c));
}

public class HasDigitSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string candidate) =>
        !string.IsNullOrEmpty(candidate) && candidate.Any(char.IsDigit);
}

public class IsEvenSpecification : Specification<int>
{
    public override bool IsSatisfiedBy(int candidate) => candidate % 2 == 0;
}
