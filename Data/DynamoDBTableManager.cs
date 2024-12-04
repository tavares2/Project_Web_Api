using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Reflection;

namespace LGC_CodeChallenge.Data
{
    public class DynamoDBTableManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public DynamoDBTableManager(IAmazonDynamoDB dynamoDBClient) 
        {
            _dynamoDbClient = dynamoDBClient;
        }

        public async Task EnsureTableExistsAsync<T>() where T : class
        {
            //Retrieve the table name from the [DynamoDBTable] attribute
            var tableName = GetTableName<T>();
            if (tableName == null) 
            {
                throw new InvalidOperationException($"Class {typeof(T).Name} does not have a [DynamoDBTable] attribute");
            }

            //Check if the table already exists
            var existingTables = await _dynamoDbClient.ListTablesAsync();
            if (existingTables.TableNames.Contains(tableName))
            {
                Console.WriteLine($"Table {tableName} already exists.");
                return;
            }

            //Define the table schema based on attributes
            var createTableRequest = BuildCreateTableRequest<T>(tableName);

            //Create the table
            var createTableResponse = await _dynamoDbClient.CreateTableAsync(createTableRequest);
            Console.WriteLine($"Table {tableName} created successfully.");
        }

        private static string GetTableName<T>() where T : class
        {
            //Look for the [DynamoDBTable] attribute
            var tableAttribute = typeof(T).GetCustomAttribute<DynamoDBTableAttribute>();
            return tableAttribute?.TableName;
        }

        private static CreateTableRequest BuildCreateTableRequest<T>(string tableName) where T : class
        {
            //Retrieve the key schema from the attributes
            var hashKeyProperty = GetHashKeyProperty<T>();
            if (hashKeyProperty == null)
            {
                throw new InvalidOperationException($"Class {typeof(T).Name} must have a property with [DynamoDBHashKey].");

            }

            var hashKeyName = hashKeyProperty.Name;
            var hashKeyType = GetScalarAttributeType(hashKeyProperty.PropertyType);

            var createTableRequest = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition(hashKeyName, hashKeyType)
            },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement(hashKeyName, KeyType.HASH)
            },
               
            };

            // Add other properties marked with [DynamoDBProperty] to AttributeDefinitions
            var dynamoDbProperties = GetDynamoDbProperties<T>();
            foreach (var prop in dynamoDbProperties)
            {
                var attributeName = prop.Name;
                var attributeType = GetScalarAttributeType(prop.PropertyType);
                createTableRequest.AttributeDefinitions.Add(new AttributeDefinition(attributeName, attributeType));
            }

            return createTableRequest;
        }

       

        private static PropertyInfo GetHashKeyProperty<T>() where T : class
        {
            // Look for the property with [DynamoDBHashKey]
            return typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<DynamoDBHashKeyAttribute>() != null);
        }

        private static List<PropertyInfo> GetDynamoDbProperties<T>() where T : class
        {
            // Look for properties with [DynamoDBProperty]
            return typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<DynamoDBPropertyAttribute>() != null)
                .ToList();
        }

        private static ScalarAttributeType GetScalarAttributeType(Type type)
        {
            // Map .NET types to DynamoDB scalar attribute types
            if (type == typeof(string)) return ScalarAttributeType.S;
            if (type == typeof(Guid)) return ScalarAttributeType.S; // DynamoDB does not natively support Guid
            if (type == typeof(int) || type == typeof(long)) return ScalarAttributeType.N;
            if (type == typeof(decimal) || type == typeof(double)) return ScalarAttributeType.N;
            if (type == typeof(byte[])) return ScalarAttributeType.B;

            throw new NotSupportedException($"Type {type.Name} is not supported for DynamoDB attributes.");
        }
    }
}
