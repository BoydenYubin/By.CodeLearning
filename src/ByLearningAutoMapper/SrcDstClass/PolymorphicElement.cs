using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningAutoMapper
{
	public class ParentSource
	{
		public int Value1 { get; set; }
	}

	public class ChildSource : ParentSource
	{
		public int Value2 { get; set; }
	}

	public class ParentDestination
	{
		public int Value1 { get; set; }
	}

	public class ChildDestination : ParentDestination
	{
		public int Value2 { get; set; }
	}
}
