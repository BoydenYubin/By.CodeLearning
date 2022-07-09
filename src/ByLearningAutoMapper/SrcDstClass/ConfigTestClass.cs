using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningAutoMapper
{
    public class ConfigSrcClass
    {
        public string property_name { get; set; }
    }
    public class ConfigDstClass
    {
        public string PropertyName { get; set; }
    }

    public class PreSrcClass
    {
        public string GetName { get; set; }
        public int frmAge { get; set; }
    }
    public class PreDstClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ConfigValidationSrc
    {
        public int SomeValue { get; set; }
    }
    public class ConfigValidationDst
    {
        public int SomeValuefff { get; set; }
    }
}
