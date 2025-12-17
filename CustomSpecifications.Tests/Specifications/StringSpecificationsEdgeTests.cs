using CustomSpecifications.Examples.Simple;
using Xunit;

namespace CustomSpecifications.Tests.Specifications;

public class StringSpecificationsEdgeTests
{
    private readonly HasAtSymbolSpecification _hasAt = new();
    private readonly HasDomainSpecification _hasDomain = new();
    private readonly NotSpamDomainSpecification _notSpam = new();
    private readonly MinLengthSpecification _minLen8 = new(8);
    private readonly HasSpecialCharacterSpecification _hasSpecial = new();
    private readonly HasDigitSpecification _hasDigit = new();

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("noatsymbol", false)]
    [InlineData("user@", false)]
    [InlineData("@domain.com", false)]
    [InlineData("user@@domain.com", false)]
    [InlineData("user@domain", false)]
    [InlineData("user@domain.", false)]
    [InlineData("user@sub.domain.com", true)]
    [InlineData("USER@DOMAIN.COM", true)]
    public void HasDomain_EdgeCases(string? email, bool expected)
    {
        Assert.Equal(expected, _hasDomain.IsSatisfiedBy(email ?? string.Empty));
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("noatsymbol", true)]
    [InlineData("user@nospam.com", true)]
    [InlineData("user@spam.com", false)]
    [InlineData("user@SPAM.com", false)]
    [InlineData("user@trash.com", false)]
    [InlineData("user@junk.com", false)]
    [InlineData("user@spam.com ", false)]
    [InlineData(" user@spam.com", false)]
    public void NotSpamDomain_CaseAndWhitespace(string? email, bool expected)
    {
        var candidate = email?.Trim() ?? string.Empty;
        Assert.Equal(expected, _notSpam.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("short", false)]
    [InlineData("1234567", false)]
    [InlineData("12345678", true)]
    [InlineData("123456789", true)]
    public void MinLength_Boundaries(string? value, bool expected)
    {
        Assert.Equal(expected, _minLen8.IsSatisfiedBy(value ?? string.Empty));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("NoDigitsHere", false)]
    [InlineData("Contains1Digit", true)]
    [InlineData("12345", true)]
    public void HasDigit_EdgeCases(string? value, bool expected)
    {
        Assert.Equal(expected, _hasDigit.IsSatisfiedBy(value ?? string.Empty));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("AlphaNum123", false)]
    [InlineData("Has@Symbol", true)]
    [InlineData("Space here", true)]
    [InlineData("Tab\tHere", true)]
    public void HasSpecialCharacter_EdgeCases(string? value, bool expected)
    {
        Assert.Equal(expected, _hasSpecial.IsSatisfiedBy(value ?? string.Empty));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("nodomain", false)]
    [InlineData("user@domain.com", true)]
    public void HasAtSymbol_Cases(string? email, bool expected)
    {
        Assert.Equal(expected, _hasAt.IsSatisfiedBy(email ?? string.Empty));
    }

    [Theory]
    [InlineData("short", false)]
    [InlineData("longbutnosymbols", false)]
    [InlineData("Long@WithSymbol", false)]
    [InlineData("NoNum@Symbol", false)]
    [InlineData("ValidP@ssw0rd", true)]
    public void StrongPassword_Composition(string password, bool expected)
    {
        var strongPassword = _minLen8.And(_hasSpecial).And(_hasDigit);
        Assert.Equal(expected, strongPassword.IsSatisfiedBy(password));
    }
}
