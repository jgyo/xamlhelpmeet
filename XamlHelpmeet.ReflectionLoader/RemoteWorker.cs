namespace XamlHelpmeet.ReflectionLoader
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Mono.Cecil;
    using XamlHelpmeet.Extensions;
    using XamlHelpmeet.Model;

    #endregion

    /// <summary>
    ///     This class uses Mono.Cecil.dll to reflect all types visible in the
    ///     assembly.
    /// </summary>
    /// <remarks>
    ///     Mono defines SystemRuntime.CompilerServices.ExtensionAttribute
    ///     duplicating the same class in mscorlib.dll, so it causes a warning
    ///     to appear in the error list. Visual Studio will use the Microsoft
    ///     class definition, so the warning can be ignored.
    /// </remarks>
    public class RemoteWorker : MarshalByRefObject
    {
        #region Methods (public)

        public string GetAssemblyFullPath(string TargetProjectPath, string AssemblyName)
        {
            if (File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".dll")))
            {
                return Path.Combine(TargetProjectPath, AssemblyName, ".dll");
            }
            return File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".exe"))
                       ? Path.Combine(TargetProjectPath, AssemblyName, ".exe")
                       : string.Empty;
        }

        public RemoteResponse<AssembliesNamespacesClasses> GetClassEntityFromUserSelectedClass(string assemblyPath,
                                                                                               bool isSilverlight,
                                                                                               List<string> references)
        {
            var ancs = new AssembliesNamespacesClasses();
            string targetProjectPath = Path.GetDirectoryName(assemblyPath);
            AssemblyDefinition targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);
            Exception ex = null;
            var assembliesToLoad = new Hashtable();

            // Load this assembly
            assembliesToLoad.Add(assemblyPath.ToLower(), null);
            foreach (var item in targetAssemblyDefinition.MainModule.AssemblyReferences)
            {
                if (item.IsNotMicrosoftAssembly())
                {
                    string assemblyFullPath = this.GetAssemblyFullPath(targetProjectPath, item.Name);
                    if (assemblyFullPath.IsNotNullOrEmpty()
                        && assembliesToLoad.ContainsKey(assemblyFullPath.ToLower()) == false)
                    {
                        assembliesToLoad.Add(assemblyFullPath.ToLower(), null);
                    }
                }
            }

            // Load up all assemblies referenced in the project, but that are not
            // loaded yet.
            foreach (var name in references)
            {
                if (!assembliesToLoad.ContainsKey(Path.GetFileName(name.ToLower())))
                {
                    assembliesToLoad.Add(name.ToLower(), null);
                }
            }

            string failedAssemblies = string.Empty;
            var status = ResponseStatus.Failed;
            foreach (string assemblyToLoadPath in assembliesToLoad.Keys)
            {
                targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyToLoadPath);

                this.LoadAssemblyClasses(targetAssemblyDefinition, isSilverlight, ancs, out ex, assembliesToLoad);

                if (ex == null)
                {
                    status = ResponseStatus.Success;
                    continue;
                }

                failedAssemblies += String.Format("{0}{1}", targetAssemblyDefinition.Name, Environment.NewLine);
                // return new RemoteResponse<AssembliesNamespacesClasses>(null, ResponseStatus.Exception, ex, String.Format("Unable to load types from target assembly: {0}", targetAssemblyDefinition.Name));
            }
            return new RemoteResponse<AssembliesNamespacesClasses>(ancs, status, failedAssemblies);
        }

        #endregion

        #region Methods (private)

        private bool CanWrite(PropertyDefinition Property)
        {
            return Property.SetMethod != null && Property.SetMethod.IsPublic;
        }

        /// <summary>
        ///     objType.BaseType can have different types in it. The base type may or may
        ///     not be
        ///     in an assembly we have loaded. However, as long as it is referenced and we
        ///     have a
        ///     path to it, it will be loaded and the base type properites added to the
        ///     list for
        ///     the TypeDefinition. This function also recurses to get the type and all its
        ///     base
        ///     classes.
        /// </summary>
        /// <param name="asy">
        ///     This is the assembly definition for the target TypeDefinition.
        /// </param>
        /// <param name="AssemblyType">
        ///     Type of the assembly.
        /// </param>
        /// <param name="AssembliesToLoad">
        ///     The this is a hashtable of all of the assemblies to load.
        /// </param>
        /// <returns>
        ///     All properties for the TypedDefinition loaded in a List&lt;
        ///     PropertyDefinition&gt;
        /// </returns>
        private string FormatPropertyTypeName(PropertyDefinition property)
        {
            string name = property.PropertyType.Name;
            string fullName = property.PropertyType.FullName;
            if (name.Contains("`") == false)
            {
                return name;
            }
            name = name.Remove(name.IndexOf("`"));

            if (property.PropertyType == null || !(property.PropertyType is GenericInstanceType)
                || fullName.IndexOf(">") == -1)
            {
                return name;
            }

            var sb = new StringBuilder(512);
            sb.AppendFormat("{0} (Of ", name);

            var propertyInstanceType = property.PropertyType as GenericInstanceType;
            if (propertyInstanceType.HasGenericArguments)
            {
                foreach (var typeReference in propertyInstanceType.GenericArguments)
                {
                    sb.Append(typeReference.Name);
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

        private List<PropertyDefinition> GetAllPropertiesForType(AssemblyDefinition assemblyDefinition,
                                                                 TypeDefinition assemblyType,
                                                                 Hashtable assembliesToLoad)
        {
            var returnValue = new List<PropertyDefinition>();
            foreach (var item in assemblyType.Properties)
            {
                returnValue.Add(item);
            }

            if (assemblyType != null && assemblyType.BaseType == assemblyType.Module.Import(typeof(object))
                && assemblyType.BaseType.Scope != null)
            {
                string baseTypeAssemblyName = string.Empty;
                var typeDef = assemblyType.BaseType as TypeDefinition;

                if (typeDef != null)
                {
                    var moduleDef = typeDef.Scope as ModuleDefinition;

                    if (moduleDef != null)
                    {
                        baseTypeAssemblyName = moduleDef.Name.ToLower();
                    }
                }

                if (baseTypeAssemblyName.IsNull())
                {
                    var assemblyNameReference = assemblyType.BaseType.Scope as AssemblyNameReference;

                    if (assemblyNameReference != null)
                    {
                        baseTypeAssemblyName = assemblyNameReference.Name.ToLower();
                    }
                }

                if (baseTypeAssemblyName.IsNullOrWhiteSpace())
                {
                    AssemblyDefinition targetAssemblyDefinition = null;

                    foreach (string assemblyName in assembliesToLoad.Keys)
                    {
                        if (!assemblyName.EndsWith(baseTypeAssemblyName)
                            && assemblyName.IndexOf(baseTypeAssemblyName) <= -1)
                        {
                            continue;
                        }
                        targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyName);
                        break;
                    }

                    if (targetAssemblyDefinition != null)
                    {
                        foreach (var baseTypeDefinition in targetAssemblyDefinition.MainModule.Types)
                        {
                            if (!baseTypeDefinition.IsClass || baseTypeDefinition.Name != assemblyType.BaseType.Name)
                            {
                                continue;
                            }
                            returnValue.AddRange(this.GetAllPropertiesForType(assemblyDefinition,
                                                                              baseTypeDefinition,
                                                                              assembliesToLoad));
                            break;
                        }
                    }
                }
            }
            return returnValue;
        }

        private void LoadAssemblyClasses(AssemblyDefinition assemblyDefinition,
                                         bool isSilverlight,
                                         AssembliesNamespacesClasses ancs,
                                         out Exception exOut,
                                         Hashtable assembliesToLoad)
        {
            exOut = null;

            try
            {
                foreach (var type in assemblyDefinition.MainModule.Types)
                {
                    if (type.IsPublic && type.IsClass && !type.IsAbstract && !type.Name.Contains("<Module>")
                        && !type.Name.Contains("AnonymousType") && !type.Name.StartsWith("_")
                        && !type.Name.EndsWith("AssemblyInfo"))
                    {
                        bool previouslyLoaded = false;

                        foreach (var anc in ancs)
                        {
                            if (type.Name != anc.TypeName || type.Namespace != anc.Namespace
                                || assemblyDefinition.Name.Name != anc.AssemblyName)
                            {
                                continue;
                            }
                            previouslyLoaded = true;
                            break;
                        }

                        if (!previouslyLoaded)
                        {
                            if (type.BaseType == null || type.BaseType.Name != "MulticastDelegate")
                            {
                                var classEntity = new ClassEntity(type.Name, isSilverlight);

                                foreach (
                                    var property in
                                        this.GetAllPropertiesForType(assemblyDefinition, type, assembliesToLoad))
                                {
                                    if (property.GetMethod != null && property.GetMethod.IsPublic)
                                    {
                                        var propertyInfo = new PropertyInformation(this.CanWrite(property),
                                                                                   property.Name,
                                                                                   this.FormatPropertyTypeName(property),
                                                                                   property.PropertyType.Namespace);

                                        // BMK Test the logic of this line.
                                        // I'm not sure about the Type handling here. JGYO
                                        // var result1 = property.PropertyType is GenericInstanceType;
                                        // var result2 = property.PropertyType.GetType() == typeof(GenericInstanceType);
                                        var git = property.PropertyType as GenericInstanceType;

                                        if (git != null)
                                        {
                                            if (git.HasGenericArguments)
                                            {
                                                foreach (var typeRef in git.GenericArguments)
                                                {
                                                    propertyInfo.GenericArguments.Add(typeRef.Name);
                                                }
                                            }
                                        }
                                        if (property.HasParameters)
                                        {
                                            foreach (var parameter in property.Parameters)
                                            {
                                                propertyInfo.PropertyParameters.Add(new PropertyParameter(
                                                                                        parameter.Name,
                                                                                        parameter.ParameterType.Name));
                                            }
                                        }
                                        classEntity.PropertyInformation.Add(propertyInfo);
                                    }
                                }
                                ancs.Add(new AssembliesNamespacesClass(assemblyDefinition.Name.Name,
                                                                       type.Namespace,
                                                                       type.Name,
                                                                       classEntity));
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

        #endregion
    }
}
