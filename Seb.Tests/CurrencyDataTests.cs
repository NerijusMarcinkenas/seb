using FluentAssertions;
using Seb.Server.Domain;

namespace Seb.Tests;

public class CurrencyDataTests
{
    [Fact]
    public void Create_CurrencyCountZero_ThrowsException()
    {
        // Arrange
        var now = DateTime.UtcNow;
        // Act
        var result = () => CurrencyData.Create([], now);

        // Assert
        result.Should().Throw<ArgumentException>("Currencies collection cannot be empty");
    }
    
    [Fact]
    public void Create_CreatesCurrencyData()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var currencies = BuildCurrencies();
        
        // Act
        var result = CurrencyData.Create(currencies, now);

        // Assert
        result.DateStamp.Should().Be(now);
        result.Currencies.Should().NotBeEmpty();
    }

    private static IReadOnlyCollection<Currency> BuildCurrencies()
    {
        return new List<Currency>
        {
            Currency.Create("Euro", "EUR", 1.0m, DateTime.UtcNow),
            Currency.Create("US Dollar", "USD", 1.2m, DateTime.UtcNow),
            Currency.Create("British Pound", "GBP", 0.8m, DateTime.UtcNow)
        };
    }
}