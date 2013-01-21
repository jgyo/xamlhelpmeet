// Guids.cs
// MUST match guids.h
using System;

namespace XamlHelpmeet
{
	static class GuidList
	{
		public const string guidXamlHelpmeetPkgString        = "37b68c5c-8ac0-436c-8483-0947379cffe9";
		public const string guidXamlHelpmeetCmdSetString     = "f3d73d49-82d2-49bf-a7e8-9edc050f70af";

		public static readonly Guid guidXamlHelpmeetCmdSet   = new Guid(guidXamlHelpmeetCmdSetString);
	}
}