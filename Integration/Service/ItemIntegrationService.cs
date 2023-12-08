using Integration.Common;
using Integration.Backend;
using System.Collections.Concurrent;

namespace Integration.Service;

public sealed class ItemIntegrationService
{
    //This is a dependency that is normally fulfilled externally.
    private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();

    /// There are many ways to handle these needs. We can use Locks and Hashset, Concurrent Dictionary,
    /// or Lazy Initialization. All of them have pros and cons. I'll pick the Concurrent Dictionary because
    /// it's more suitable to handle high traffic, and I won't need to work on locking as well.
    
    private readonly ConcurrentDictionary<string, bool> savedItems = new();

    // This is called externally and can be called multithreaded, in parallel.
    // More than one item with the same content should not be saved. However,
    // calling this with different contents at the same time is OK, and should
    // be allowed for performance reasons.
    public Result SaveItem(string itemContent)
    {
        if (savedItems.TryAdd(itemContent, true))
        {
            var item = ItemIntegrationBackend.SaveItem(itemContent);

            return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
        }

        return new Result(false, $"Duplicate item received with content {itemContent}.");
    }

    public List<Item> GetAllItems()
    {
        return ItemIntegrationBackend.GetAllItems();
    }
}