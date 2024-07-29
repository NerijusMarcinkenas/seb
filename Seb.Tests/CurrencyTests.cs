using FluentAssertions;
using Seb.Server.Domain;

namespace Seb.Tests;

public class CurrencyTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void Create_NameIsEmpty_ThrowsException(string name, string code, decimal rate,DateTime date, string expectedMessage)
    {
        // Act
        var result = () => Currency.Create(name, code, rate, date);

        // Assert
        result.Should().Throw<ArgumentException>(expectedMessage);
    }
    
    [Fact]
    public void Create_CreatesCurrency()
    {
        // Arrange
        var now = DateTime.UtcNow;
        
        // Act
        var result = Currency.Create("Euro", "EUR", 1.0m, now);

        // Assert
        result.Name.Should().Be("Euro");
        result.Code.Should().Be("EUR");
        result.Rate.Should().Be(1.0m);
        result.DateStamp.Should().Be(now);
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return ["", "EUR", 1.0m, DateTime.UtcNow, "Name cannot be empty"];
        yield return ["US Dollar", "", 1.2m, DateTime.UtcNow, "Currency code cannot be empty"];
        yield return ["British Pound", "GBP", 0, DateTime.UtcNow, "Currency rate must be greater than 0"];
        yield return
        [
            "British Pound British Pound British Pound British Pound ", "GBP", 1.2, DateTime.UtcNow,
            "Currency name cannot be longer than 50 characters"
        ];
        yield return
        [
            "British Pound  ", "GBPs", 1.2, DateTime.UtcNow, $"Currency code cannot be longer than 3 characters"
        ];
    }
}