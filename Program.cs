using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using FluentValidation;
using FluentValidation.AspNetCore;
using LGC_CodeChallenge.Contracts;
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


builder.Services.AddAWSService<IAmazonDynamoDB>();

// Add AppDynamoDBContext
builder.Services.AddScoped<AppDynamoDBContext>();

// Add services
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add validators

builder.Services.AddFluentValidationAutoValidation() // Enables server-side validation
                .AddFluentValidationClientsideAdapters(); // Enables client-side validation (optional)

// Register all validators in the same assembly as ProductRequestValidator
builder.Services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();


builder.Services.AddAutoMapper(typeof(Program).Assembly);


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


