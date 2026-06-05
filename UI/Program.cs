using Core;

var usersPath = "c:\\tmp\\Users.txt";
var peoplePath = "c:\\tmp\\people.txt";
var logPath = "c:\\tmp\\log.txt";

using var logger = new LogWriter(logPath);
logger.WriteLog("info", "Application started");

// Login
var loggedUser = Login(usersPath, logger);
if (loggedUser == null)
{
    Console.WriteLine("Login failed. Exiting application.");
    logger.WriteLog("error", "Login failed");
    return;
}

Console.WriteLine($"\nWelcome, {loggedUser.Username}!");
logger.WriteLog("infor", $"User {loggedUser.Username} logged in successfully");

// Load people data

var people = File.Exists(peoplePath) 
    ? File.ReadAllLines(peoplePath).Where(l => !string.IsNullOrWhiteSpace(l))
          .Select(Person.Parse).ToList()
    : new List<Person>();

// Menu

var option = string.Empty;
do
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("1. Show content");
    Console.WriteLine("2. Add person");
    Console.WriteLine("3. Save changes");
    Console.WriteLine("4. Edit person");
    Console.WriteLine("5. Delete person");
    Console.WriteLine("6. Report by city");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    option = Console.ReadLine() ?? "0";

    switch (option)
    {
        case "1":
            ShowContent(people);
            logger.WriteLog("info", $"[{loggedUser.Username}] Listed all people.");
            break;
        case "2":
            AddPerson(people, loggedUser.Username, logger);
            break;
        case "3":
            SaveChanges(peoplePath, people, loggedUser.Username, logger);
            break;
        case "4":
            EditPerson(people, loggedUser.Username, logger);
            break;
        case "5":
            DeletePerson(people, loggedUser.Username, logger);
            break;
        case "6":
            ReportByCity(people, loggedUser.Username, logger);
            break;
        case "0":
            SaveChanges(peoplePath, people, loggedUser.Username, logger);
            logger.WriteLog("info", $"[{loggedUser.Username}] Application exited.");
            break;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
} while (option != "0");

void ShowContent(List<Person> list)
{
    if (list.Count == 0)
    {
        Console.WriteLine("There are no registered people.");
        return;
    }
    Console.WriteLine("\n========================================");
    foreach (var p in list)
    {
        Console.WriteLine($"\n{p.Id,-5}   {p.FullName}");
        Console.WriteLine($"        Phone:   {p.Phone}");
        Console.WriteLine($"        City:    {p.City}");
        Console.WriteLine($"        Balance: ${p.Balance,19:N2}");
    }
    Console.WriteLine("\n========================================");
}

void AddPerson(List<Person> list, string user, LogWriter log)
{
    Console.WriteLine("\n--- Add Person ---");

    // Validations
    int id;
    while (true)
    {
        Console.Write("ID: ");
        var input = Console.ReadLine() ?? "";
        if (!int.TryParse(input, out id))
        {
            Console.WriteLine("The ID must be a number.");
            continue;
        }
        if (list.Any(p => p.Id == id))
        {
            Console.WriteLine("That ID already exists. Please enter a different one.");
            continue;
        }
        break;
    }

    string firstName;
    while (true)
    {
        Console.Write("First name: ");
        firstName = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(firstName)) break;
        Console.WriteLine("The first name cannot be empty.");
    }

    string lastName;
    while (true)
    {
        Console.Write("Last name: ");
        lastName = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(lastName)) break;
        Console.WriteLine("The last name cannot be empty.");
    }

    string phone;
    while (true)
    {
        Console.Write("Phone: ");
        phone = Console.ReadLine() ?? "";
        if (phone.Replace(" ", "").All(char.IsDigit) && phone.Length >= 7)
            break;
        Console.WriteLine("Invalid phone number. Only digits, minimum 7 characters.");
    }

    Console.Write("City: ");
    var city = Console.ReadLine() ?? "";

    decimal balance;
    while (true)
    {
        Console.Write("Balance: ");
        if (decimal.TryParse(Console.ReadLine(), out balance) && balance >= 0)
            break;
        Console.WriteLine("The balance must be a positive number.");
    }

    var person = new Person
    {
        Id = id,
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        City = city,
        Balance = balance
    };

    list.Add(person);
    Console.WriteLine($"Person '{person.FullName}' added successfully.");
    log.WriteLog("info", $"[{user}] Added person: ID={id}, Name={person.FullName}.");
}

void SaveChanges(string path, List<Person> list, string user, LogWriter log)
{
    File.WriteAllLines(path, list.Select(p => p.ToString()));
    Console.WriteLine("Changes saved.");
    log.WriteLog("info", $"[{user}] Changes saved. Total records: {list.Count}.");
}

