﻿using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Components;
using Humanizer;

namespace StockApp.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => { options.IncludeXmlComments(xmlFile); });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StockApp API",
                    Description = "This Api will be responsible for overall data distribution",
                    Contact= new OpenApiContact
                    {
                        Name="Rafael Gomez",
                        Email="rafaelogomezr@gmail.com",
                        Url = new Uri("https://www.itla.edu.do")
                    }
                });

                //To enable swagger documentationn
                options.EnableAnnotations();

                options.DescribeAllParametersInCamelCase();  

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your bearer token in this format - Bearer {your token here}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="Bearer",
                            Name= "Bearer",
                            In = ParameterLocation.Header
                        },new List<string>()
                    }
                });

            });
        }
        public static void AddApiVersioningExtension(this IServiceCollection services) 
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
        }
    }
}
