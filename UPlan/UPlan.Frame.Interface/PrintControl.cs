using UPlan.Common.Validate;
using System;
using System.IO;
namespace UPlan.Frame.Interface
{
	[Serializable]
	public struct PrintControl
	{
		public bool isSelected;
		public Vert posVert;
		public Hori posHori;
		public bool isOnMap;
		public Property property;
		public string name;
		public bool IsValidated()
		{
			return this.name == "Legend" || File.Exists(this.property.bitMapPos) || DataValidate.NotEmptyString(this.property.comment);
		}
	}
}
