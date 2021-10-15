using Microsoft.Azure.Cosmos.Table;

namespace AzureTableStorageIndexing
{
    public abstract class IndexedTableEntity : TableEntity
    {
        [IndexedProperty]
        public string Id { get; set; }
    }
}
