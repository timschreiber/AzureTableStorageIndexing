using AzureTableStorageIndexing.Attributes;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableStorageIndexing.TestConsole.Entities
{
    public class TestEntity : TableEntity
    {
        public TestEntity()
            : base()
        { }

        public TestEntity(Guid id)
        {
            PartitionKey = id.ToString();
            RowKey = id.ToString();
        }

        public Guid Id => Guid.Parse(PartitionKey);

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [CloudIndex(Name = "Phase")]
        [CloudIndex(Name = "PhaseAndNotificationPreference")]
        public string Phase { get; set; }

        [CloudIndex(Name = "NotificationPreference")]
        [CloudIndex(Name = "PhaseAndNotificationPreference")]
        public string NotificationPreference { get; set; }

        public string Description { get; set; }
    }
}
