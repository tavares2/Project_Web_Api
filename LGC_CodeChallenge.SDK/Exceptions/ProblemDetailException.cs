﻿using LGC_CodeChallenge.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.SDK.Exceptions
{
    public class ProblemDetailException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Type { get; }
        public string Title { get; }
        public string Detail { get; }
        public string Instance { get; }

        // Constructor that receives the problem details and the HTTP status code
        public ProblemDetailException(HttpStatusCode statusCode, string type, string title, string detail, string instance)
            : base($"HTTP {statusCode}: {title} - {detail}")
        {
            StatusCode = statusCode;
            Type = type;
            Title = title;
            Detail = detail;
            Instance = instance;
        }

        // Static helper method to deserialize problem details from the response
        public static async Task<ProblemDetail> DeserializeAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                // Attempt to deserialize the content to a ProblemDetail object
                var problemDetails = JsonSerializer.Deserialize<ProblemDetail>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Handle case insensitivity
                });

                return problemDetails;
            }
            catch (JsonException ex)
            {
                // Log or handle deserialization errors here if needed
                throw new Exception("Failed to deserialize problem details.", ex);
            }
        }
    }
}
