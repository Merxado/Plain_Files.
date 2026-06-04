using Core;

var usersPath = "c:\\users.txt";
var peoplePath = "c:\\people.txt";
var logPath = "c:\\logo.txt";

using var logger = new LogWriter(logPath);
logger.WriteLog("infor", "Application started");

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

// Menu

UserAccount? Login (string path, LogWriter log)
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