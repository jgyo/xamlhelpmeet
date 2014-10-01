// file:    UIControlFactory\UIControlFactory.cs
//
// summary: Implements the control factory class
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Data;
using XamlHelpmeet.Extensions;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.UIControlFactory
{
using System.Diagnostics.Contracts;
using System.Linq;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Control factory.
/// </summary>
public class UIControlFactory
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();


    #region Constants
    /// <summary>
    /// The close brace
    /// </summary>
    private const string STR_BRACECLOSE = "}";

    /// <summary>
    /// The brace close escaped.
    /// </summary>
    private const string STR_BRACECLOSEESCAPED = @"\}";

    /// <summary>
    /// The brace open.
    /// </summary>
    private const string STR_BRACEOPEN = "{";

    /// <summary>
    /// The brace open escaped.
    /// </summary>
    private const string STR_BRACEOPENESCAPED = @"\{";

    /// <summary>
    /// Content Binding And String Format.
    /// </summary>
    private const string STR_ContentBindingAndStringFormat =
        " Content=\"{{Binding Path={0}}}\" ContentStringFormat=\"{1}\"";

    /// <summary>
    /// The Content Binding Path Format.
    /// </summary>
    private const string STR_ContentBindingPathFormat =
        " Content=\"{{Binding Path={0}}\"";

    /// <summary>
    /// The Content Format.
    /// </summary>
    private const string STR_ContentFormat = " Content=\"{0}\"";

    /// <summary>
    /// The Content Is Checked Binding Path Mode Format.
    /// </summary>
    private const string STR_ContentIsCheckedBindingPathModeFormat =
        " Content=\"{0}\" IsChecked=\"{{Binding Path={1}, Mode={2}{3}}}\"";

    /// <summary>
    /// The Grid Column Format.
    /// </summary>
    private const string STR_GridColumnFormat = " Grid.Column=\"{0}\"";

    /// <summary>
    /// The Grid Row Format.
    /// </summary>
    private const string STR_GridRowFormat = " Grid.Row=\"{0}\"";

    /// <summary>
    /// The Horizontal Alignment Stretch.
    /// </summary>
    private const string STR_HorizontalAlignmentStretch =
        " HorizontalAlignment=\"Stretch\"";

    /// <summary>
    /// The maximum length.
    /// </summary>
    private const string STR_MaxLengthFormat = " MaxLength=\"{0}\"";

    /// <summary>
    /// The selected date.
    /// </summary>
    private const string STR_SelectedDateBindingFormat =
        " SelectedDate=\"{{Binding Path={0}, Mode=TwoWay}}\"";

    /// <summary>
    /// The selected value.
    /// </summary>
    private const string STR_SelectedValueBindingFormat =
        " SelectedValue=\"{{Binding Path={0}, Mode={1}{2}}}\"";

    /// <summary>
    /// Source Binding Format.
    /// </summary>
    private const string STR_SourceBindingFormat =
        " Source=\"{{Binding Path={0}}}\"";

    /// <summary>
    /// String Format Format.
    /// </summary>
    private const string STR_StringFormatFormat = ", StringFormat={0}";

    /// <summary>
    /// Target Null Value.
    /// </summary>
    private const string STR_TargetNullValue = ", TargetNullValue=''";

    /// <summary>
    /// Text Binding Path Format.
    /// </summary>
    private const string STR_TextBindingPathFormat =
        " Text=\"{{Binding Path={0}{1}}}\"";

    /// <summary>
    /// The Text Binding Path Mode Format.
    /// </summary>
    private const string STR_TextBindingPathMode123Format =
        " Text=\"{{Binding Path={0}, Mode={1}{2}{3}}}\"";

    /// <summary>
    /// The Text Binding Path Mode Format.
    /// </summary>
    private const string STR_TextBindingPathMode12Format =
        " Text=\"{{Binding Path={0}, Mode={1}{2}}}\"";

    /// <summary>
    /// The Text Format.
    /// </summary>
    private const string STR_TextFormat = " Text=\"{0}\"";

    /// <summary>
    /// The update source trigger.
    /// </summary>
    private const string STR_UpdateSourceTrigger =
        ", UpdateSourceTrigger=PropertyChanged";

    /// <summary>
    /// The update source trigger.
    /// </summary>
    private const string STR_UpdateSourceTriggerLostFocus =
        ", UpdateSourceTrigger=LostFocus";

    /// <summary>
    /// The width.
    /// </summary>
    private const string STR_WidthFormat = " Width=\"{0}\"";

    #endregion

    #region Declarations
    /// <summary>
    /// Filename of the save settings file.
    /// </summary>
    private static readonly string saveSettingsFilename = Path.Combine(
                Environment.GetEnvironmentVariable("APPDATA"),
                @"YoderTools\Xaml Helpmeet\xhmDefault.Settings");

    /// <summary>
    /// Pathname of the save settings folder.
    /// </summary>
    private static readonly string _saveSettingsFolderName = Path.Combine(
                Environment.GetEnvironmentVariable("APPDATA"),
                @"YoderTools\Xaml Helpmeet\");

    /// <summary>
    /// The instance.
    /// </summary>
    private static UIControlFactory instance;

    #endregion Declarations

    #region Properties
    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static UIControlFactory Instance
    {
        get
        {
            Contract.Ensures(Contract.Result<UIControlFactory>() != null);
            logger.Debug("Entered member.");

            if (instance != null)
            {
                return instance;
            }

            instance = new UIControlFactory();
            instance.UIControls = instance.Load();

            return instance;
        }
    }

    /// <summary>
    /// Gets or sets the controls.
    /// </summary>
    /// <value>
    /// The user interface controls.
    /// </value>
    public UIControls UIControls
    {
        get;
        set;
    }

    #endregion

    #region UIControl Creators
    /// <summary>
    /// Makes check box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="column">
    /// The column.
    /// </param>
    /// <param name="row">
    /// The row.
    /// </param>
    /// <param name="Content">
    /// The content.
    /// </param>
    /// <param name="Path">
    /// Full pathname of the file.
    /// </param>
    /// <param name="BindingMode">
    /// The binding mode.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeCheckBox(UIPlatform uiPlatform, int? column, int? row,
                               string Content, string Path, BindingMode BindingMode)
    {
        logger.Debug("Entered member.");

        var ctrl = GetUIControl(ControlType.CheckBox, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, column);
        AppendFormat(sb, STR_GridRowFormat, row);
        if (uiPlatform == UIPlatform.WPF)
        {
            AppendFormat(sb, STR_ContentIsCheckedBindingPathModeFormat, Content,
                         Path, BindingMode, BindingMode == BindingMode.TwoWay ?
                         string.Concat(STR_UpdateSourceTrigger, ctrl.BindingPropertyString) :
                         string.Empty);
        }
        else
        {
            AppendFormat(sb, STR_ContentIsCheckedBindingPathModeFormat, Content,
                         Path, BindingMode, BindingMode == BindingMode.TwoWay ?
                         ctrl.BindingPropertyString : string.Empty);
        }

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
    }

    /// <summary>
    /// Makes combo box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Path">
    /// Full pathname of the file.
    /// </param>
    /// <param name="BindingMode">
    /// The binding mode.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeComboBox(UIPlatform uiPlatform, int? Column,
                               int? Row, string Path, BindingMode BindingMode)
    {
        logger.Debug("Entered member.");

        var ctrl = GetUIControl(ControlType.ComboBox, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);

        if (uiPlatform == UIPlatform.WPF)
        {
            AppendFormat(sb, STR_SelectedValueBindingFormat, Path, BindingMode,
                         BindingMode ==
                         BindingMode.TwoWay ? string.Concat(STR_UpdateSourceTrigger,
                                 ctrl.BindingPropertyString) : string.Empty);
        }
        else
        {
            AppendFormat(sb, STR_SelectedValueBindingFormat, Path, BindingMode,
                         BindingMode ==
                         BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty);
        }
        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
    }

    /// <summary>
    /// Makes date picker.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Path">
    /// Full pathname of the file.
    /// </param>
    /// <param name="Width">
    /// The width.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeDatePicker(UIPlatform uiPlatform, int? Column,
                                 int? Row, string Path, int? Width)
    {
        logger.Debug("Entered member.");

        var ctrl = GetUIControl(ControlType.DatePicker, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);
        AppendFormat(sb, STR_WidthFormat, Width);
        AppendFormat(sb, STR_SelectedDateBindingFormat, Path);

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
    }

    /// <summary>
    /// Makes an image.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Path">
    /// Full pathname of the file.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeImage(UIPlatform uiPlatform, int? Column, int? Row,
                            string Path)
    {
        logger.Debug("Entered member.");

        var ctrl = GetUIControl(ControlType.Image, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);
        AppendFormat(sb, STR_SourceBindingFormat, Path);

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
    }

    /// <summary>
    /// Makes a label.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Content">
    /// The content.
    /// </param>
    /// <param name="StringFormat">
    /// The string format.
    /// </param>
    /// <param name="SilverlightVersion">
    /// The silverlight version.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeLabel(UIPlatform uiPlatform, int? Column, int? Row,
                            string Content, string StringFormat, string SilverlightVersion)
    {
        logger.Debug("Entered member.");

        var ctrl = GetUIControl(ControlType.Label, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);

        if (uiPlatform == UIPlatform.WPF || SilverlightVersion.StartsWith("3"))
        {
            if (StringFormat.IsNotNullOrEmpty())
            {
                AppendFormat(sb, STR_ContentBindingAndStringFormat,
                             Content, StringFormat.Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED).
                             Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
            }
            else
            {
                AppendFormat(sb, STR_ContentBindingPathFormat, Content);
            }
        }
        else
        {
            AppendFormat(sb, STR_ContentBindingPathFormat, Content);
        }

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Content);
    }

    /// <summary>
    /// Makes label without binding.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Content">
    /// The content.
    /// </param>
    /// <returns>
    /// XAML for a label.
    /// </returns>
    public string MakeLabelWithoutBinding(UIPlatform uiPlatform,
                                          int? Column, int? Row, string Content)
    {
        var ctrl = GetUIControl(ControlType.Label, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);

        AppendFormat(sb, ctrl.ControlType.ToLower().Contains("label") ?
                     STR_ContentFormat : STR_TextFormat, Content);

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, string.Empty);
    }

    /// <summary>
    /// Makes text block.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="Column">
    /// The column.
    /// </param>
    /// <param name="Row">
    /// The row.
    /// </param>
    /// <param name="Path">
    /// Full pathname of the file.
    /// </param>
    /// <param name="StringFormat">
    /// The string format.
    /// </param>
    /// <param name="SilverlightVersion">
    /// The silverlight version.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeTextBlock(UIPlatform uiPlatform, int? Column, int? Row,
                                string Path, string StringFormat, string SilverlightVersion)
    {
        if (StringFormat.IsNotNullOrEmpty())
        {
            StringFormat = string.Format(STR_StringFormatFormat, StringFormat.
                                         Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED).
                                         Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
        }
        if (uiPlatform == UIPlatform.Silverlight &&
                SilverlightVersion.StartsWith("3"))
        {
            StringFormat = string.Empty;
        }

        var ctrl = GetUIControl(ControlType.TextBlock, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, Column);
        AppendFormat(sb, STR_GridRowFormat, Row);

        AppendFormat(sb, STR_TextBindingPathFormat, Path, StringFormat);

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
    }

    /// <summary>
    /// Makes text box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <param name="column">
    /// The column.
    /// </param>
    /// <param name="row">
    /// The row.
    /// </param>
    /// <param name="path">
    /// Full pathname of the file.
    /// </param>
    /// <param name="bindingMode">
    /// The binding mode.
    /// </param>
    /// <param name="width">
    /// The width.
    /// </param>
    /// <param name="maximumLength">
    /// Length of the maximum.
    /// </param>
    /// <param name="stringFormat">
    /// The string format.
    /// </param>
    /// <param name="isSourceNullable">
    /// true if this UIControlFactory is source nullable.
    /// </param>
    /// <param name="SilverlightVersion">
    /// The silverlight version.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string MakeTextBox(UIPlatform uiPlatform, int? column, int? row,
                              string path,
                              BindingMode bindingMode, int? width, int? maximumLength,
                              string stringFormat, bool isSourceNullable,
                              string SilverlightVersion)
    {
        if (stringFormat.IsNotNullOrEmpty())
        {
            stringFormat = string.Format(STR_StringFormatFormat, stringFormat
                                         .Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED)
                                         .Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
        }
        if (uiPlatform == UIPlatform.Silverlight &&
                SilverlightVersion.StartsWith("3"))
        {
            SilverlightVersion = string.Empty;
        }

        var ctrl = GetUIControl(ControlType.TextBox, uiPlatform);
        var sb = new StringBuilder(1024);

        AppendFormat(sb, STR_GridColumnFormat, column);
        AppendFormat(sb, STR_GridRowFormat, row);

        if (uiPlatform == UIPlatform.WPF)
        {
            AppendFormat(sb, STR_TextBindingPathMode123Format, path, bindingMode,
                         bindingMode == BindingMode.TwoWay ? string.Concat(
                             STR_UpdateSourceTriggerLostFocus,
                             ctrl.BindingPropertyString, stringFormat) : string.Empty,
                         isSourceNullable &&
                         ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue :
                         string.Empty);
        }
        else
        {
            if (SilverlightVersion.StartsWith("3"))
            {
                AppendFormat(sb, STR_TextBindingPathMode12Format, path, bindingMode,
                             bindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString :
                             string.Empty);
            }
            else if (stringFormat.IsNullOrWhiteSpace())
            {
                AppendFormat(sb, STR_TextBindingPathMode123Format, path, bindingMode,
                             bindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString :
                             string.Empty,
                             ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue :
                             string.Empty);
            }
            else
            {
                AppendFormat(sb, STR_TextBindingPathMode123Format, path, bindingMode,
                             bindingMode == BindingMode.TwoWay ? string.Concat(
                                 ctrl.BindingPropertyString, stringFormat) :
                             string.Empty, isSourceNullable &&
                             ctrl.IncludeTargetNullValueForNullableBindings ?
                             STR_TargetNullValue : string.Empty);
            }
        }

        AppendFormat(sb, STR_WidthFormat, width);

        if (width == null && uiPlatform == UIPlatform.Silverlight)
        {
            AppendFormat(sb, STR_HorizontalAlignmentStretch);
        }

        AppendFormat(sb, STR_MaxLengthFormat, maximumLength);

        return ctrl.MakeControlFromDefaults(sb.ToString(), true, path);
    }

    /// <summary>
    /// Appends a format.
    /// </summary>
    /// <param name="sb">
    /// The sb.
    /// </param>
    /// <param name="Format">
    /// Describes the format to use.
    /// </param>
    /// <param name="Args">
    /// A variable-length parameters list containing arguments.
    /// </param>
    private void AppendFormat(StringBuilder sb, string Format,
                              params object[] Args)
    {
        sb.AppendFormat(Format, Args);
    }

    /// <summary>
    /// Appends a format.
    /// </summary>
    /// <param name="sb">
    /// The sb.
    /// </param>
    /// <param name="Format">
    /// Describes the format to use.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    private void AppendFormat(StringBuilder sb, string Format, int? value)
    {
        logger.Debug("Entered member.");

        if (!value.HasValue)
        { return; }

        AppendFormat(sb, Format, value.ToString());
    }

    #endregion UIControl Creators

    #region Methods
    /// <summary>
    /// Gets user interface control.
    /// </summary>
    /// <param name="ControlType">
    /// Type of the control.
    /// </param>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The user interface control.
    /// </returns>
    public UIControl GetUIControl(ControlType ControlType,
                                  UIPlatform uiPlatform)
    {
        return UIControls.GetUIControl(ControlType, uiPlatform);
    }

    /// <summary>
    /// Gets user interface control.
    /// </summary>
    /// <param name="ControlRole">
    /// The control role.
    /// </param>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The user interface control.
    /// </returns>
    public UIControl GetUIControl(UIControlRole ControlRole,
                                  UIPlatform uiPlatform)
    {
        return UIControls.GetUIControl(ControlRole, uiPlatform);
    }

    /// <summary>
    /// Gets user interface controls for platform.
    /// </summary>
    /// <param name="Platform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The user interface controls for platform.
    /// </returns>
    public List<UIControl> GetUIControlsForPlatform(UIPlatform Platform)
    {
        logger.Debug("Entered member.");

        return UIControls.GetUIControlsForPlatform(Platform);
    }

    //!+ Routine to load settings.

    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <returns>
    /// The UIControls.
    /// </returns>
    public UIControls Load()
    {
        logger.Debug("Entered member.");

        if (!Directory.Exists(_saveSettingsFolderName))
        {
            logger.Info("Creating settings folder.");
            Directory.CreateDirectory(_saveSettingsFolderName);
        }

        if (!File.Exists(saveSettingsFilename))
        {
            //+ If the settings file does not exit, create it.
            logger.Info("Creating initial settings file.");
            CreateDefaults();
            Save(false);
        }
        else
        {
            try
            {
                //+ Otherwise start with an empty UIControls
                if (UIControls != null)
                {
                    UIControls.Clear();
                    UIControls = null;
                }

                //+ And deserialize the stream into UIControls.
                using (var fs = new FileStream(saveSettingsFilename, FileMode.Open))
                {
                    UIControls = Deserialize(fs) as UIControls;
                }
            }
            catch (Exception ex)
            {
                //+ If that doesn't work, just use defaults.
                UIUtilities.ShowExceptionMessage("Settings File",
                                                 "Unable to load previous settings file.  Creating new settings file.");
                logger.Debug("Unable to load previous settings file.", ex);
                CreateDefaults();
                Save(false);
            }
        }
        return UIControls;
    }

    /// <summary>
    /// Saves settings file.
    /// </summary>
    /// <param name="ShowSaveMessage">
    /// Show a save message if true.
    /// </param>
    public void Save(bool ShowSaveMessage)
    {
        logger.Debug("Entered member.");

        logger.Debug("Entered member.");

        var listToRemove = new List<UIProperty>();

        foreach (var uicontrol in UIControls)
        {
            // Create a list of control properties that were not
            // configured.
            listToRemove.AddRange(uicontrol.ControlProperties.Where(
                                      obj => obj.PropertyName.IsNullOrEmpty() ||
                                      obj.PropertyName.IsNullOrEmpty() || obj.PropertyName.Equals("ChangeMe") ||
                                      obj.PropertyValue.Equals("ChangeMe")));

            // Remove the non-configured properties.
            foreach (var item in listToRemove)
            {
                uicontrol.ControlProperties.Remove(item);
            }

            listToRemove.Clear();
        }

        try
        {
            using (var fs = new FileStream(saveSettingsFilename, FileMode.Create))
            {
                Serialize(fs, UIControls);
            }

            if (ShowSaveMessage)
            {
                UIUtilities.ShowInformationMessage("Saved Settings File Location",
                                                   String.Format("Settings saved to: {0}", saveSettingsFilename));
            }
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("Exception While Saving Settings",
                                             ex.Message);
            logger.Debug("Exception raised saving settings.", ex);
        }
    }

    #endregion Methods

    #region Create Methods
    /// <summary>
    /// Adds the platforms.
    /// </summary>
    /// <param name="CreateMethod">
    /// The create method.
    /// </param>
    private void AddPlatforms(Func<UIPlatform, UIControl> CreateMethod)
    {
        logger.Debug("Entered member.");

        logger.Debug("Entered member.");

        UIControls.Add(CreateMethod(UIPlatform.Silverlight));
        UIControls.Add(CreateMethod(UIPlatform.WPF));
    }

    /// <summary>
    /// Creates a border.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new border.
    /// </returns>
    private UIControl CreateBorder(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.Border, "Border");
        obj.ControlProperties.Add(new UIProperty("BorderBrush", "LightGray"));
        obj.ControlProperties.Add(new UIProperty("BorderThickness", "1"));
        obj.ControlProperties.Add(new UIProperty("CornerRadius", "10"));
        obj.ControlProperties.Add(new UIProperty("Padding", "10"));
        return obj;
    }

    /// <summary>
    /// Creates check box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new check box.
    /// </returns>
    private UIControl CreateCheckBox(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.CheckBox, "CheckBox")
        {
            IncludeNotifyOnValidationError = true,
            IncludeValidatesOnDataErrors = true,
            IncludeValidatesOnExceptions = true
        };

        return obj;
    }

    /// <summary>
    /// Creates combo box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new combo box.
    /// </returns>
    private UIControl CreateComboBox(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.ComboBox, "ComboBox")
        {
            IncludeNotifyOnValidationError = true,
            IncludeValidatesOnDataErrors = true,
            IncludeValidatesOnExceptions = true
        };

        if (uiPlatform == UIPlatform.WPF)
        {
            obj.ControlProperties.Add(new UIProperty("IsSynchronizedWithCurrentItem",
                                      "True"));
        }
        return obj;
    }

    /// <summary>
    /// Creates data grid.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new data grid.
    /// </returns>
    private UIControl CreateDataGrid(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.DataGrid, "DataGrid");

        if (uiPlatform == UIPlatform.WPF)
        {
            obj.ControlProperties.Add(new UIProperty("AlternationCount", "2"));
        }
        else // UIPlatform.Silverlight
        {
            obj.ControlType = "sdk:DataGrid";
        }

        obj.ControlProperties.Add(new UIProperty("AutoGenerateColumns", "False"));

        return obj;
    }

    /// <summary>
    /// Creates date picker.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new date picker.
    /// </returns>
    private UIControl CreateDatePicker(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.DatePicker,
                                "DatePicker");

        if (uiPlatform == UIPlatform.Silverlight)
        {
            obj.ControlType = "sdk:DatePicker";
        }

        obj.ControlProperties.Add(new UIProperty("SelectedDateFormat", "Short"));

        return obj;
    }

    /// <summary>
    /// Creates the defaults.
    /// </summary>
    private void CreateDefaults()
    {
        logger.Debug("Entered member.");

        UIControls = new UIControls();

        AddPlatforms(CreateBorder);
        AddPlatforms(CreateCheckBox);
        AddPlatforms(CreateComboBox);
        AddPlatforms(CreateDataGrid);
        AddPlatforms(CreateDatePicker);
        AddPlatforms(CreateGrid);
        AddPlatforms(CreateImage);
        AddPlatforms(CreateLabel);
        AddPlatforms(CreateTextBlock);
        AddPlatforms(CreateTextBox);
    }

    /// <summary>
    /// Creates a grid.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new grid.
    /// </returns>
    private UIControl CreateGrid(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        return new UIControl(uiPlatform, UIControlRole.Grid, "Grid");
    }

    /// <summary>
    /// Creates an image.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new image.
    /// </returns>
    private UIControl CreateImage(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        return new UIControl(uiPlatform, UIControlRole.Image, "Image");
    }

    /// <summary>
    /// Creates a label.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new label.
    /// </returns>
    private UIControl CreateLabel(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.Label, "Label");

        if (uiPlatform == UIPlatform.Silverlight)
        {
            obj.ControlType = "sdk:Label";
        }

        return obj;
    }

    /// <summary>
    /// Creates text block.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new text block.
    /// </returns>
    private UIControl CreateTextBlock(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        return new UIControl(uiPlatform, UIControlRole.TextBlock, "TextBlock");
    }

    /// <summary>
    /// Creates text box.
    /// </summary>
    /// <param name="uiPlatform">
    /// The platform.
    /// </param>
    /// <returns>
    /// The new text box.
    /// </returns>
    private UIControl CreateTextBox(UIPlatform uiPlatform)
    {
        logger.Debug("Entered member.");

        var obj = new UIControl(uiPlatform, UIControlRole.TextBox, "TextBox")
        {
            IncludeNotifyOnValidationError = true,
            IncludeValidatesOnDataErrors = true,
            IncludeValidatesOnExceptions = true
        };
        obj.ControlProperties.Add(new UIProperty("HorizontalAlignment", "Left"));
        obj.ControlProperties.Add(new UIProperty("VerticalAlignment", "Top"));

        return obj;
    }

    #endregion Create Methods

    #region Serialization

    /*  Karl Shifflett notes here:

        'The below three functions took FOREVER to correct until I read this thread.
        'this mess is required because the Deserialize method does not load assemblies the way you "think" it would.
        'the below assembly resolve function allows the Deserialize method to find the assembly its in.
        'the thread has the full story.
        '
        'http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/e5f0c371-b900-41d8-9a5b-1052739f2521/
    */

    /// <summary>
    /// Event handler. Called by CurrentDomain for assembly resolve
    /// events.
    /// </summary>
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    /// <param name="e">
    /// Resolve event information.
    /// </param>
    /// <returns>
    /// An Assembly.
    /// </returns>
    private static Assembly CurrentDomain_AssemblyResolve(object sender,
            ResolveEventArgs e)
    {
        var assemblyShortName = e.Name.Split(',')[0];
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var asy in assemblies)
        {
            if (assemblyShortName == asy.FullName.Split(',')[0])
            {
                return asy;
            }
        }
        return null;
    }

    /// <summary>
    /// Deserialize this XamlHelpmeet.UI.UIControlFactory.UIControlFactory to
    /// the given stream.
    /// </summary>
    /// <param name="IncomingData">
    /// Information describing the incoming.
    /// </param>
    /// <returns>
    /// An object.
    /// </returns>
    private object Deserialize(FileStream IncomingData)
    {
        logger.Debug("Entered member.");

        var binaryFormatter = new BinaryFormatter();

        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        object result = binaryFormatter.Deserialize(IncomingData);
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        return result;
    }

    /// <summary>
    /// Serialize this XamlHelpmeet.UI.UIControlFactory.UIControlFactory to
    /// the given stream.
    /// </summary>
    /// <param name="IutputStream">
    /// The iutput stream.
    /// </param>
    /// <param name="Target">
    /// Target for the.
    /// </param>
    private void Serialize(Stream IutputStream, Object Target)
    {
        logger.Debug("Entered member.");

        var binaryFormatter = new BinaryFormatter();
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        binaryFormatter.Serialize(IutputStream, Target);
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
    }

    #endregion Serialization
}
}