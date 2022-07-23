using ByLearningProtobuf.BaseClass;
using ProtoBuf;
using Shouldly;
using System.IO;
using Xunit;

namespace ByLearningProtobuf
{
    public class SimpleUseTest
    {
        [Fact]
        public void SimpleSerializeTest()
        {
            Person person = new Person() {
                Id = 1,
                Name = "jacky",
                Address = new Address()
                {
                    Line1 = "Line1",
                    Line2 = "Line2"
                }
            };
            MemoryStream ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, person);
            var data = ms.ToArray();
            data.ShouldNotBeEmpty();
        }
        [Fact]
        public void SimpleDeSerializeTest()
        {
            Person person = new Person()
            {
                Id = 1,
                Name = "jacky",
                Address = new Address()
                {
                    Line1 = "Line1",
                    Line2 = "Line2"
                }
            };
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, person);
            ms.Position = 0;
            var deperson = ProtoBuf.Serializer.Deserialize<Person>(ms);
            deperson.Address.Line1.ShouldBe("Line1");
        }
        [Fact]
        public void SerializeToFileTest()
        {
            var person = new Person
            {
                Id = 12345,
                Name = "Fred",
                Address = new Address
                {
                    Line1 = "Flat 1",
                    Line2 = "The Meadows"
                }
            };
            using (var file = File.Create("person.bin"))
            {
                Serializer.Serialize(file, person);
            }
        }
        [Fact]
        public void DeserializeFromFileTest()
        {
            using (var file = File.OpenRead("person.bin"))
            {
                var person = Serializer.Deserialize<Person>(file);
                person.Name.ShouldBe("Fred");
            }
        }
    }
}
