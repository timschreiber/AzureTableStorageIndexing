using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableStorageIndexing
{
    public class CloudIndexEntity : TableEntity
    {
        [IgnoreProperty]
        public string IndexTableName { get; set; }

        [IgnoreProperty]
        public string[] EntityRowKeys { get; set; }

        public string EntityRowKeysValue
        {
            get => string.Join(',', EntityRowKeys);
            set => EntityRowKeys = value.Split(',');
        }
    }
}
