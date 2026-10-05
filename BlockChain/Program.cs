using BlockChain.Services;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var testBlockChain = new BlockChainService(2, 30000, 10);
var displayService = new DisplayService();

Console.WriteLine("Створюємо блокчейн із 5 блоків...\n");
for (int i = 1; i <= 4; i++)
{
    testBlockChain.AddBlock($"Data {i}", $"Author {i}");
}

displayService.ShowChain(testBlockChain.Chain);

// 1. Дати користувачу можливість змінити дані одного блоку
Console.WriteLine("\n--- КРОК 1: Зміна даних блоку #2 ---\n");
testBlockChain.ModifyBlockData(2, "Hacked Data!");

displayService.ShowChain(testBlockChain.Chain);

// 2. Показати, що після зміни Blockchain став INVALID
bool isValidAfterMod = testBlockChain.IsValid();
Console.WriteLine($"\n--- КРОК 2: Перевірка валідності ---");
Console.WriteLine($"Blockchain is valid: {(isValidAfterMod ? "valid" : "invalid")}\n");

displayService.ShowChain(testBlockChain.Chain);

// 3 & 4. Показати, що перемайнінгу лише одного блоку недостатньо
Console.WriteLine("\n--- КРОК 3 & 4: Перемайнюємо тільки змінений блок #2 ---\n");
testBlockChain.RemineSingleBlock(2);

displayService.ShowChain(testBlockChain.Chain);

bool isValidAfterSingleRemine = testBlockChain.IsValid();
Console.WriteLine($"Blockchain is valid після перемайнінгу лише блоку #2: {(isValidAfterSingleRemine ? "valid" : "invalid")}");
Console.WriteLine("(Ланцюг усе ще невалідний, оскільки наступні блоки посилаються на старий хеш попередника!)");

// 5. Вивести час і кількість спроб для відновлення ланцюга за допомогою RepairChain()
Console.WriteLine("\n--- КРОК 5: Використання RepairChain() для повного відновлення ---");
var swTotal = Stopwatch.StartNew();
var result = testBlockChain.RepairChain();
swTotal.Stop();

bool isValidFinal = testBlockChain.IsValid();
Console.WriteLine($"\nЗагальний час відновлення: {result.TotalTime:F4} сек");
Console.WriteLine($"Загальна кількість спроб (Nonce): {result.TotalAttempts}");
Console.WriteLine($"Blockchain is valid після RepairChain: {(isValidFinal ? "valid" : "invalid")}\n");

displayService.ShowChain(testBlockChain.Chain);


//using BlockChain.Services;
//using System.Diagnostics;
//using System.Text;

//Console.OutputEncoding = Encoding.UTF8;
//var displayService = new DisplayService();

//var testBlockChain = new BlockChainService(1,3,2);

//while (true)
//{
//    Console.WriteLine("1. Add Block");
//    Console.WriteLine("2. Display BlockChain");
//    Console.WriteLine("3. Validate BlockChain");
//    Console.WriteLine("4. СТАТИСТИКА МАЙНІНГУ");
//    Console.WriteLine("5. Exit");

//    var choice = Console.ReadLine();

//    switch (choice)
//    {
//        case "1":
//            testBlockChain.AddBlock("test data", "Oleg");
//            break;
//        case "2":
//            displayService.ShowChain(testBlockChain.Chain);
//            break;
//        case "3":
//            bool isValid = testBlockChain.IsValid();
//            Console.WriteLine($"Blockchain is valid: {(isValid?"valid":"invalid")}");
//            break;
//        case "4":
//            var blocks = testBlockChain.Chain.Where(b => b.Index > 0).ToList();



//            var count = blocks.Count;
//            var min = blocks.MinBy(b => b.MiningDuration);
//            var max = blocks.MaxBy(b => b.MiningDuration);
//            var maxNonce = blocks.MaxBy(b => b.Nonce);
//            double avgTime = blocks.Average(b => b.MiningDuration);
//            double avgAttempts = blocks.Average(b => b.Nonce);
//            int minDif = blocks.Min(b => b.Difficulty);
//            int maxDif = blocks.Max(b => b.Difficulty);

//            Console.WriteLine("\n=== СТАТИСТИКА МАЙНІНГУ ===");
//            Console.WriteLine($"Загальна кількість замайнених блоків: {count}");
//            Console.WriteLine($"Найшвидший блок: #{min.Index} ({min.MiningDuration:f2} сек)");
//            Console.WriteLine($"Найповільніший блок: #{max.Index} ({max.MiningDuration:f2} сек)");
//            Console.WriteLine($"Блок з найбільшою кількістю Attempts: #{maxNonce.Index} ({maxNonce.Nonce})");
//            Console.WriteLine($"Середній час майнінгу: {avgTime:f2} сек");
//            Console.WriteLine($"Середня кількість спроб: {avgAttempts:f2}");
//            Console.WriteLine($"Мінімальна Difficulty: {minDif}");
//            Console.WriteLine($"Максимальна Difficulty: {maxDif}");
//            Console.WriteLine(new string('-', 40) + "\n");
//            break;
//        case "5":
//            return;
//        default:
//            Console.WriteLine("Invalid choice. Please try again.");
//            break;
//    }
//}




//using BlockChain.Services;
//using System.Diagnostics;

//var displayService = new DisplayService();
//int difficulty = 1;
//var testBlockChain = new BlockChainService(difficulty);

//while (true)
//{
//    Console.Clear();
//    Console.WriteLine("1. Change Difficulty");
//    Console.WriteLine("2. AddBlock");
//    Console.WriteLine("3. ShowChain");
//    Console.WriteLine("4. Exit");

//    switch (Console.ReadLine())
//    {
//        case "1":
//            {
//                Console.Write("Difficulty (1-5): ");
//                difficulty = int.Parse(Console.ReadLine() ?? "1");
//                testBlockChain = new BlockChainService(difficulty);
//            }
//                break;


//        case "2":
//            {
//                var sw = Stopwatch.StartNew();
//                testBlockChain.AddBlock($"Test Data for difficulty {difficulty}", "Oleg");
//                sw.Stop();
//                Console.WriteLine($"Mined in {sw.ElapsedMilliseconds} ms");
//            }
//            break;

//        case "3":
//            {
//                displayService.ShowChain(testBlockChain.Chain);
//            }
//            break;

//        case "4":
//            return;
//    }

//    Console.WriteLine("\nPress any key...");
//    Console.ReadKey();
//}