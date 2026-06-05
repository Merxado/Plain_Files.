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
    ? File.ReadAllLines(peoplePath).Where(1 => !string.IsNullOrWhiteSpace(1))
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
        case "0":
            SaveChanges(peoplePath, people, loggedUser.Username, logger);
            logger.WriteLog("info", $"[{loggedUser.Username}] Application exited.");
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (option != "0");

void ShowContent(List<Person> list)
{
    if (list.Count == 0)
    {
        Console.WriteLine("No hay personas registradas.");
        return;
    }
    Console.WriteLine("\n========================================");
    foreach (var p in list)
    {
        Console.WriteLine($"\n{p.Id,-5}   {p.FullName}");
        Console.WriteLine($"        Phone:   {p.Phone}");
        Console.WriteLine($"        City:    {p.City}");
        Console.WriteLine($"        Balance: {p.Balance,20:C2}");
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
            Console.WriteLine("El ID debe ser un número.");
            continue;
        }
        if (list.Any(p => p.Id == id))
        {
            Console.WriteLine("Ese ID ya existe. Ingrese uno diferente.");
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
        Console.WriteLine("El nombre no puede estar vacío.");
    }

    string lastName;
    while (true)
    {
        Console.Write("Last name: ");
        lastName = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(lastName)) break;
        Console.WriteLine("El apellido no puede estar vacío.");
    }

    string phone;
    while (true)
    {
        Console.Write("Phone: ");
        phone = Console.ReadLine() ?? "";
        if (phone.Replace(" ", "").All(char.IsDigit) && phone.Length >= 7)
            break;
        Console.WriteLine("Teléfono inválido. Solo números, mínimo 7 dígitos.");
    }

    Console.Write("City: ");
    var city = Console.ReadLine() ?? "";

    decimal balance;
    while (true)
    {
        Console.Write("Balance: ");
        if (decimal.TryParse(Console.ReadLine(), out balance) && balance >= 0)
            break;
        Console.WriteLine("El balance debe ser un número positivo.");
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
    Console.WriteLine($"Persona '{person.FullName}' agregada correctamente.");
    log.WriteLog("info", $"[{user}] Added person: ID={id}, Name={person.FullName}.");
}

void SaveChanges(string path, List<Person> list, string user, LogWriter log)
{
    File.WriteAllLines(path, list.Select(p => p.ToString()));
    Console.WriteLine("Cambios guardados.");
    log.WriteLog("info", $"[{user}] Changes saved. Total records: {list.Count}.");
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