/*using System;
using NUnit.Framework;
namespace AssemblyCSharp
{

	[TestFixture]
	public class TransformationTests
	{

		[TestCase(1024,640)]
		[TestCase(0,0)]
		[TestCase(480,300)]
		[TestCase(240,150)]
		public void Transform_X( float exp, float inp )
		{
			var t = new Transposer(640,480,1024,768);
			Assert.AreEqual( exp, t.X(inp));
		}

		[TestCase(0,480)]
		[TestCase(768,0)]
		public void Transform_Y( float exp, float inp )
		{
			var t = new Transposer(640f,480f,1024f,768f);
			Assert.AreEqual( exp, t.Y(inp));
		}
	}

}
*/
