using Microsoft.Azure.Cosmos.Table;

namespace AzureTableStorageIndexing
{
    public abstract class IndexedTableEntity : TableEntity
    {
        public string Id { get; set; }
    }
}
