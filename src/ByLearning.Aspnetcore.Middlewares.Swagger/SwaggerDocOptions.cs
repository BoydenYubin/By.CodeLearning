using System;

namespace ByLearning.Aspnetcore.Middlewares.Swagger
{
    public class SwaggerDocOptions
    {
        public static SwaggerDocOptions Default => new SwaggerDocOptions()
        {
            Version = "1.0.0",
            Title = "Default Swagger API Docs",
            Description = "Default Swagger API Docs, if you want to chanage it, make new object as SwaggerDocOptions",
            TermsOfService = new Uri("http://sample.TermsOfService.com"),
            Contact = new SwaggerDocContactOptions()
            {
                Name = "ByLearning",
                Email = "bylearning@gmail.com",
                Uri = new Uri("www.github.com/bylearning")
            },
            License = new SwaggerDocLicenseOptions()
            {
                Name = "ByLearning",
                Uri = new Uri("www.github.com/bylearning")
            }
        };
        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Uri TermsOfService { get; set; }
        public SwaggerDocContactOptions Contact { get; set; }

        public SwaggerDocLicenseOptions License { get; set; }
    }

    public class SwaggerDocContactOptions
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Uri { get; set; }
    }

    public class SwaggerDocLicenseOptions
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}
