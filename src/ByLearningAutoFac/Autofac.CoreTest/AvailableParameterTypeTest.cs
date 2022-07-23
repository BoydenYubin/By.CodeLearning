using Autofac;
using Autofac.Core;
using Shouldly;
using System;
using Xunit;

namespace ByLearningAutoFac
{
    #region Base Class for Test
    public interface IConfigReader
    {
        Guid Guid { get; set; }
        string ConfigSectionName { get; set; }
    }
    public class ConfigReader : IConfigReader
    {
        public Guid Guid { get; set; }
        public string ConfigSectionName { get; set; }
        public ConfigReader(string configSectionName)
        {
            this.ConfigSectionName = configSectionName;
        }
        public ConfigReader(string configSectionName, Guid guid)
        {
            // Store config section name
            this.ConfigSectionName = configSectionName;
            this.Guid = guid;
        }
        // ...read configuration based on the section name.
    }
    #endregion
    public class AvailableParameterTypeTest
    {
        private ContainerBuilder builder;
        public AvailableParameterTypeTest()
        {
            builder = new ContainerBuilder();
        }

        [Fact]
        public void RegisterWithNamedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>()
                .WithParameter("configSectionName", "sectionName");
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>();
            reader.GetType().ShouldBe(typeof(ConfigReader));
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
        [Fact]
        public void ResolveWithNamedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>(new NamedParameter("configSectionName", "sectionName"));
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
        [Fact]
        public void RegisterWithTypedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>()
                .WithParameter(new TypedParameter(typeof(string), "sectionName"));
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>();
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
        [Fact]
        public void ResolveWithTypedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>(new TypedParameter(typeof(string), "sectionName"));
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
        [Fact]
        public void RegisterWithResolvedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "configSectionName",
                    (pi, ctx) => "sectionName"));
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>();
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
        [Fact]
        public void ResolveWithResolvedParameterTest()
        {
            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            var container = builder.Build();
            var reader = container.Resolve<IConfigReader>(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "configSectionName",
                    (pi, ctx) => "sectionName"));
            reader.ConfigSectionName.ShouldBe("sectionName");
        }
    }
}
