//======================================================================
// EXAMPLE 1
//======================================================================

//using Core;

//var textFile = new SimpleTextFile("c:\\tmp\\animals.txt");
//using var logger = new LogWriter("c:\\tmp\\app.log");
//try
//{
//    logger.WriteLog("info", "Application started");
//    var lines = textFile.ReadLines();
//    var list = lines.ToList();
//    var option = string.Empty;

//    do
//    {
//        option = Menu();
//        switch (option)
//        {
//            case "1":
//                foreach (var item in list)
//                {
//                    Console.WriteLine(item);
//                }
//                logger.WriteLog("info", "File listed.");
//                break;
//            case "2":
//                Console.Write("Enter a new line: ");
//                var newLine = Console.ReadLine();
//                list.Add(newLine!);
//                logger.WriteLog("info", $"New line added: {newLine}");
//                break;
//            case "3":
//                Console.Write("Enter a new line: ");
//                var lineToRemove = Console.ReadLine();
//                list.Remove(lineToRemove!);
//                logger.WriteLog("info", $"Line removed: {lineToRemove}");
//                break;
//            case "6":
//                textFile.WriteLines(list.ToArray());
//                Console.WriteLine("Changes saved.");
//                logger.WriteLog("info", "File saved.");
//                break;
//            default:
//                break;
//        }
//    } while (option != "0");
//    textFile.WriteLines(list.ToArray());
//    Console.WriteLine("Changes saved.");

//}
//catch (Exception ex)
//{
//    logger.WriteLog("error", $"An error happened: {ex.Message}.");
//}
//finally
//{
//    logger.WriteLog("info", "Application ended.");
//}

//string Menu()
//{
//    Console.WriteLine("Menu:");
//    Console.WriteLine("1. Show lines");
//    Console.WriteLine("2. Add line");
//    Console.WriteLine("3. Remove one occurrence");
//    Console.WriteLine("4. Remove all occurrences"); // Homework: Implement this option
//    Console.WriteLine("5. Sort"); // Homework: Implement this option
//    Console.WriteLine("6. Save changes");
//    Console.WriteLine("0. Exit");
//    Console.Write("Choose an option: ");
//    return Console.ReadLine() ?? string.Empty;
//}

//======================================================================
// EXAMPLE 2
//======================================================================

//var people = new List<string[]>
//{
//    new[] { "Id", "Name", "Age"},
//    new[] { "1", "Juan", "51"},
//    new[] { "2", "Maria", "46"},
//    new[] { "3", "Valery", "16"},
//};

//var manualCsvHelper = new ManualCsvHelper();
//manualCsvHelper.WriteCSV("c:\\tmp\\people.csv", people);

//var loadedPeople = manualCsvHelper.ReadCSV("c:\\tmp\\people.csv");
//foreach (var person in loadedPeople)
//{
//    Console.WriteLine(string.Join("|", person));
//}

// Homework: Implement a CSV helper with a menu.

//======================================================================
// EXAMPLE 3
//======================================================================

//using Core;

//var list = new List<Person>()
//{
//    new() { Id = 1, Name = "Juan", Age = 51 },
//    new() { Id = 2, Name = "Maria", Age = 46 },
//    new() { Id = 3, Name = "Valery", Age = 16 },
//    new() { Id = 4, Name = "Sofia", Age = 14 },
//    new() { Id = 5, Name = "Carlos", Age = 20 },    
//};

//var helper = new CsvHelperExample();
//helper.Write("c:\\tmp\\people2.csv", list);
//var people = helper.Read("c:\\tmp\\people2.csv");

//foreach (var p in people)
//{
//    Console.WriteLine($"{p.Id}: {p.Name} ({p.Age} years old)");  
//}

//======================================================================
// EXAMPLE 4
//======================================================================

using Core;
using System.Data;
using System.Reflection.Metadata;

var items = new List<FixedWidthRecord>
{
    new() { Code = "A1001", Description = "iPhone 17 Pro MAx", Category = "Smartphone", Quantity = 5, Price = 1345.43M },
    new() { Code = "B2002", Description = "iWatch Series 15", Category = "Wearable", Quantity = 8, Price = 678.45M },
};

