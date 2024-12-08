using AOC2024;
using System.Reflection;

var puzzels = Assembly.GetAssembly(typeof(PuzzelBase))!.GetTypes()
    .Where(t => t.IsSubclassOf(typeof(PuzzelBase)))
    .Select(t => (PuzzelBase)Activator.CreateInstance(t)!)
    .OrderBy(p => p.Day);

// Run the last Puzzel
puzzels.Last().Run();