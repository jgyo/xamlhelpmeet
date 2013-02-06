using EnvDTE;
using EnvDTE80;
using System;
using System.ComponentModel.Design;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
    /// <summary>
    /// 	Group into border with stack panel horizontal root.
    /// </summary>
    /// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
    public class GroupIntoBorderWithStackPanelHorizontalRoot : CommandBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GroupIntoBorderWithStackPanelHorizontalRoot
        /// class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="id">The id.</param>
        public GroupIntoBorderWithStackPanelHorizontalRoot(DTE2 application, CommandID id)
            : base(application, id)
        {
            Caption = "Border With Root StackPanel - Horizontall";
            CommandName = "GroupIntoBorderWithStackPanelHorizontalRoot";
            ToolTip = "Group selection into a border with root stackpanel horizontal being added.";
        }

        #endregion

        /// <summary>
        /// 	Determine if we can execute.
        /// </summary>
        /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.CanExecute(vsCommandExecOption)"/>
        public override bool CanExecute(vsCommandExecOption executeOption)
        {
            return base.CanExecute(executeOption) && IsTextSelected();
        }

        /// <summary>
        /// 	Executes this CommandBase.
        /// </summary>
        /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
        public override void Execute()
        {
            try
            {
                GroupInto("<Border>\r\n<StackPanel Orientation=\"Horizontal\">\r\n", "</StackPanel>\r\n</Border>\r\n");
            }
            catch (Exception ex)
            {
                UIUtilities.ShowExceptionMessage("Group Into " + Caption, ex.Message, String.Empty, ex.ToString());
            }
        }

        /// <summary>
        /// 	Gets the status.
        /// </summary>
        /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()"/>
        public override vsCommandStatus GetStatus()
        {
            // This will add vsCommandStatusEnabled to vsCommandStatusSupported,
            // if IsTextSelected() returns true. Otherwise or'ing with
            // vsCommandStatusUnsupported leaves vsCommandStatusSupported
            // unchanged.
            return vsCommandStatus.vsCommandStatusSupported |
                (IsTextSelected() ?
                vsCommandStatus.vsCommandStatusEnabled :
                vsCommandStatus.vsCommandStatusUnsupported);
        }
    }
}