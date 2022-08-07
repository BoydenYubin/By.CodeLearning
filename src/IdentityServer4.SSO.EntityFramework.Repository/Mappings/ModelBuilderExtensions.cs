using ByLearning.EntityFrameworkCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ByLearning.SSO.EntityFramework.Repository.Mappings
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureEventStoreContext(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoredEventMap());
            builder.ApplyConfiguration(new EventDetailsMap());
        }

        public static void ConfigureSSOContext(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmailMap());
            builder.ApplyConfiguration(new TemplateMap());
        }
    }
}
