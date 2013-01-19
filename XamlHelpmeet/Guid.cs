// Guids.cs
// MUST match guids.h
using System;

namespace XamlHelpmeet
{
	static class GuidList
	{
		public const string guidXamlHelpmeetPkgString        = "62b1166f-4c11-48f9-a092-9537de7c4cb7";
		public const string guidXamlHelpmeetCmdSetString     = "578c01ed-6bd8-4bed-8e1c-1ff47f1347eb";

		public const string guidXamlHelpmeetCodeMenuString   = "13c17015-82a3-4310-b912-6ed7f6d6f19f";
		public const string guidXamlHelpmeetXamlMenuString   = "6720b59d-1e17-41c0-aa57-ed761e00301a";
		public const string guidGroupIntoMenuString          = "7c468f4f-1061-4e81-aead-41c48da74927";
		public const string guidGroupIntoBorderMenuString    = "7ef51685-b558-4977-959c-4a58f15cee96";
		public const string guidToolsMenuString              = "e50735e7-a179-46d1-864a-514c010e3d1c";

		public static readonly Guid guidXamlHelpmeetCmdSet   = new Guid(guidXamlHelpmeetCmdSetString);

		public static readonly Guid guidXamlHelpmeetCodeMenu = new Guid(guidXamlHelpmeetCodeMenuString);
		public static readonly Guid guidXamlHelpmeetXamlMenu = new Guid(guidXamlHelpmeetXamlMenuString);
		public static readonly Guid guidGroupIntoMenu        = new Guid(guidGroupIntoMenuString);
		public static readonly Guid guidGroupIntoBorderMenu  = new Guid(guidGroupIntoBorderMenuString);
		public static readonly Guid guidToolsMenu            = new Guid(guidToolsMenuString);
	}
}