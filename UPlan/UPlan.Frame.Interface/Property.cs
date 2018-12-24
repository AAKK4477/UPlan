using System;
using System.Drawing;
namespace UPlan.Frame.Interface
{
	[Serializable]
	public struct Property
	{
		public int top;
		public int left;
		public int width;
		public int height;
		public string comment;
		public Font font;
		public string bitMapPos;
	}
}
