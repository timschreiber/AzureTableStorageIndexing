using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageIndexing
{
    public class IndexedRepository<T> where T : IndexedTableEntity, new()
    {
        public IndexedRepository(IOptionsMonitor<IndexedRepositoryOptions> options)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(options.CurrentValue.StorageAccountConnectionString);
            var _cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var tableName = typeof(T).Name;
            CloudTable = _cloudTableClient.GetTableReference(tableName);
            CloudTable.CreateIfNotExists();
        }

        protected CloudTable CloudTable { get; }

        public virtual T GetByIdAsync(string id, string partitionKey = null)
        {
            var q = CloudTable.CreateQuery<T>();
            if (!string.IsNullOrWhiteSpace(partitionKey))
                q.Where(x => x.PartitionKey == partitionKey);
            q.Where(x => x.RowKey == $"{typeof(T).Name}_Id_{id}");
            return q.Execute().FirstOrDefault();
        }

        //public async IEnumerable<Task<T>> GetByPropertyName(string partitionKey, string propertyName, string propertyValue)
        //{
        //    IndexedFilterHelper.GetFilterCondition()
        //}


        //protected string GetFilter<T>(Expression<Func<T, bool>> e)
        //{
        //    BinaryExpression expression = e.Body as BinaryExpression;

        //    var left = (expression.Left as MemberExpression).Member.Name;
        //    var right1 = expression.Right;
        //    var operator = right1.lefft

        //    var ee = expression.Operator
        //    var ww = expression.Right;


        //    string name = (expression.Left as MemberExpression).Member.Name;
        //    string value = expression.Right.NodeType == ExpressionType.

        //}

        private Expression<Func<T, bool>> mutateExpression()


        private class FilterVisitor : ExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression node)
            {
                Console.WriteLine("Constant: {0}", node);
                return base.VisitConstant(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Console.WriteLine("Parameter: {0}", node);
                return base.VisitParameter(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                Console.WriteLine("Binary with operator {0}", node.NodeType);
                return base.VisitBinary(node);
            }
        }



    }











} 
