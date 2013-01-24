using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Model
{
	/// <summary>
	/// 	Assemblies namespaces class.
	/// </summary>
	[Serializable]
	public class AssembliesNamespacesClass
	{
		/// <summary>
		/// 	Initializes a new instance of the AssembliesNamespacesClass class.
		/// </summary>
		/// <param name="AssemblyName">
		/// 	Name of the assembly.
		/// </param>
		/// <param name="Namespac">
		/// 	The namespac.
		/// </param>
		/// <param name="TypeName">
		/// 	Name of the type.
		/// </param>
		/// <param name="ClassEntity">
		/// 	The class entity.
		/// </param>
		public AssembliesNamespacesClass(string AssemblyName, 
			string Namespac,
			string TypeName, 
			ClassEntity ClassEntity)
		{
			this.ClassEntity = ClassEntity;
			Namespace = Namespac;
			this.TypeName = TypeName;
			this.AssemblyName = AssemblyName;
		}

		/// <summary>
		/// 	Gets the name of the assembly.
		/// </summary>
		/// <value>
		/// 	The name of the assembly.
		/// </value>
		public string AssemblyName { get; private set; }

		/// <summary>
		/// 	Gets the name of the type.
		/// </summary>
		/// <value>
		/// 	The name of the type.
		/// </value>
		public string TypeName { get; private set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this AssembliesNamespacesClass is
		/// 	selected.
		/// </summary>
		/// <value>
		/// 	true if this AssembliesNamespacesClass is selected, otherwise false.
		/// </value>
		public bool IsSelected { get; set; }

		/// <summary>
		/// 	Gets the namespace.
		/// </summary>
		/// <value>
		/// 	The namespace.
		/// </value>
		public string Namespace { get; private set; }

		/// <summary>
		/// 	Gets or sets the class entity.
		/// </summary>
		/// <value>
		/// 	The class entity.
		/// </value>
		public ClassEntity ClassEntity { get; set; }

	}
}
