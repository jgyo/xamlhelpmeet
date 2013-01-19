using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Cecil;
using XamlHelpmeet.Extentions;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.ReflectionLoader
{
	/// <summary>
	/// 	This class uses Mono.Cecil.dll to reflect all types visible in the
	/// 	assembly.
	/// </summary>
	/// <remarks>
	///     Mono defines SystemRuntime.CompilerServices.ExtensionAttribute
	/// 	duplicating the same class in mscorlib.dll, so it causes a warning
	/// 	to appear in the error list. Visual Studio will use the Microsoft
	/// 	class definition, so the warning can be ignored.
	/// </remarks>
	public class RemoteWorker
	{
		public string GetAssemblyFullPath(string TargetProjectPath, string AssemblyName)
		{
			if (File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".dll")))
			{
				return Path.Combine(TargetProjectPath, AssemblyName, ".dll");
			}
			if (File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".exe")))
			{
				return Path.Combine(TargetProjectPath, AssemblyName, ".exe");
			}
			return string.Empty;
		}

		// BMK Clean up parameters when finished
		public RemoteResponse<XamlHelpmeet.Model.AssembliesNamespacesClasses> GetClassEntityFromUserSelectedClass(string AssemblyPath, bool IsSilverlight, string NameOfSourceCommand, List<string> references)
		{
			var data = new AssembliesNamespacesClasses();
			var targetProjectPath = Path.GetDirectoryName(AssemblyPath);
			var targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(AssemblyPath);
			Exception ex = null;
			var assembliesToLoad = new Hashtable();

			// Load this assembly
			assembliesToLoad.Add(AssemblyPath.ToLower(), null);
			foreach (var item in targetAssemblyDefinition.MainModule.AssemblyReferences)
			{
				if (item.IsNotMicrosoftAssembly())
				{
					var assemblyFullPath = GetAssemblyFullPath(targetProjectPath, item.Name);
					if (assemblyFullPath.IsNotNullOrEmpty() && assembliesToLoad.ContainsKey(assemblyFullPath.ToLower()) == false)
					{
						assembliesToLoad.Add(assemblyFullPath.ToLower(), null);
					}
				}
			}

			// Load up all assemblies referenced in the project, but that are not
			// loaded yet.
			foreach (var item in references)
			{
				targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(item);

				LoadAssemblyClasses(targetAssemblyDefinition, IsSilverlight, data, out ex, assembliesToLoad);

				if (ex != null)
				{
					return new RemoteResponse<AssembliesNamespacesClasses>(null, ResponseStatus.Exception, ex, String.Format("Unable to load types from target assembly: {0}", targetAssemblyDefinition.Name));
				}
			}
			return new RemoteResponse<AssembliesNamespacesClasses>(data, ResponseStatus.Success, null, null);
		}

		private bool CanWrite(PropertyDefinition Property)
		{
			return Property.SetMethod != null && Property.SetMethod.IsPublic;
		}

		/// <summary>
		/// 	objType.BaseType can have different types in it. The base type may or may not be
		/// 	in an assembly we have loaded. However, as long as it is referenced and we have a
		/// 	path to it, it will be loaded and the base type properites added to the list for
		/// 	the TypeDefinition. This function also recurses to get the type and all its base
		/// 	classes.
		/// </summary>
		/// <param name="asy">
		/// 	This is the assembly definition for the target TypeDefinition.
		/// </param>
		/// <param name="AssemblyType">
		/// 	Type of the assembly.
		/// </param>
		/// <param name="AssembliesToLoad">
		/// 	The this is a hashtable of all of the assemblies to load.
		/// </param>
		/// <returns>
		/// 	All properties for the TypedDefinition loaded in a List&lt;PropertyDefinition&gt;
		/// </returns>

		private string FormatPropertyTypeName(PropertyDefinition Property)
		{
			var name = Property.Name;
			var fullName = Property.PropertyType.FullName;
			if (name.Contains("`") == false)
			{
				return name;
			}
			name = name.Remove(name.IndexOf("`"));

			// BMK check the Type logic of this line.
			if (Property.PropertyType == null || Property.PropertyType.GetType() == typeof(GenericInstanceType) || fullName.IndexOf(">") == -1)
			{
				return name;
			}

			var sb = new StringBuilder(512);
			sb.AppendFormat("{0} (Of ", name);

			var obj = Property.PropertyType as GenericInstanceType;
			if (obj.HasGenericArguments)
			{
				foreach (var tr in obj.GenericArguments)
				{
					sb.Append(tr.Name);
					sb.Append(", ");
				}
			}
			else
			{
				return name;
			}

			sb.Length -= 2;
			sb.Append(")");
			return sb.ToString();
		}

		private List<PropertyDefinition> GetAllPropertiesForType(AssemblyDefinition asy, TypeDefinition AssemblyType, Hashtable AssembliesToLoad)
		{
			var returnValue = new List<PropertyDefinition>();
			foreach (var item in AssemblyType.Properties)
			{
				returnValue.Add(item);
			}

			if (AssemblyType != null && AssemblyType.BaseType == AssemblyType.Module.Import(typeof(object)) && AssemblyType.BaseType.Scope != null)
			{
				var baseTypeAssemblyName = string.Empty;
				var td = AssemblyType.BaseType as TypeDefinition;

				if (td != null)
				{
					var md = td.Scope as ModuleDefinition;

					if (md != null)
					{
						baseTypeAssemblyName = md.Name.ToLower();
					}
				}

				if (baseTypeAssemblyName.IsNull())
				{
					var anr = AssemblyType.BaseType.Scope as AssemblyNameReference;

					if (anr != null)
					{
						baseTypeAssemblyName = anr.Name.ToLower();
					}
				}

				if (baseTypeAssemblyName.IsNullOrWhiteSpace())
				{
					AssemblyDefinition targetAssemblyDefinition = null;

					foreach (string assemblyName in AssembliesToLoad.Keys)
					{
						if (!assemblyName.EndsWith(baseTypeAssemblyName) && assemblyName.IndexOf(baseTypeAssemblyName) <= -1)
							continue;
						targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyName);
						break;
					}

					if (targetAssemblyDefinition != null)
					{
						foreach (var baseTypeDefinition in targetAssemblyDefinition.MainModule.Types)
						{
							if (!baseTypeDefinition.IsClass || baseTypeDefinition.Name != AssemblyType.BaseType.Name)
								continue;
							returnValue.AddRange(GetAllPropertiesForType(asy, baseTypeDefinition, AssembliesToLoad));
							break;
						}
					}
				}
			}
			return returnValue;
		}

		private void LoadAssemblyClasses(AssemblyDefinition asy, bool IsSilverlight, AssembliesNamespacesClasses Data, out Exception exOut, Hashtable AssembliesToLoad)
		{
			exOut = null;

			try
			{
				foreach (var type in asy.MainModule.Types)
				{
					if (type.IsPublic &&
						type.IsClass &&
						!type.IsAbstract &&
						type.Name.Contains("<Module>") &&
						!type.Name.Contains("AnonymousType") &&
						!type.Name.StartsWith("_") &&
						!type.Name.EndsWith("AssemblyInfo"))
					{
						var previouslyLoaded = false;

						foreach (var anc in Data)
						{
							if (type.Name != anc.TypeName ||
								type.Namespace != anc.Namespace ||
								asy.Name.Name != anc.AssemblyName)
								continue;
							previouslyLoaded = true;
							break;
						}

						if (!previouslyLoaded)
						{
							if (type.BaseType != null || type.BaseType.Name != "MulticastDelegate")
							{
								var classEntity = new ClassEntity(type.Name, IsSilverlight);

								// Original code, now been replaced by following
								// line
								// For Each objProperty As PropertyDefinition In objType.Properties
								foreach (var property in GetAllPropertiesForType(asy, type, AssembliesToLoad))
								{
									if (property.GetMethod != null && property.GetMethod.IsPublic)
									{
										var pi = new PropertyInformation(CanWrite(property), property.Name, FormatPropertyTypeName(property), property.PropertyType.Namespace);

										// BMK Test the logic of this line.
										// I'm not sure about the Type handling here. JGYO
										if (property.PropertyType != null && property.PropertyType.GetType() == typeof(GenericInstanceType))
										{
											var obj = property.PropertyType as GenericInstanceType;
											if (obj.HasGenericArguments)
											{
												foreach (var tr in obj.GenericArguments)
												{
													pi.GenericArguments.Add(tr.Name);
												}
											}
										}
										if (property.HasParameters)
										{
											foreach (var pd in property.Parameters)
											{
												pi.PropertyParameters.Add(new PropertyParameter(pd.Name, pd.ParameterType.Name));
											}
										}
										classEntity.PropertyInformation.Add(pi);
									}
								}
								Data.Add(new AssembliesNamespacesClass(asy.Name.Name, type.Namespace, type.Name, classEntity));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				exOut = ex;
			}
		}
	}
}