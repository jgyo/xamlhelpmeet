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
		/// <param name="assemblyName">
		/// 	Name of the assembly.
		/// </param>
		/// <param name="nameSpace">
		/// 	The namespac.
		/// </param>
		/// <param name="typeName">
		/// 	Name of the type.
		/// </param>
		/// <param name="classEntity">
		/// 	The class entity.
		/// </param>
		public AssembliesNamespacesClass(string assemblyName, 
			string nameSpace,
			string typeName, 
			ClassEntity classEntity)
		{
			this.ClassEntity = classEntity;
			this.Namespace = nameSpace;
			this.TypeName = typeName;
			this.AssemblyName = assemblyName;
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
