namespace Seb.Server.Application;

public record CurrencyModel(string Name, string Code, decimal Rate, DateOnly DateStamp);