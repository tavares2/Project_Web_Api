using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using FluentValidation;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Repositories;
using LGC_CodeChallenge.Services;
using LGC_CodeChallenge.Validators;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var localStackUrl = "http://localhost:4566";
var localStackRegion = "eu-west-1";

if (builder.Environment.IsDevelopment())
{
    var awsOptions = builder.Configuration.GetAWSOptions();
    awsOptions.DefaultClientConfig.ServiceURL = localStackUrl;
    awsOptions.DefaultClientConfig.AuthenticationRegion = localStackRegion;
    builder.Services.AddDefaultAWSOptions(awsOptions);
}

// Add AWS DynamoDB Client
//builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
//{
//    var configuration = sp.GetRequiredService<IConfiguration>();
//
//
//    // Extract AWS region and service URL from configuration
//    var region = configuration.GetSection("AWS:Region").Value;
//    
//
//    // Set the credentials (for LocalStack or AWS)
//    var awsCredentials = new BasicAWSCredentials(
//         configuration.GetSection("AWS:Credentials:AccessKeyId").Value,
//         configuration.GetSection("AWS:Credentials:SecretAccessKey").Value
//     );
//
//    // Set the Service URL for LocalStack if available
//    var serviceUrl = configuration.GetSection("AWS:ServiceURL").Value;
//
//    Console.WriteLine($"DynamoDB Service URL: {serviceUrl}");
//
//
//    // Set the Service URL (for LocalStack or other custom endpoints)
//    var clientConfig = new AmazonDynamoDBConfig
//    {
//        ServiceURL = serviceUrl,  // Set LocalStack or custom service URL
//        RegionEndpoint = RegionEndpoint.EUWest1
//    };
//    
//
//        return new AmazonDynamoDBClient(awsCredentials, clientConfig);
//    
//    
//}
//    );

builder.Services.AddAWSService<IAmazonDynamoDB>();

// Add AppDynamoDBContext
builder.Services.AddScoped<AppDynamoDBContext>();

// Add services
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add validators
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();


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



app.Run();


