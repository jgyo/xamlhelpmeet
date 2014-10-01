// Type: VSLangProj.PrjKind
// Assembly: VSLangProj, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5562100C-4E47-428E-9315-BA04C3319B0C
// Assembly location: C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\PublicAssemblies\VSLangProj.dll

namespace XamlHelpmeet.Utility
{
/// <summary>
/// This static class is to replace the utility of a class by the same name in the VSLangProj
/// namespace. The other class causes build errors due to attributes it uses to make it usable
/// by unmanaged code.
/// </summary>
public static class PrjKind
{
    /// <summary>
    /// VB Project Id.
    /// </summary>
    public const string prjKindVBProject =
        "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";

    /// <summary>
    /// C# Project Id.
    /// </summary>
    public const string prjKindCSharpProject =
        "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

    /// <summary>
    /// VBA Project Id.
    /// </summary>
    public const string prjKindVSAProject =
        "{13B7A3EE-4614-11D3-9BC7-00C04F79DE25}";
}
}