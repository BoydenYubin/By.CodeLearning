using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningAutoMapper
{
    public class SourceClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class Destination
    {
        public string DName { get; set; }
        public int DAge { get; set; }
    }
}
