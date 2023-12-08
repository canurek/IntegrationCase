using Integration.Service;

namespace Integration;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var service = new ItemIntegrationService();

        var itemsToSave = new string[] { "a", "b", "c", "a", "b", "c" };

        //It's easy to demonstrate working parallelly like that.
        Parallel.ForEach(itemsToSave, item => service.SaveItem(item));

        Console.WriteLine("Everything recorded:");

        service.GetAllItems().ForEach(Console.WriteLine);

        Console.ReadLine();
    }
}