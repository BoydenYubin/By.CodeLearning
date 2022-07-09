using AutoMapper;
using Shouldly;
using System;
using Xunit;

namespace ByLearningAutoMapper
{
    public class SimpleMapperTest
    {
        /// <summary>
        /// ���Ե���ƥ�䷽��
        /// </summary>
        [Fact]
        public void ForMemberTest()
        {
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<SourceClass, Destination>()
                .ForMember(s => s.DName, opt => opt.MapFrom(s => s.Name))
                .ForMember(s => s.DAge, opt => opt.MapFrom(s => s.Age))
                );
            var mapper = new Mapper(config);
            var dst = mapper.Map<Destination>(new SourceClass { Name = "Boyden", Age = 28 });
            dst.DName.ShouldBe("Boyden");
            dst.DAge.ShouldBe(28);
        }
        /// <summary>
        /// ���Է�ת��Mapper
        /// </summary>
        [Fact]
        public void ReverseMapperTest()
        {
            var config = new MapperConfiguration(
                   cfg => cfg.CreateMap<SourceClass, Destination>()
                   .ForMember(s => s.DName, opt => opt.MapFrom(s => s.Name))
                   .ForMember(s => s.DAge, opt => opt.MapFrom(s => s.Age))
                   .ReverseMap()
                   );
            var mapper = new Mapper(config);
            var src = mapper.Map<SourceClass>(new Destination { DName = "boyden", DAge = 28 });
            src.Name.ShouldBe("boyden");
        }
        /// <summary>
        /// ƥ��ǰ��ƥ���
        /// BeforeMap && AfterMap
        /// </summary>
        [Fact]
        public void BeforeCreateMapTest()
        {
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<SourceClass, Destination>()
                .ForMember(s => s.DName, opt => opt.MapFrom(s => s.Name))
                .ForMember(s => s.DAge, opt => opt.MapFrom(s => s.Age))
                .BeforeMap((s, d) =>
                {
                    s.Age += 1;
                    s.Name += "12";
                })
                .AfterMap((s, d) =>
                {
                    d.DAge++;
                    d.DName += "13";
                }));
            var mapper = new Mapper(config);
            var dst = mapper.Map<Destination>(new SourceClass { Name = "Boyden", Age = 28 });
            dst.DName.ShouldBe("Boyden1213");
            dst.DAge.ShouldBe(30);
        }
        /// <summary>
        /// SourceMemberNamingConvention && DestinationMemberNamingConvention
        /// �����»���_ && ��˹������
        /// LowerUnderscoreNamingConvention  PascalCaseNamingConvention
        /// </summary>
        [Fact]
        public void NamingConventionsTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
                cfg.CreateMap<ConfigSrcClass, ConfigDstClass>();
            });
            var mapper = new Mapper(config);
            var res = mapper.Map<ConfigDstClass>(new ConfigSrcClass { property_name = "12" });
            res.PropertyName.ShouldBe("12");
        }
        /// <summary>
        /// ǰ׺ʹ�÷���
        /// </summary>
        [Fact]
        public void RecognizingPreTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.ClearPrefixes(); //By default AutoMapper recognizes the prefix ��Get��, if you need to clear the prefix:
                cfg.RecognizePrefixes("frm");
                cfg.CreateMap<PreSrcClass, PreDstClass>();
            });
            var mapper = new Mapper(config);
            var res = mapper.Map<PreDstClass>(new PreSrcClass { GetName = "12", frmAge = 23 });
            res.Name.ShouldBe("12");
            res.Age.ShouldBe(23);
        }

        [Fact]
        public void GlobalPropertyTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                // don't map any fields
                cfg.ShouldMapField = fi => false;
                // map properties with a public or private getter
                cfg.ShouldMapProperty = pi =>
                    pi.GetMethod != null && (pi.GetMethod.IsPublic || pi.GetMethod.IsPrivate);
            });
        }
        /// <summary>
        /// ƥ�����
        /// </summary>
        [Fact]
        public void ConfigValidationTest()
        {
            //����Map��Ĭ��ʹ�õ���MemberList.Destination
            //������MemberList.Source��MemberList.None
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ConfigValidationSrc, ConfigValidationDst>(MemberList.Destination));
            try
            {
                //ʹ��AssertConfigurationIsValid���������MemberList�������Ŀ��
                //�������������û�б�ƥ�䵽����û��ƥ��ľͻ��׳��쳣
                configuration.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                ex.GetType().ShouldBe(typeof(AutoMapperConfigurationException));
            }
            //Ҳ������ForMember��ʹ��opt => opt.Ignore()�����Ե��˴���
            configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ConfigValidationSrc, ConfigValidationDst>()
                .ForMember(dst => dst.SomeValuefff, opt => opt.Ignore()));
            configuration.AssertConfigurationIsValid();
        }
        /// <summary>
        /// �̳����ƥ��ʵ��
        /// </summary>
        [Fact]
        public void NestedClassTest()
        {
            //���ڼ̳��࣬��Ҫ�������Map
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OuterSource, OuterDest>();
                cfg.CreateMap<InnerSource, InnerDest>();
            });
            config.AssertConfigurationIsValid();
            var source = new OuterSource
            {
                Value = 5,
                Inner = new InnerSource { OtherValue = 15 }
            };
            var mapper = config.CreateMapper();
            var dest = mapper.Map<OuterSource, OuterDest>(source);

            dest.Value.ShouldBe(5);
            dest.Inner.ShouldNotBeNull();
            dest.Inner.OtherValue.ShouldBe(15);
        }
        /// <summary>
        /// ����ƥ��
        /// </summary>
        [Fact]
        public void CollectionTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SourceClass, Destination>()
                .ForMember(dst => dst.DAge, opt => opt.MapFrom(src => src.Age))
                .ForMember(dst => dst.DName, opt => opt.MapFrom(src => src.Name));
            });
            var mapper = new Mapper(config);
            var source = new SourceClass[]
            {
                new SourceClass(){ Name="b1", Age=1 },
                new SourceClass(){ Name="b2", Age=2 },
                new SourceClass(){ Name="b3", Age=3 },
                new SourceClass(){ Name="b4", Age=4 }
            };
            var dst = mapper.Map<Destination[]>(source);
        }
        /// <summary>
        /// �������ת��ʵ��
        /// </summary>
        [Fact]
        public void PolymorphicElementTest()
        {
            var config = new MapperConfiguration(c => {
                c.CreateMap<ParentSource, ParentDestination>()
                     .Include<ChildSource, ChildDestination>();
                c.CreateMap<ChildSource, ChildDestination>();
            });

            var sources = new[]
            {
                new ParentSource(),
                new ChildSource(),
                new ParentSource()
            };
            var mapper = new Mapper(config);
            var destinations = mapper.Map<ParentSource[], ParentDestination[]>(sources);

            destinations[0].ShouldBeOfType<ParentDestination>();
            destinations[1].ShouldBeOfType<ChildDestination>();
            destinations[2].ShouldBeOfType<ParentDestination>();
        }
    }
}
