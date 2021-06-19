using System;

namespace AzureTableStorageIndexing
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IndexedPropertyAttribute : Attribute
    { }
}
