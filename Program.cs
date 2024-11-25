using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Services;
using LGC_CodeChallenge.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AWS DynamoDB Client
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

// Add custom DynamoDBContext
builder.Services.AddScoped<IDynamoDBContext, LGC_CodeChallenge.Data.DynamoDBContext>();

//Add utility for managing tables
builder.Services.AddScoped<DynamoDBTableManager>();

//Add services
builder.Services.AddScoped<ProductService>();

//Add validators
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

app.Run();
