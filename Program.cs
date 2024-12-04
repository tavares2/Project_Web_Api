using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Services;
using LGC_CodeChallenge.Validators;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AWS DynamoDB Client
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();


    // Extract AWS region and service URL from configuration
    var region = configuration["AWS:Region"];
    

    // Set the credentials (for LocalStack or AWS)
    var awsCredentials = new BasicAWSCredentials(
         configuration["AWS:Credentials:AccessKeyId"],
         configuration["AWS:Credentials:SecretAccessKey"]
     );

    // Set the Service URL for LocalStack if available
    var serviceUrl = configuration["AWS:ServiceURL"];
    
        // Set the Service URL (for LocalStack or other custom endpoints)
        var clientConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = configuration["AWS:ServiceURL"],  // Set LocalStack or custom service URL
            UseHttp = !string.IsNullOrEmpty(serviceUrl)
        };
    

        return new AmazonDynamoDBClient(awsCredentials, clientConfig);
    
    
}
    );

// Add AppDynamoDBContext
builder.Services.AddScoped<AppDynamoDBContext>();

// Add utility for managing tables
builder.Services.AddScoped<DynamoDBTableManager>();

// Add services
builder.Services.AddScoped<ProductService>();

// Add validators
builder.Services.AddScoped<ProductValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//Call TestDynamoDBTableManager 
await TestDynamoDBTableManager(app.Services);

app.Run();

//Test Method to Ensure DynamoDB Table Exists
async Task TestDynamoDBTableManager(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var client = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();
        var tableManager = scope.ServiceProvider.GetRequiredService<DynamoDBTableManager>();

        try
        {
            //Ensure the Model table exists (creates it if it doesn't exist)
            await tableManager.EnsureTableExistsAsync<Product>();
            Console.WriteLine("Table has been created.");

            //Table list to confirm that the Product table was created
            var listTablesRequest = new ListTablesRequest();
            var listTablesResponse = await client.ListTablesAsync(listTablesRequest);

            Console.WriteLine("Tables in DynamoDB:");
            foreach (var table in listTablesResponse.TableNames)
            {
                Console.WriteLine(table);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}