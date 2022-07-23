using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace ByLearningJson.ConvertTest
{
    public class JsonConvertTest
    {
        [SetUp]
        public void SetUp()
        {

        }
        #region
        public class KeysJsonConverter : JsonConverter
        {
            private readonly Type[] _types;

            public KeysJsonConverter(params Type[] types)
            {
                _types = types;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                JToken t = JToken.FromObject(value);

                if (t.Type != JTokenType.Object)
                {
                    t.WriteTo(writer);
                }
                else
                {
                    JObject o = (JObject)t;
                    IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                    o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                    o.WriteTo(writer);
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanConvert(Type objectType)
            {
                return _types.Any(t => t == objectType);
            }
        }

        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IList<string> Roles { get; set; }
        }
        #endregion
        [Test]
        public void CustomJsonConverterTest()
        {
            Employee employee = new Employee
            {
                FirstName = "James",
                LastName = "Newton-King",
                Roles = new List<string>
                {
                    "Admin"
                }
            };
            string json = JsonConvert.SerializeObject(employee, Formatting.Indented, new KeysJsonConverter(typeof(Employee)));
            Console.WriteLine(json);
            Employee newEmployee = JsonConvert.DeserializeObject<Employee>(json, new KeysJsonConverter(typeof(Employee)));
            Console.WriteLine(newEmployee.FirstName);
        }

        #region
        public class VersionConverter : JsonConverter<Version>
        {
            public override void WriteJson(JsonWriter writer, Version value, JsonSerializer serializer)
            {
                value = new Version(10, 0, 5);
                writer.WriteValue(value.ToString());
            }

            public override Version ReadJson(JsonReader reader, Type objectType, Version existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                string s = (string)reader.Value;

                return new Version(s);
            }
        }

        public class NuGetPackage
        {
            public string PackageId { get; set; }
            public Version Version { get; set; }
        }
        #endregion

        [Test]
        public void CustomJsonConverterTTest()
        {
            NuGetPackage p1 = new NuGetPackage
            {
                PackageId = "Newtonsoft.Json",
                Version = new Version(10, 0, 4)
            };

            string json = JsonConvert.SerializeObject(p1, Formatting.Indented, new VersionConverter());

            Console.WriteLine(json);
            // {
            //   "PackageId": "Newtonsoft.Json",
            //   "Version": "10.0.4"
            // }

            NuGetPackage p2 = JsonConvert.DeserializeObject<NuGetPackage>(json, new VersionConverter());

            Console.WriteLine(p2.Version.ToString());
            // 10.0.4
        }

        #region
        public class DynamicContractResolver : DefaultContractResolver
        {
            private readonly char _startingWithChar;

            public DynamicContractResolver(char startingWithChar)
            {
                _startingWithChar = startingWithChar;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

                // only serializer properties that start with the specified character
                properties =
                    properties.Where(p => p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();

                return properties;
            }
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string FullName
            {
                get { return FirstName + " " + LastName; }
            }
        }
        #endregion
        [Test]
        public void CustomIContractResolverTest()
        {
            Person person = new Person
            {
                FirstName = "Dennis",
                LastName = "Deepwater-Diver"
            };

            string startingWithF = JsonConvert.SerializeObject(person, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new DynamicContractResolver('F') });

            Console.WriteLine(startingWithF);
            // {
            //   "FirstName": "Dennis",
            //   "FullName": "Dennis Deepwater-Diver"
            // }

            string startingWithL = JsonConvert.SerializeObject(person, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new DynamicContractResolver('L') });

            Console.WriteLine(startingWithL);
            // {
            //   "LastName": "Deepwater-Diver"
            // }

        }

        #region
        
        #endregion

        public void CustomITraceWriterTest()
        {

        }

        #region
        public class KnownTypesBinder : ISerializationBinder
        {
            public IList<Type> KnownTypes { get; set; }

            public Type BindToType(string assemblyName, string typeName)
            {
                return KnownTypes.SingleOrDefault(t => t.Name == typeName);
            }

            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }
        }

        public class Car
        {
            public string Maker { get; set; }
            public string Model { get; set; }
        }
        #endregion

        [Test]
        public void CustomSerializationBinderTest()
        {
            KnownTypesBinder knownTypesBinder = new KnownTypesBinder
            {
                KnownTypes = new List<Type> { typeof(string) }
            };

            Car car = new Car
            {
                Maker = "Ford",
                Model = "Explorer"
            };

            string json = JsonConvert.SerializeObject(car, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = knownTypesBinder
            });

            Console.WriteLine(json);
            // {
            //   "$type": "Car",
            //   "Maker": "Ford",
            //   "Model": "Explorer"
            // }

            object newValue = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = knownTypesBinder
            });

            Console.WriteLine(newValue.GetType().Name);
            // Car
        }
    }
}
