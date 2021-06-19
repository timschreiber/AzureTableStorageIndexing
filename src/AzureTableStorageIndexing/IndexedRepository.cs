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
        private CloudTable _cloudTable;

        public IndexedRepository(IOptionsMonitor<IndexedRepositoryOptions> options)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(options.CurrentValue.StorageAccountConnectionString);
            CloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var tableName = typeof(T).Name;
            _cloudTable = CloudTableClient.GetTableReference(tableName);
            _cloudTable.CreateIfNotExists();
        }

        protected CloudTableClient CloudTableClient { get; }

        //public virtual async Task<T> GetByIdAsync(string partitionKey, string id)
        //{

        //}

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
