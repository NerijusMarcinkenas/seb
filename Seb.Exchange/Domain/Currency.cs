namespace Seb.Server.Domain;

public class Currency
{
    public const int NameMaxLength = 50;
    public const int CodeMaxLength = 3;
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Currency() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Currency(string name, string code, decimal rate, DateTime dateStamp)
    {
        Name = name;
        Code = code;
        Rate = rate;
        DateStamp = dateStamp;
    }

    public string Name { get; }
    public decimal Rate { get; }
    public string Code { get; }
    public DateTime DateStamp { get; }
    
    public static Currency Create(string name, string code, decimal rate, DateTime dateStamp)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Currency name cannot be empty", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be empty", nameof(code));
        }

        if (rate <= 0)
        {
            throw new ArgumentException("Currency rate must be greater than 0", nameof(rate));
        }

        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Currency name cannot be longer than {NameMaxLength} characters",
                nameof(name));
        }

        if (code.Length > CodeMaxLength)
        {
            throw new ArgumentException($"Currency code cannot be longer than {CodeMaxLength} characters",
                nameof(code));
        }

        return new Currency(name, code, rate, dateStamp);
    }
}