void EditPerson(List<Person> list, string user, LogWriter log)
{
    Console.Write("\nEnter ID to edit: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var person = list.FirstOrDefault(p => p.Id == id);
    if (person == null)
    {
        Console.WriteLine("Person not found.");
        return;
    }

    Console.WriteLine($"Editing: {person.FullName} (Press Enter to keep current value)");

    Console.Write($"First name [{person.FirstName}]: ");
    var input = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(input)) person.FirstName = input;

    Console.Write($"Last Name [{person.LastName}]");
    input = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(input)) person.LastName = input;

    Console.Write($"Phone [{person.Phone}]");
    input = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(input))
    {
        if (input.Replace(" ", " ").All(char.IsDigit) && input.Length >= 7)
            person.Phone = input;
        else
            Console.WriteLine("Invalid phone number, keeping the previous value. ");
    }

    Console.Write($"City [{person.City}]: ");
    input = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(input)) person.City = input;

    Console.Write($"Balance [{person.Balance:F2}]: ");
    input = Console.ReadLine();
    if (!String.IsNullOrWhiteSpace(input))
    {
        if (decimal.TryParse(input, out decimal balance) && balance >= 0)
            person.Balance = balance;
        else
            Console.WriteLine("Invalid balance, keeping the previous value. ");
    }

    Console.WriteLine($"Person '{person.FullName}' updated.");
    log.WriteLog("info", $"[{user}] Edited person ID={id}. ");
}

void DeletePerson(List<Person> list, string user, LogWriter log)
{
    Console.Write("\nEnter ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var person = list.FirstOrDefault(p => p.Id == id);
    if (person == null)
    {
        Console.WriteLine("Person not found.");
        return;
    }

    Console.WriteLine($"\nID:        {person.Id}");
    Console.WriteLine($"Nombre:  {person.FullName}");
    Console.WriteLine($"Phone:   {person.Phone}");
    Console.WriteLine($"City:    {person.City}");
    Console.WriteLine($"Balance: ${person.Balance:N2}");
    Console.Write($"\nDo you want to delete this person? (s/n): ");

    var confirm = Console.ReadLine() ?? "";
    if (confirm.ToLower() == "s")
    {
        list.Remove(person);
        Console.WriteLine("Persona eliminada.");
        log.WriteLog("info", $"[{user}] Deleted person ID={id}, Name={person.FullName}. ");
    }
    else
    {
        Console.WriteLine("Operation cancelled.");
    }
}

void ReportByCity(List<Person> list, string user, LogWriter log)
{
    if (list.Count == 0)
    {
        Console.WriteLine("There's no person registered."); 
        return;
    }

    var grouped = list
        .OrderBy(p => p.City)
        .ThenBy(p => p.FullName)
        .GroupBy(p => p.City);

    decimal grandTotal = 0;

    Console.WriteLine($"\n{"ID",-5} {"First Name",-15} {"Last Name",-15} {"Balance",-10}");

    foreach (var group in grouped)
    {
        Console.WriteLine($"\nCity: {group.Key}");
        Console.WriteLine($"{"-",5} {"----------------",15} {"-----------------",15} {"-----------------",15}");

        decimal cityTotal = 0;
        foreach(var p in group)
        {
            Console.WriteLine(($"{p.Id,-5} {p.FirstName,-15} {p.LastName,-15} {p.Balance,15:N2}"));
            cityTotal += p.Balance;
        }

        Console.WriteLine($"{"═",5} {"═════════════",15} {"═════════════",15} {"═════════════",15}");
        Console.WriteLine($"Total: {group.Key,-30} {cityTotal,15:N2}");
        grandTotal += cityTotal;
    }

    Console.WriteLine($"\n{"═",5} {"═════════════",15} {"═════════════",15} {"═════════════",15}");
    Console.WriteLine($"{"Total General:",-37} {grandTotal,15:N2}");

    log.WriteLog("info", $"[{user}] Generated report by city.");
}

UserAccount? Login(string path, LogWriter log)
{
    var lines = File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string>();
    var users = lines.Select(UserAccount.Parse).ToList();

    int attempts = 0;

    while (attempts < 3)
    {
        Console.Write("Username: ");
        var username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? "";

        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine("User not found. Please try again.");
            log.WriteLog("warn", $"Login attempt with non-existent username: {username}");
            attempts++;
        }
        else if (!user.IsActive)
        {
                Console.WriteLine("User account is inactive. Please contact support.");
                log.WriteLog("warn", $"Login attempt for inactive user: {username}");
                return null;
            
        }
        else if (user.Password != password)
        {
            Console.WriteLine("Incorrect password. Please try again.");
            log.WriteLog("warn", $"Incorrect password attempt for user: {username}");
            attempts++;

            if(attempts == 3)
            {
                user.IsActive = false;
                File.WriteAllLines(path, users.Select(u => u.ToString()));
                Console.WriteLine("Too many failed attempts. Your account has been locked.");
                log.WriteLog("warn", $"User {username} has been locked due to too many failed login attempts");
                return null;
            }
        }
        else
        {
            return user; // Login successful
        }
    }

    return null;
}