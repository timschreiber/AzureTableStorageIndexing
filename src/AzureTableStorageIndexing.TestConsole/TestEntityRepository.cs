using AzureTableStorageIndexing.TestConsole.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableStorageIndexing.TestConsole
{
    public class TestEntityRepository : IndexedRepository<TestEntity>
    {
        public TestEntityRepository(IOptionsMonitor<IndexedRepositoryOptions> options)
            : base(options)
        { }

        public void foo()
        {
            CloudTableClient.
        }
    }
}
