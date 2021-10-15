using AzureTableStorageIndexing.Attributes;
using AzureTableStorageIndexing.TestConsole.Enums;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableStorageIndexing.TestConsole.Entities
{
    public class TestEntity : IndexedTableEntity
    {
        public TestEntity()
            : base()
        { }

        public TestEntity(Guid id)
        {
            PartitionKey = id.ToString();
            RowKey = id.ToString();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [IndexedProperty]
        public string Phase { get; set; }

        [IndexedProperty]
        public NotificationPreference NotificationPreference { get; set; }

        public string Description { get; set; }
    }
}
