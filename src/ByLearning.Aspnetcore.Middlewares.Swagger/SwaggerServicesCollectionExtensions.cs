using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ByLearning.Aspnetcore.Middlewares.Swagger
{
    public static class SwaggerServicesCollectionExtensions
    {
        /// <summary>
        /// If you want to ignore some API, you should add attribute [ApiExplorerSettings(IgnoreApi = true)] on your API
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services, [AllowNull] SwaggerDocOptions options)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("", new OpenApiInfo()
                {
                    Version = options?.Version ?? SwaggerDocOptions.Default.Version,
                    Title = options?.Title ?? SwaggerDocOptions.Default.Title,
                    Description = options?.Description ?? SwaggerDocOptions.Default.Description,
                    TermsOfService = options?.TermsOfService ?? SwaggerDocOptions.Default.TermsOfService,
                    License = new OpenApiLicense
                    {
                        Name = options?.License?.Name ?? SwaggerDocOptions.Default.License.Name,
                        Url = options?.License?.Uri ?? SwaggerDocOptions.Default.License.Uri
                    },
                    Contact = new OpenApiContact()
                    {
                        Name = options?.Contact?.Name ?? SwaggerDocOptions.Default.Contact.Name,
                        Email = options?.Contact?.Email ?? SwaggerDocOptions.Default.Contact.Email,
                        Url = options?.Contact?.Uri ?? SwaggerDocOptions.Default.Contact.Uri
                    }
                }); ;
                //Should ignore the suppress warnings of 1591
                //add that on Visualstudio to ignore the warnings
                var basePath = AppContext.BaseDirectory;
                //controller xml path, to add comments
                var xmlPath = Path.Combine(basePath, "");
                config.IncludeXmlComments(xmlPath);
                //model xml path, to add comments for model
                var modelPath = Path.Combine(basePath, "");
                config.IncludeXmlComments(modelPath);

                #region Oauth if use
                //config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        Implicit = new OpenApiOAuthFlow()
                //        {
                //            AuthorizationUrl = new Uri($""),
                //            TokenUrl = new Uri(""),
                //            Scopes = new Dictionary<string, string>()
                //            {
                //                { "XXX", "XXXXX" }
                //            }
                //        }
                //    }
                //});
                //config.OperationFilter<AuthorizeCheckOperationFilter>();
                #endregion

            });
            return services;
        }
        /// <summary>
        /// Only deploy the swagger UI in development environment!
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerUIMiddle(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    // c.IndexStream to change the index.html for swagger
                    c.RoutePrefix = "";  //if the route path was set empty string, webUI can viewed by base Url+index.html
                });
            }
            return app;
        }
    }
}
