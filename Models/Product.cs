using Amazon.DynamoDBv2.DataModel;
namespace LGC_CodeChallenge.Models

{
    [DynamoDBTable("ProductTable")]
    public class Product
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public decimal Price { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public int Stock {  get; set; }
    }
}
