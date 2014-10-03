namespace XamlHelpmeet.ReflectionLoader
{
#region Imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

using NLog;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.Model;

using YoderZone.Extensions.NLog;

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
// ReSharper disable once ClassNeverInstantiated.Global
public class RemoteWorker : MarshalByRefObject
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    static RemoteWorker()
    {
        var settingsConfig = SettingsHelper.NewConfiguration("XamlHelpmeet",
                             "YoderZone");
        var fileTarget = FileTargetFactory.CreateFileTarget("xhmRemoteFileTarget",
                         "XHM${shortdate}.log", settingsConfig);
        var loggingRule = RuleFactory.CreateRule("XamlHelpmeet.*", fileTarget);
        settingsConfig.AddTarget(fileTarget, true);
        settingsConfig.AddRule("xhmRemoteRule", loggingRule);
    }

    #region Methods (public)

    public string GetAssemblyFullPath(string TargetProjectPath,
                                      string AssemblyName)
    {
        if (File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".dll")))
        {
            return Path.Combine(TargetProjectPath, AssemblyName, ".dll");
        }
        return File.Exists(Path.Combine(TargetProjectPath, AssemblyName, ".exe"))
               ? Path.Combine(TargetProjectPath, AssemblyName, ".exe")
               : string.Empty;
    }

    public RemoteResponse<ClassInformationList>
    GetClassEntityFromUserSelectedClass(string assemblyPath,
                                        bool isSilverlight,
                                        IEnumerable<string> references)
    {
        Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(
                assemblyPath));
        Contract.Requires<ArgumentNullException>(references != null);
        Contract.Ensures(Contract.Result<RemoteResponse<ClassInformationList>>()
                         != null);

        logger.Trace("assemblyPath: {0}", assemblyPath);
        logger.Trace("isSilverlight: {0}", isSilverlight);
        var enumerable = references as IList<string> ?? references.ToList();
        logger.Trace("references: {0}", enumerable);

        try
        {
            var ancs = new ClassInformationList();
            string targetProjectPath = Path.GetDirectoryName(assemblyPath);
            logger.Trace("targetProjectPath: {0}", targetProjectPath);

            AssemblyDefinition targetAssemblyDefinition =
                AssemblyDefinition.ReadAssembly(assemblyPath);
            logger.Trace("targetAssemblyDefinition: {0}", targetAssemblyDefinition);

            var assembliesToLoad = new Hashtable { { assemblyPath.ToLower(), null } };

            // Load this assembly
            foreach (var item in
                     targetAssemblyDefinition.MainModule.AssemblyReferences)
            {
                logger.Trace("item: {0}", item);

                if (!item.IsNotMicrosoftAssembly())
                {
                    continue;
                }

                string assemblyFullPath = this.GetAssemblyFullPath(targetProjectPath,
                                          item.Name);
                logger.Trace("assemblyFullPath: {0}", assemblyFullPath);

                if (assemblyFullPath.IsNotNullOrEmpty()
                        && assembliesToLoad.ContainsKey(assemblyFullPath.ToLower()) == false)
                {
                    assembliesToLoad.Add(assemblyFullPath.ToLower(), null);
                }
            }

            // Load up all assemblies referenced in the project, but that are not
            // loaded yet.
            foreach (var name in enumerable)
            {
                logger.Trace("name: {0}", name);

                if (!assembliesToLoad.ContainsKey(Path.GetFileName(name.ToLower())))
                {
                    assembliesToLoad.Add(name.ToLower(), null);
                }
            }

            string failedAssemblies = string.Empty;
            var status = ResponseStatus.Failed;
            foreach (string assemblyToLoadPath in assembliesToLoad.Keys)
            {
                logger.Trace("assemblyToLoadPath: {0}", assemblyToLoadPath);

                targetAssemblyDefinition = AssemblyDefinition.ReadAssembly(
                                               assemblyToLoadPath);
                logger.Trace("targetAssemblyDefinition: {0}", targetAssemblyDefinition);

                Exception ex;
                this.LoadAssemblyClasses(targetAssemblyDefinition, isSilverlight, ancs,
                                         out ex, assembliesToLoad);

                if (ex == null)
                {
                    status = ResponseStatus.Success;
                    logger.Trace("status: {0}", status);
                    continue;
                }

                failedAssemblies += String.Format("{0}{1}", targetAssemblyDefinition.Name,
                                                  Environment.NewLine);
                // return new RemoteResponse<ClassInformationList>(null, ResponseStatus.Exception, ex, String.Format("Unable to load types from target assembly: {0}", targetAssemblyDefinition.Name));
            }

            logger.Trace("failedAssemblies: {0}", failedAssemblies);
            logger.Trace("ancs: {0}", ancs);
            logger.Trace("status: {0}", status);

            return new RemoteResponse<ClassInformationList>(ancs, status,
                    failedAssemblies);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    #endregion

    #region Methods (private)

    private bool CanWrite(PropertyDefinition Property)
    {
        logger.Debug("Entered member.");

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
    /// <returns>
    ///     All properties for the TypedDefinition loaded in a List&lt;
    ///     PropertyDefinition&gt;
    /// </returns>
    private string FormatPropertyTypeName(PropertyDefinition property)
    {
        logger.Debug("Entered member.");

        string name = property.PropertyType.Name;
        string fullName = property.PropertyType.FullName;
        if (name.Contains("`") == false)
        {
            return name;
        }
        name = name.Remove(name.IndexOf("`", System.StringComparison.Ordinal));

        if (!(property.PropertyType is GenericInstanceType)
                || fullName.IndexOf(">", System.StringComparison.Ordinal) == -1)
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

    private IEnumerable<PropertyDefinition> GetAllPropertiesForType(
        TypeDefinition assemblyType,
        Hashtable assembliesToLoad)
    {
        var returnValue = assemblyType.Properties.ToList();

        if (assemblyType.BaseType == assemblyType.Module.Import(typeof(object))
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
                var assemblyNameReference = assemblyType.BaseType.Scope as
                                            AssemblyNameReference;

                if (assemblyNameReference != null)
                {
                    baseTypeAssemblyName = assemblyNameReference.Name.ToLower();
                }
            }

            if (baseTypeAssemblyName.IsNullOrWhiteSpace())
            {
                AssemblyDefinition targetAssemblyDefinition = (from string assemblyName in
                        assembliesToLoad.Keys
                        where assemblyName.EndsWith(baseTypeAssemblyName) ||
                        assemblyName.IndexOf(baseTypeAssemblyName,
                                             System.StringComparison.Ordinal) > -1
                        select AssemblyDefinition.ReadAssembly(assemblyName)).FirstOrDefault();

                if (targetAssemblyDefinition != null)
                {
                    foreach (var baseTypeDefinition in
                             targetAssemblyDefinition.MainModule.Types)
                    {
                        if (!baseTypeDefinition.IsClass ||
                                baseTypeDefinition.Name != assemblyType.BaseType.Name)
                        {
                            continue;
                        }
                        returnValue.AddRange(this.GetAllPropertiesForType(baseTypeDefinition,
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
                                     ClassInformationList ancs,
                                     out Exception exOut,
                                     Hashtable assembliesToLoad)
    {
        exOut = null;

        try
        {
            foreach (var type in assemblyDefinition.MainModule.Types)
            {
                if (type.IsPublic && type.IsClass && !type.IsAbstract &&
                        !type.Name.Contains("<Module>")
                        && !type.Name.Contains("AnonymousType") && !type.Name.StartsWith("_")
                        && !type.Name.EndsWith("AssemblyInfo"))
                {
                    bool previouslyLoaded = ancs.Any(anc => type.Name == anc.TypeName &&
                                                     type.Namespace == anc.Namespace &&
                                                     assemblyDefinition.Name.Name == anc.AssemblyName);

                    if (!previouslyLoaded)
                    {
                        if (type.BaseType == null || type.BaseType.Name != "MulticastDelegate")
                        {
                            var classEntity = new ClassEntity(type.Name, isSilverlight);

                            foreach (
                                var property in
                                this.GetAllPropertiesForType(type, assembliesToLoad))
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
                            ancs.Add(new ClassInformation(assemblyDefinition.Name.Name,
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
