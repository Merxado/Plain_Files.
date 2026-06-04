namespace Core;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string City { get; set; } = null!;
    public decimal Balance { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"{Id},{FirstName},{LastName},{Phone},{City},{Balance:F2}";
    }

    public static Person Parse(string line)
    {
        var fields = line.Split(',');
        return new Person
        {
            Id = int.Parse(fields[0]),
            FirstName = fields[1],
            LastName = fields[2],
            Phone = fields[3],
            City = fields[4],
            Balance = decimal.Parse(fields[5])
        };
    }
}