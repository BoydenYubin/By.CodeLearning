using ByLearningMessagePack.BaseClass;
using MessagePack;
using Shouldly;
using Xunit;

namespace ByLearningMessagePack
{
    /// <summary>
    /// https://github.com/neuecc/MessagePack-CSharp
    /// </summary>
    public class SimpleUseTest
    {
        [Fact]
        public void SimpleSerializeTest()
        {
            var data = MessagePackSerializer.Serialize(new MyClass()
            {
                FirstName = "boyden",
                LastName = "yol",
                Age = 29
            }) ;
            data.ShouldNotBeEmpty();
        }
    }
}