var products = new (string Description, string Category)[]
{
    ("iPhone 17 Pro", "Smartphone"), ("iPhone 16", "Smartphone"), ("iPad Pro M4", "Tablet"), ("iPad Air", "Tablet"), ("MacBook Pro 16", "Laptop"),
    ("MacBook Air M3", "Laptop"), ("iMac 24 M4", "Desktop"), ("Mac Mini M4", "Desktop"), ("Mac Studio M3", "Desktop"), ("AirPods Pro 3", "Audio"),
    ("AirPods Max", "Audio"), ("Apple TV 4K", "Streaming"), ("HomePod mini", "Audio"), ("Samsung S25 Ultra", "Smartphone"), ("Samsung S25", "Smartphone"),
    ("Galaxy Z Fold 6", "Smartphone"), ("Galaxy Z Flip 6", "Smartphone"), ("Galaxy Tab S10", "Tablet"), ("Galaxy Buds 3", "Audio"), ("Galaxy Watch 7", "Wearable"),
    ("Pixel 9 Pro", "Smartphone"), ("Pixel 9", "Smartphone"), ("Pixel Watch 3", "Wearable"), ("Pixel Buds Pro", "Audio"), ("Xiaomi 14 Pro", "Smartphone"),
    ("Redmi Note 13", "Smartphone"), ("Huawei P60 Pro", "Smartphone"), ("OnePlus 12", "Smartphone"), ("Sony WH-1000XM5", "Audio"), ("Sony PS5 Pro", "Gaming"),
    ("Sony PS5", "Gaming"), ("Xbox Series X", "Gaming"), ("Xbox Series S", "Gaming"), ("Nintendo Switch 2", "Gaming"), ("Steam Deck OLED", "Gaming"),
    ("Meta Quest 3", "VR"), ("PS VR2", "VR"), ("Dell XPS 15", "Laptop"), ("Dell XPS 13", "Laptop"), ("HP Spectre x360", "Laptop"),
    ("ThinkPad X1 Carbon", "Laptop"), ("Lenovo Legion 7", "Laptop"), ("Asus ROG Strix", "Laptop"), ("Asus Zenbook 14", "Laptop"), ("MSI Stealth 16", "Laptop"),
    ("Razer Blade 15", "Laptop"), ("Surface Pro 11", "Tablet"), ("Surface Laptop 7", "Laptop"), ("LG Gram 17", "Laptop"), ("Acer Predator", "Laptop"),
    ("Logitech MX Master 3", "Periferico"), ("Logitech G Pro X", "Periferico"), ("Razer DeathAdder V3", "Periferico"), ("Corsair K100", "Periferico"), ("SteelSeries Apex", "Periferico"),
    ("HyperX Cloud 3", "Audio"), ("Bose QC Ultra", "Audio"), ("JBL Flip 6", "Audio"), ("Marshall Stanmore", "Audio"), ("Sonos Era 300", "Audio"),
    ("Beats Studio Pro", "Audio"), ("Garmin Fenix 7", "Wearable"), ("Fitbit Charge 6", "Wearable"), ("GoPro Hero 13", "Camara"), ("DJI Mini 4 Pro", "Drone"),
    ("DJI Mavic 3", "Drone"), ("DJI Osmo Pocket 3", "Camara"), ("Canon EOS R5", "Camara"), ("Sony A7 IV", "Camara"), ("Nikon Z8", "Camara"),
    ("Fujifilm X-T5", "Camara"), ("Samsung TV QN90D", "TV"), ("LG OLED C4", "TV"), ("Sony Bravia A95L", "TV"), ("TCL QM8 Mini LED", "TV"),
    ("Hisense U8N", "TV"), ("Roku Ultra", "Streaming"), ("Chromecast 4K", "Streaming"), ("Fire TV Stick", "Streaming"), ("NVIDIA RTX 4090", "Componente"),
    ("NVIDIA RTX 4080", "Componente"), ("AMD RX 7900 XTX", "Componente"), ("Intel i9-14900K", "Componente"), ("AMD Ryzen 9 7950X", "Componente"), ("Samsung 990 Pro", "Almacenamiento"),
    ("WD Black SN850X", "Almacenamiento"), ("Crucial T705", "Almacenamiento"), ("Seagate FireCuda", "Almacenamiento"), ("Kingston Fury", "Componente"), ("Corsair Vengeance", "Componente"),
    ("ASUS ROG Swift", "Monitor"), ("LG UltraGear 27", "Monitor"), ("Alienware AW3423", "Monitor"), ("Odyssey G9", "Monitor"), ("BenQ Mobiuz", "Monitor"),
    ("Anker PowerCore", "Accesorio"), ("UGREEN Nexode", "Accesorio"), ("Belkin BoostCharge", "Accesorio"), ("Tile Pro Tracker", "Accesorio"), ("Apple AirTag", "Accesorio")
};

var random = new Random();
for (int j = 0; j < products.Length; j++)
{
    items.Add(new FixedWidthRecord
    {
        Code = $"P{j + 1:D4}",
        Description = products[j].Description,
        Category = products[j].Category,
        Quantity = random.Next(1, 11),
        Price = random.Next(5000, 200000) / 100
    });
}

items = items.OrderBy(x => x.Category).ToList();

var path = "c:\\tmp\\products.txt";
var example = new FixedWidthExample();
example.Write(path, items);

var loadedItems = example.Read(path);
decimal total = 0;
Console.WriteLine($"{"Code",-5} {"Description",-40} {"Category",-15} {"Qty",5} {"Price",20} {"Value",20}");
Console.WriteLine($"===== ======================================== =============== ===== ==================== ====================");

var i = 0;
while (i < loadedItems.Count)
{
    int totalRecords = 0;
    decimal totalCategory = 0;
    var previousCategory = loadedItems[i].Category;

    while (i < loadedItems.Count && previousCategory == loadedItems[i].Category)
    {
        Console.WriteLine($"{loadedItems[i].Code,-5} " +
                        $"{loadedItems[i].Description,-40} " +
                        $"{loadedItems[i].Category,-15} " +
                        $"{loadedItems[i].Quantity,5} " +
                        $"{loadedItems[i].Price,20:C2} " +
                        $"{loadedItems[i].Value,20:C2}");
        totalRecords++;
        totalCategory += loadedItems[i].Value;
        i++;
    }

    Console.WriteLine($"----- ---------------------------------------- --------------- ----- -------------------- --------------------");
    Console.WriteLine($"Total category: {previousCategory,-15},                   number of products: {totalRecords,10:N0}, value: {totalCategory,20:C2}");
    Console.WriteLine($"===== ======================================== =============== ===== ==================== ====================");
    total += totalCategory;
}

Console.WriteLine($"                                                                                    TOTAL {total,20:C2}");