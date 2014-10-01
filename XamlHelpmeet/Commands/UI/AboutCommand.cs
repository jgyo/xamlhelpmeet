// file:    Commands\UI\AboutCommand.cs
//
// summary: Implements the about command class
using System;

using XamlHelpmeet.UI;
using EnvDTE80;

using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.UI
{
using NLog;

using XamlHelpmeet.UI.About;

using YoderZone.Extensions.NLog;

/// <summary>
///     About command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class AboutCommand : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();
    /// <summary>
    /// Initializes a new instance of the AboutCommand class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public AboutCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "About Xaml Helpmeet";
        CommandName = String.Format("AboutCommand");
        ToolTip = "About Xaml Helpmeet";
    }

    /// <summary>
    ///     Executes this CommandBase.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        var obj = new AboutWindow();
        obj.ShowDialog();
    }
}
}
