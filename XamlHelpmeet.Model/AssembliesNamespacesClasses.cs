using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Model
{
	[Serializable]
	public class AssembliesNamespacesClasses : List<AssembliesNamespacesClass>
	{
		public AssembliesNamespacesClasses()
		{
			
		}
		public AssembliesNamespacesClasses(int capacity)
			: base(capacity)
		{
			
		}
		public AssembliesNamespacesClasses(IEnumerable<AssembliesNamespacesClass> collection)
			: base(collection)
		{
			
		}
		public AssembliesNamespacesClass SelectedItem
		{
			get
			{
				return (from anc in this
					where anc.IsSelected
					select anc).SingleOrDefault();
			}
		}
	}
}
