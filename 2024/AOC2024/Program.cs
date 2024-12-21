using AOC2024;
using System.Reflection;

var puzzels = Assembly.GetAssembly(typeof(PuzzelBase))!.GetTypes()
    .Where(t => t.IsSubclassOf(typeof(PuzzelBase)))
    .Select(t => (PuzzelBase)Activator.CreateInstance(t)!)
    .OrderBy(p => p.Day);

if (args.Length == 1)
{
    var day = int.Parse(args[0]);
    puzzels.First(p => p.Day == day).Run();
    return;
}

// Run the last Puzzel
puzzels.Last().Run();