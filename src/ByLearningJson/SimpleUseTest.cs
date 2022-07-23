using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ByLearningJson
{
    public class SimpleUseTest
    {
        private List<StringComparison> stringComparisons;

        [SetUp]
        public void Setup()
        {
            stringComparisons = new List<StringComparison>
            {
                StringComparison.CurrentCulture,
                StringComparison.Ordinal
            };
        }

        [Test]
        public void JsonWithoutConverterTest()
        {
            string jsonWithoutConverter = JsonConvert.SerializeObject(stringComparisons);
            Assert.AreEqual("[0,4]", jsonWithoutConverter);
        }

        [Test]
        public void JsonWithConverterTest()
        {
            string jsonWithConverter = JsonConvert.SerializeObject(stringComparisons, new StringEnumConverter());
            List<StringComparison> newStringComparsions = JsonConvert.DeserializeObject<List<StringComparison>>(
            jsonWithConverter,
            new StringEnumConverter());
            Assert.AreEqual("[\"CurrentCulture\",\"Ordinal\"]", jsonWithConverter);
        }

        /*
         "[\"JsonConverter`1\",\"BinaryConverter\",\"BsonObjectIdConverter\",\"CustomCreationConverter`1\",
           \"DataSetConverter\",\"DataTableConverter\",\"DateTimeConverterBase\",\"DiscriminatedUnionConverter\",
           \"EntityKeyMemberConverter\",\"ExpandoObjectConverter\",\"IsoDateTimeConverter\",\"JavaScriptDateTimeConverter\",
           \"KeyValuePairConverter\",\"RegexConverter\",\"StringEnumConverter\",\"UnixDateTimeConverter\",
           \"VersionConverter\",\"XmlNodeConverter\"]"
        */

        [Test]
        public void GetConverterTest()
        {
            Assembly assembly = typeof(JsonConvert).Assembly;
            var types = assembly.GetTypes();
            var result = types.Where(type => type.IsSubclassOf(typeof(JsonConverter))).Select(type => type.Name);
            var str = JsonConvert.SerializeObject(result);
            Assert.AreNotEqual("fakestr", str);
        }

        [Test]
        public void SerializeJsonToFile()
        {
            Movie movie = new Movie
            {
                Name = "Bad Boys",
                Year = 1995
            };
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var file1 = Path.Combine(desktop, "movieV1.json");
            File.WriteAllText(file1, JsonConvert.SerializeObject(movie));
            // serialize JSON directly to a file
            var file2 = Path.Combine(desktop, "movieV2.json");
            using (StreamWriter file = File.CreateText(file2))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, movie);
            }
            Assert.Pass();
        }

        [Test]
        public void SerializeConditionalPropertyTest()
        {
            Employee joe = new Employee();
            joe.Name = "Joe Employee";
            Employee mike = new Employee();
            mike.Name = "Mike Manager";
            joe.Manager = mike;
            // mike is his own manager
            // ShouldSerialize will skip this property
            mike.Manager = mike;
            string json = JsonConvert.SerializeObject(new[] { joe, mike }, Formatting.Indented);
            Assert.Pass();
        }

        [Test]
        public void SerializeAnonymousTypeTest()
        {
            var definition = new { Name = "" };
            string json1 = @"{'Name':'James'}";
            var customer1 = JsonConvert.DeserializeAnonymousType(json1, definition);
            Assert.AreEqual("James", customer1.Name);

            string json2 = @"{'Name':'Mike'}";
            var customer2 = JsonConvert.DeserializeAnonymousType(json2, definition);
            Assert.AreEqual("Mike", customer2.Name);
        }

        [Test]
        public void CustomCreationConverterTest()
        {
            string json = @"{
               'Department': 'Furniture',
               'JobTitle': 'Carpenter',
               'FirstName': 'John',
               'LastName': 'Joinery',
               'BirthDate': '1983-02-02T00:00:00'
             }";
            Person person = JsonConvert.DeserializeObject<Person>(json, new PersonConverter());
            Assert.AreEqual("EmployeePing", person.GetType().Name);
            // Employee
            EmployeePing employee = (EmployeePing)person;
            Console.WriteLine(employee.JobTitle);
            Assert.AreEqual("Carpenter", employee.JobTitle);
            // Carpenter
        }

        [Test]
        public void PopulateAnObjectTest()
        {
            Account account = new Account
            {
                Email = "james@example.com",
                Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };
            string json = @"{
              'Active': false,
              'Roles': [
                'Expired'
              ]
            }";
            JsonConvert.PopulateObject(json, account);
            Assert.AreEqual("james@example.com", account.Email);
            Assert.AreEqual(false, account.Active);
        }

        [Test]
        public void ConstructorHandlingTest()
        {
            string json = @"{'Url':'http://www.google.com'}";
            try
            {
                JsonConvert.DeserializeObject<Website>(json);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'website')", ex.Message);
            }
            Website website = JsonConvert.DeserializeObject<Website>(json, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
            Assert.AreEqual("http://www.google.com", website.Url);
        }

        [Test]
        public void ObjectCreationHandlingTest()
        {
            string json = @"{
              'Name': 'James',
              'Offices': [
                'Auckland',
                'Wellington',
                'Christchurch'
              ]
            }";
            UserViewModel model1 = JsonConvert.DeserializeObject<UserViewModel>(json);
            foreach (string office in model1.Offices)
            {
                Console.WriteLine(office);
            }
            // Auckland
            // Wellington
            // Christchurch
            // Auckland
            // Wellington
            // Christchurch
            UserViewModel model2 = JsonConvert.DeserializeObject<UserViewModel>(json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            foreach (string office in model2.Offices)
            {
                Console.WriteLine(office);
            }
            // Auckland
            // Wellington
            // Christchurch
        }

        [Test]
        public void DefaultValueHandlingTest()
        {
            PersonPing person = new PersonPing();
            string jsonIncludeDefaultValues = JsonConvert.SerializeObject(person, Formatting.Indented);
            Console.WriteLine(jsonIncludeDefaultValues);
            // {
            //   "Name": null,
            //   "Age": 0,
            //   "Partner": null,
            //   "Salary": null
            // }
            string jsonIgnoreDefaultValues = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            Console.WriteLine(jsonIgnoreDefaultValues);
            // {}
        }

        [Test]
        public void MissMemberHandlingTest()
        {
            string json = @"{
            'FullName': 'Dan Deleted',
            'Deleted': true,
            'DeletedDate': '2013-01-20T00:00:00'
            }";
            try
            {
                JsonConvert.DeserializeObject<Account>(json, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                });
            }
            catch (JsonSerializationException ex)
            {
                Assert.AreEqual("Could not find member 'FullName' on object of type 'Account'. Path 'FullName', line 2, position 23.", ex.Message);
                // Could not find member 'DeletedDate' on object of type 'Account'. Path 'DeletedDate', line 4, position 23.
            }
            JsonConvert.DeserializeObject<Account>(json, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            Assert.Pass();
        }

        [Test]
        public void ReferenceLoopHandlingTest()
        {
            EmployeeFoo joe = new EmployeeFoo { Name = "Joe User" };
            EmployeeFoo mike = new EmployeeFoo { Name = "Mike Manager" };
            joe.Manager = mike;
            mike.Manager = mike;
            try
            {
                string json = JsonConvert.SerializeObject(joe, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Error
                });
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Self referencing loop detected for property 'Manager' with type 'ByLearningJson.EmployeeFoo'. Path 'Manager'.", ex.Message);
            }
        }

        [Test]
        public void PreserveReferencesHandlingTest()
        {
            Directory root = new Directory { Name = "Root" };
            Directory documents = new Directory { Name = "My Documents", Parent = root };
            FileInfo file = new FileInfo { Name = "ImportantLegalDocument.docx", Parent = documents };
            documents.Files = new List<FileInfo> { file };

            try
            {
                JsonConvert.SerializeObject(documents, Formatting.Indented);
            }
            catch (JsonSerializationException)
            {
                // Self referencing loop detected for property 'Parent' with type
                // 'Newtonsoft.Json.Tests.Documentation.Examples.ReferenceLoopHandlingObject+Directory'. Path 'Files[0]'.
            }

            string preserveReferenacesAll = JsonConvert.SerializeObject(documents, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });
            string preserveReferenacesObjects = JsonConvert.SerializeObject(documents, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            Assert.Pass();
        }

        [Test]
        public void DateFormatHandlingTest()
        {
            DateTime mayanEndOfTheWorld = new DateTime(2012, 12, 21);
            string jsonIsoDate = JsonConvert.SerializeObject(mayanEndOfTheWorld);
            Assert.AreEqual("\"2012-12-21T00:00:00\"", jsonIsoDate);
            string jsonMsDate = JsonConvert.SerializeObject(mayanEndOfTheWorld, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            });
            Assert.AreEqual("\"\\/Date(1356019200000+0800)\\/\"", jsonMsDate);
        }

        [Test]
        public void DateTimeZoneHandlingTest()
        {
            Flight flight = new Flight
            {
                Destination = "Dubai",
                DepartureDate = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Unspecified),
                DepartureDateUtc = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Utc),
                DepartureDateLocal = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Local),
                Duration = TimeSpan.FromHours(5.5)
            };

            string jsonWithRoundtripTimeZone = JsonConvert.SerializeObject(flight, Formatting.Indented, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
            });

            Console.WriteLine(jsonWithRoundtripTimeZone);
            // {
            //   "Destination": "Dubai",
            //   "DepartureDate": "2013-01-21T00:00:00",
            //   "DepartureDateUtc": "2013-01-21T00:00:00Z",
            //   "DepartureDateLocal": "2013-01-21T00:00:00+01:00",
            //   "Duration": "05:30:00"
            // }

            string jsonWithLocalTimeZone = JsonConvert.SerializeObject(flight, Formatting.Indented, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });

            Console.WriteLine(jsonWithLocalTimeZone);
            // {
            //   "Destination": "Dubai",
            //   "DepartureDate": "2013-01-21T00:00:00+01:00",
            //   "DepartureDateUtc": "2013-01-21T01:00:00+01:00",
            //   "DepartureDateLocal": "2013-01-21T00:00:00+01:00",
            //   "Duration": "05:30:00"
            // }

            string jsonWithUtcTimeZone = JsonConvert.SerializeObject(flight, Formatting.Indented, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Console.WriteLine(jsonWithUtcTimeZone);
            // {
            //   "Destination": "Dubai",
            //   "DepartureDate": "2013-01-21T00:00:00Z",
            //   "DepartureDateUtc": "2013-01-21T00:00:00Z",
            //   "DepartureDateLocal": "2013-01-20T23:00:00Z",
            //   "Duration": "05:30:00"
            // }

            string jsonWithUnspecifiedTimeZone = JsonConvert.SerializeObject(flight, Formatting.Indented, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            });

            Console.WriteLine(jsonWithUnspecifiedTimeZone);
            // {
            //   "Destination": "Dubai",
            //   "DepartureDate": "2013-01-21T00:00:00",
            //   "DepartureDateUtc": "2013-01-21T00:00:00",
            //   "DepartureDateLocal": "2013-01-21T00:00:00",
            //   "Duration": "05:30:00"
            // }
            Assert.Pass();
        }

        [Test]
        public void TypeNameHandlingTest()
        {
            Stockholder stockholder = new Stockholder
            {
                FullName = "Steve Stockholder",
                Businesses = new List<Business>
                {
                    new Hotel
                    {
                        Name = "Hudson Hotel",
                        Stars = 4
                    }
                }
            };

            string jsonTypeNameAll = JsonConvert.SerializeObject(stockholder, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            Console.WriteLine(jsonTypeNameAll);
            // {
            //   "$type": "Newtonsoft.Json.Samples.Stockholder, Newtonsoft.Json.Tests",
            //   "FullName": "Steve Stockholder",
            //   "Businesses": {
            //     "$type": "System.Collections.Generic.List`1[[Newtonsoft.Json.Samples.Business, Newtonsoft.Json.Tests]], mscorlib",
            //     "$values": [
            //       {
            //         "$type": "Newtonsoft.Json.Samples.Hotel, Newtonsoft.Json.Tests",
            //         "Stars": 4,
            //         "Name": "Hudson Hotel"
            //       }
            //     ]
            //   }
            // }

            string jsonTypeNameAuto = JsonConvert.SerializeObject(stockholder, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            Console.WriteLine(jsonTypeNameAuto);
            // {
            //   "FullName": "Steve Stockholder",
            //   "Businesses": [
            //     {
            //       "$type": "Newtonsoft.Json.Samples.Hotel, Newtonsoft.Json.Tests",
            //       "Stars": 4,
            //       "Name": "Hudson Hotel"
            //     }
            //   ]
            // }

            // for security TypeNameHandling is required when deserializing
            Stockholder newStockholder = JsonConvert.DeserializeObject<Stockholder>(jsonTypeNameAuto, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            Console.WriteLine(newStockholder.Businesses[0].GetType().Name);
            // Hotel
        }

        [Test]
        public void MetadataPropertyHandlingTest()
        {
            string json = @"{
              'Name': 'James',
              'Password': 'Password1',
              '$type': 'ByLearningJson.User, ByLearningJson'
            }";

            object o = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                // $type no longer needs to be first
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
            });        
            User u = (User)o;
            Assert.AreEqual("James", u.Name);
        }

        [Test]
        public void ContractResolverTest()
        {
            PersonFoo person = new PersonFoo
            {
                FirstName = "Sarah",
                LastName = "Security"
            };
            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            Console.WriteLine(json);
            // {
            //   "firstName": "Sarah",
            //   "lastName": "Security",
            //   "fullName": "Sarah Security"
            // }
            Assert.Pass();
        }
        [Test]
        public void TraceWriterTest()
        {
            string json = @"{
            'FullName': 'Dan Deleted',
            'Deleted': true,
            'DeletedDate': '2013-01-20T00:00:00'
            }";
            var traceWriter = new MemoryTraceWriter();
            Account account = JsonConvert.DeserializeObject<Account>(json, new JsonSerializerSettings
            {
                TraceWriter = traceWriter
            });
            Console.WriteLine(traceWriter.ToString());
            // 2013-01-21T01:36:24.422 Info Started deserializing Newtonsoft.Json.Tests.Documentation.Examples.TraceWriter+Account. Path 'FullName', line 2, position 20.
            // 2013-01-21T01:36:24.442 Verbose Could not find member 'DeletedDate' on Newtonsoft.Json.Tests.Documentation.Examples.TraceWriter+Account. Path 'DeletedDate', line 4, position 23.
            // 2013-01-21T01:36:24.447 Info Finished deserializing Newtonsoft.Json.Tests.Documentation.Examples.TraceWriter+Account. Path '', line 5, position 8.
            // 2013-01-21T01:36:24.450 Verbose Deserialized JSON: 
            // {
            //   "FullName": "Dan Deleted",
            //   "Deleted": true,
            //   "DeletedDate": "2013-01-20T00:00:00"
            // }
        }

        [Test]
        public void ErrorHandlingTest()
        {
            List<string> errors = new List<string>();

            List<DateTime> c = JsonConvert.DeserializeObject<List<DateTime>>(@"[
              '2009-09-09T00:00:00Z',
              'I am not a date and will error!',
              [
                1
              ],
              '1977-02-20T00:00:00Z',
              null,
              '2000-12-01T00:00:00Z'
            ]",
            new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
                Converters = { new IsoDateTimeConverter() }
            });
            // 2009-09-09T00:00:00Z
            // 1977-02-20T00:00:00Z
            // 2000-12-01T00:00:00Z

            // The string was not recognized as a valid DateTime. There is a unknown word starting at index 0.
            // Unexpected token parsing date. Expected String, got StartArray.
            // Cannot convert null value to System.DateTime.
        }

        [Test]
        public void MaxDepthTest()
        {
            string json = @"[
              [
                [
                  '1',
                  'Two',
                  'III'
                ]
              ]
            ]";

            try
            {
                var obj = JsonConvert.DeserializeObject<List<IList<IList<string>>>>(json, new JsonSerializerSettings
                {
                    MaxDepth = 2
                });
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine(ex.Message);
                // The reader's MaxDepth of 2 has been exceeded. Path '[0][0]', line 3, position 12.
            }
        }
    }
}