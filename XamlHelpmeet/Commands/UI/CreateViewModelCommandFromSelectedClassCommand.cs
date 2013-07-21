// file:	Commands\UI\CreateViewModelCommandFromSelectedClassCommand.cs
//
// summary:	Implements the create view model command from selected class command class
namespace XamlHelpmeet.Commands.UI
{
    #region Imports

    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Windows.Forms;
    using EnvDTE;
    using EnvDTE80;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.Utilities;
    using XamlHelpmeet.UI.ViewModelCreation;
    using XamlHelpmeet.Utility;

    #endregion

    /// <summary>
    ///     Create view model command from selected class command.
    /// </summary>
    /// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
    internal class CreateViewModelCommandFromSelectedClassCommand : CommandBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the
        ///     CreateViewModelCommandFromSelectedClassCommand
        ///     class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="id">The id.</param>
        public CreateViewModelCommandFromSelectedClassCommand(DTE2 application, CommandID id) : base(application, id)
        {
            this.Caption = "Create ViewModel for Class";
            this.CommandName = "CreateViewModelCommandFromSelectedClassCommand";
            this.ToolTip = "Create ViewModel for class.";
        }

        #endregion

        #region Methods (public)

        /// <summary>
        ///     Executes this CreateViewModelCommandFromSelectedClassCommand.
        /// </summary>
        public override void Execute()
        {
            try
            {
                var remoteTypeReflector = new RemoteTypeReflector();
                ClassEntity classEntity =
                    remoteTypeReflector.GetClassEntityFromSelectedClass(
                                                                        this.Application.SelectedItems.Item(1)
                                                                            .ProjectItem.ContainingProject,
                                                                        this.Caption);
                if (classEntity != null)
                {
                    var obj = new CreateViewModelWindow(classEntity,
                                                        this.Application.ActiveWindow.Caption.EndsWith(".vb"));
                    bool? result = obj.ShowDialog();
                    if (result ?? false)
                    {
                        try
                        {
                            Clipboard.Clear();
                            Clipboard.SetText(obj.ViewModelText);
                        }
                        catch //(Exception ex)
                        {
                            // Had to do this to avoid useless exception message when running this code in a 
                            // VPC, since Vista & VPC and the Clipboard don't play nice together sometimes.
                            // the operation works, you just get an exception for no reason.
                        }
                        UIUtilities.ShowInformationMessage("Ready to Paste",
                                                           "Position curosr inside a ViewModel file and execute a paste operation.");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                UIUtilities.ShowInformationMessage("File Not Found",
                                                   "Unable to find an assemble for the file specified. Have you compiled the project?",
                                                   ex.FileName,
                                                   ex.FusionLog);
            }
            catch (Exception ex)
            {
                UIUtilities.ShowExceptionMessage(this.Caption, ex.Message, string.Empty, ex.ToString());
            }
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <returns>
        ///     The status.
        /// </returns>
        public override vsCommandStatus GetStatus()
        {
            if (this.Application.ActiveDocument == null
                || !(this.Application.ActiveDocument.Name.EndsWith("vb")
                     || this.Application.ActiveDocument.Name.EndsWith("cs")))
            {
                return vsCommandStatus.vsCommandStatusUnsupported | vsCommandStatus.vsCommandStatusInvisible;
            }
            return vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
        }

        #endregion
    }
}
