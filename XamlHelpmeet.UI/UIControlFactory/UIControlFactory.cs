// file:	UIControlFactory\UIControlFactory.cs
//
// summary:	Implements the control factory class
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
	/// <summary>
	///     Control factory.
	/// </summary>
	public class UIControlFactory
	{
		#region Constants

		private const string STR_BRACECLOSE = "}";
		private const string STR_BRACECLOSEESCAPED = @"\}";
		private const string STR_BRACEOPEN = "{";
		private const string STR_BRACEOPENESCAPED = @"\{";
		private const string STR_ContentBindingAndStringFormatFormat = " Content=\"{{Binding Path={0}}}\" ContentStringFormat=\"{1}\"";
		private const string STR_ContentBindingPathFormat = " Content=\"{{Binding Path={0}}\"";
		private const string STR_ContentFormat = " Content=\"{0}\"";
		private const string STR_ContentIsCheckedBindingPathModeFormat = " Content=\"{0}\" IsChecked=\"{{Binding Path={1}, Mode={2}{3}}}\"";
		private const string STR_GridColumnFormat = " Grid.Column=\"{0}\"";
		private const string STR_GridRowFormat = " Grid.Row=\"{0}\"";
		private const string STR_HorizontalAlignmentStretch = " HorizontalAlignment=\"Stretch\"";
		private const string STR_MaxLengthFormat = " MaxLength=\"{0}\"";
		private const string STR_SelectedDateBindingFormat = " SelectedDate=\"{{Binding Path={0}, Mode=TwoWay}}\"";
		private const string STR_SelectedValueBindingFormat = " SelectedValue=\"{{Binding Path={0}, Mode={1}{2}}}\"";
		private const string STR_SourceBindingFormat = " Source=\"{{Binding Path={0}}}\"";
		private const string STR_StringFormatFormat = ", StringFormat={0}";
		private const string STR_TargetNullValue = ", TargetNullValue=''";
		private const string STR_TextbindingPathFormat = " Text=\"{{Binding Path={0}{1}}}\"";
		private const string STR_TextBindingPathMode123Format = " Text=\"{{Binding Path={0}, Mode={1}{2}{3}}}\"";
		private const string STR_TextBindingPathMode12Format = " Text=\"{{Binding Path={0}, Mode={1}{2}}}\"";
		private const string STR_TextFormat = " Text=\"{0}\"";
		private const string STR_UpdateSourceTrigger = ", UpdateSourceTrigger=PropertyChanged";
		private const string STR_UpdateSourceTriggerLostFocus = ", UpdateSourceTrigger=LostFocus";
		private const string STR_WidthFormat = " Width=\"{0}\"";

		#endregion

		#region Declarations

		private static readonly string _saveSettingsFilename = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), @"YoderTools\Xaml Helpmeet\xhmDefault.Settings");
		private static readonly string _saveSettingsFolderName = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), @"YoderTools\Xaml Helpmeet\");
		private static UIControlFactory _instance;

		#endregion Declarations

		#region Properties

		/// <summary>
		///     Gets the instance.
		/// </summary>
		/// <value>
		///     The instance.
		/// </value>
		public static UIControlFactory Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = new UIControlFactory();
				_instance.UIControls = _instance.Load();
				return _instance;
			}
		}

		/// <summary>
		///     Gets or sets the controls.
		/// </summary>
		/// <value>
		///     The user interface controls.
		/// </value>
		public UIControls UIControls
		{
			get;
			set;
		}

		#endregion

		#region UIControl Creators

		/// <summary>
		///     Makes check box.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Content">
		///     The content.
		/// </param>
		/// <param name="Path">
		///     Full pathname of the file.
		/// </param>
		/// <param name="BindingMode">
		///     The binding mode.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeCheckBox(UIPlatform uiPlatform, int? Column, int? Row,
			string Content, string Path, BindingMode BindingMode)
		{
			var ctrl = GetUIControl(ControlType.CheckBox, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
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
		///     Makes combo box.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Path">
		///     Full pathname of the file.
		/// </param>
		/// <param name="BindingMode">
		///     The binding mode.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeComboBox(UIPlatform uiPlatform, int? Column,
			int? Row, string Path, BindingMode BindingMode)
		{
			var ctrl = GetUIControl(ControlType.ComboBox, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			if (uiPlatform == UIPlatform.WPF)
			{
				AppendFormat(sb, STR_SelectedValueBindingFormat, Path, BindingMode, BindingMode ==
					BindingMode.TwoWay ? string.Concat(STR_UpdateSourceTrigger,
					ctrl.BindingPropertyString) : string.Empty);
			}
			else
			{
				AppendFormat(sb, STR_SelectedValueBindingFormat, Path, BindingMode, BindingMode ==
					BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty);
			}
			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		/// <summary>
		///     Makes date picker.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Path">
		///     Full pathname of the file.
		/// </param>
		/// <param name="Width">
		///     The width.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeDatePicker(UIPlatform uiPlatform, int? Column,
			int? Row, string Path, int? Width)
		{
			var ctrl = GetUIControl(ControlType.DatePicker, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
			AppendFormat(sb, STR_WidthFormat, Width);
			AppendFormat(sb, STR_SelectedDateBindingFormat, Path);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		/// <summary>
		///     Makes an image.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Path">
		///     Full pathname of the file.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeImage(UIPlatform uiPlatform, int? Column, int? Row,
			string Path)
		{
			var ctrl = GetUIControl(ControlType.Image, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
			AppendFormat(sb, STR_SourceBindingFormat, Path);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		/// <summary>
		///     Makes a label.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Content">
		///     The content.
		/// </param>
		/// <param name="StringFormat">
		///     The string format.
		/// </param>
		/// <param name="SilverlightVersion">
		///     The silverlight version.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeLabel(UIPlatform uiPlatform, int? Column, int? Row,
			string Content, string StringFormat, string SilverlightVersion)
		{
			var ctrl = GetUIControl(ControlType.Label, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			if (uiPlatform == UIPlatform.WPF || SilverlightVersion.StartsWith("3"))
			{
				if (StringFormat.IsNotNullOrEmpty())
				{
					AppendFormat(sb, STR_ContentBindingAndStringFormatFormat,
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
		///     Makes label without binding.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Content">
		///     The content.
		/// </param>
		/// <returns>
		///     XAML for a label.
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
		///     Makes text block.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="Column">
		///     The column.
		/// </param>
		/// <param name="Row">
		///     The row.
		/// </param>
		/// <param name="Path">
		///     Full pathname of the file.
		/// </param>
		/// <param name="StringFormat">
		///     The string format.
		/// </param>
		/// <param name="SilverlightVersion">
		///     The silverlight version.
		/// </param>
		/// <returns>
		///     .
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
			if (uiPlatform == UIPlatform.Silverlight && SilverlightVersion.StartsWith("3"))
			{
				StringFormat = string.Empty;
			}

			var ctrl = GetUIControl(ControlType.TextBlock, uiPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			AppendFormat(sb, STR_TextbindingPathFormat, Path, StringFormat);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		/// <summary>
		///     Makes text box.
		/// </summary>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <param name="column">
		///     The column.
		/// </param>
		/// <param name="row">
		///     The row.
		/// </param>
		/// <param name="path">
		///     Full pathname of the file.
		/// </param>
		/// <param name="bindingMode">
		///     The binding mode.
		/// </param>
		/// <param name="width">
		///     The width.
		/// </param>
		/// <param name="maximumLength">
		///     Length of the maximum.
		/// </param>
		/// <param name="stringFormat">
		///     The string format.
		/// </param>
		/// <param name="isSourceNullable">
		///     true if this UIControlFactory is source nullable.
		/// </param>
		/// <param name="SilverlightVersion">
		///     The silverlight version.
		/// </param>
		/// <returns>
		///     .
		/// </returns>
		public string MakeTextBox(UIPlatform uiPlatform, int? column, int? row, string path,
			BindingMode bindingMode, int? width, int? maximumLength, string stringFormat, bool isSourceNullable,
			string SilverlightVersion)
		{
			if (stringFormat.IsNotNullOrEmpty())
			{
				stringFormat = string.Format(STR_StringFormatFormat, stringFormat
					.Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED)
					.Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
			}
			if (uiPlatform == UIPlatform.Silverlight && SilverlightVersion.StartsWith("3"))
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
					bindingMode == BindingMode.TwoWay ? string.Concat(STR_UpdateSourceTriggerLostFocus,
						ctrl.BindingPropertyString, stringFormat) : string.Empty, isSourceNullable &&
						ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue : string.Empty);
			}
			else
			{
				if (SilverlightVersion.StartsWith("3"))
				{
					AppendFormat(sb, STR_TextBindingPathMode12Format, path, bindingMode,
						bindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty);
				}
				else if (stringFormat.IsNullOrWhiteSpace())
				{
					AppendFormat(sb, STR_TextBindingPathMode123Format, path, bindingMode,
						bindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty,
						ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue : string.Empty);
				}
				else
				{
					AppendFormat(sb, STR_TextBindingPathMode123Format, path, bindingMode,
						bindingMode == BindingMode.TwoWay ? string.Concat(ctrl.BindingPropertyString, stringFormat) :
						string.Empty, isSourceNullable && ctrl.IncludeTargetNullValueForNullableBindings ?
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

		private void AppendFormat(StringBuilder sb, string Format, params object[] Args)
		{
			sb.AppendFormat(Format, Args);
		}

		private void AppendFormat(StringBuilder sb, string Format, int? value)
		{
			if (!value.HasValue)
				return;

			AppendFormat(sb, Format, value.ToString());
		}

		#endregion UIControl Creators

		#region Methods

		/// <summary>
		///     Gets user interface control.
		/// </summary>
		/// <param name="ControlType">
		///     Type of the control.
		/// </param>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <returns>
		///     The user interface control.
		/// </returns>
		public UIControl GetUIControl(ControlType ControlType, UIPlatform uiPlatform)
		{
			return UIControls.GetUIControl(ControlType, uiPlatform);
		}

		/// <summary>
		///     Gets user interface control.
		/// </summary>
		/// <param name="ControlRole">
		///     The control role.
		/// </param>
		/// <param name="uiPlatform">
		///     The platform.
		/// </param>
		/// <returns>
		///     The user interface control.
		/// </returns>
		public UIControl GetUIControl(UIControlRole ControlRole, UIPlatform uiPlatform)
		{
			return UIControls.GetUIControl(ControlRole, uiPlatform);
		}

		/// <summary>
		///     Gets user interface controls for platform.
		/// </summary>
		/// <param name="Platform">
		///     The platform.
		/// </param>
		/// <returns>
		///     The user interface controls for platform.
		/// </returns>
		public List<UIControl> GetUIControlsForPlatform(UIPlatform Platform)
		{
			return UIControls.GetUIControlsForPlatform(Platform);
		}

		//!+ Routine to load settings.

		/// <summary>
		///     Gets the settings.
		/// </summary>
		/// <returns>
		///     .
		/// </returns>
		public UIControls Load()
		{
			if (!Directory.Exists(_saveSettingsFolderName))
			{
				Directory.CreateDirectory(_saveSettingsFolderName);
			}

			if (!File.Exists(_saveSettingsFilename))
			{
				//+ If the settings file does not exit, create it.
				CreateDefaults();
				Save(false);
				//UIUtilities.ShowExceptionMessage("Settings File Created", "Your settings file has been created for you.  You can configure your settings using the Set Control Defaults command.");
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
					using (var fs = new FileStream(_saveSettingsFilename, FileMode.Open))
					{
						UIControls = Deserialize(fs) as UIControls;
					}
				}
				catch (Exception ex)
				{
					//+ If that doesn't work, just use defaults.
					UIUtilities.ShowExceptionMessage("Settings File", "Unable to load previous settings file.  Creating new settings file.",
						string.Empty, ex.ToString());
					CreateDefaults();
					Save(false);
				}
			}
			return UIControls;
		}

		/// <summary>
		///     Saves settings file.
		/// </summary>
		/// <param name="ShowSaveMessage">
		///     The show save message to save.
		/// </param>
		public void Save(bool ShowSaveMessage)
		{
			var listToRemove = new List<UIProperty>();

			foreach (var uicontrol in UIControls)
			{
				foreach (var obj in uicontrol.ControlProperties)
				{
					if (obj.PropertyName.IsNullOrEmpty() || obj.PropertyName.IsNullOrEmpty() ||
						obj.PropertyName.Equals("ChangeMe") || obj.PropertyValue.Equals("ChangeMe"))
					{
						listToRemove.Add(obj);
					}
				}

				foreach (var item in listToRemove)
				{
					uicontrol.ControlProperties.Remove(item);
				}
				listToRemove.Clear();
			}

			try
			{
				using (var fs = new FileStream(_saveSettingsFilename, FileMode.Create))
				{
					Serialize(fs, UIControls);
				}

				if (ShowSaveMessage)
				{
					UIUtilities.ShowInformationMessage("Saved Settings File Location",
						String.Format("Settings saved to: {0}", _saveSettingsFilename));
				}
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("Exception While Saving Settings",
					ex.Message, string.Empty, ex.ToString());
			}
		}

		#endregion Methods

		#region Create Methods

		private void AddPlatforms(Func<UIPlatform, UIControl> CreateMethod)
		{
			UIControls.Add(CreateMethod(UIPlatform.Silverlight));
			UIControls.Add(CreateMethod(UIPlatform.WPF));
		}

		private UIControl CreateBorder(UIPlatform uiPlatform)
		{
			var obj = new UIControl(uiPlatform, UIControlRole.Border, "Border");
			obj.ControlProperties.Add(new UIProperty("BorderBrush", "LightGray"));
			obj.ControlProperties.Add(new UIProperty("BorderThickness", "1"));
			obj.ControlProperties.Add(new UIProperty("CornerRadius", "10"));
			obj.ControlProperties.Add(new UIProperty("Padding", "10"));
			return obj;
		}

		private UIControl CreateCheckBox(UIPlatform uiPlatform)
		{
			var obj = new UIControl(uiPlatform, UIControlRole.CheckBox, "CheckBox")
			{
				IncludeNotifyOnValidationError = true,
				IncludeValidatesOnDataErrors = true,
				IncludeValidatesOnExceptions = true
			};

			return obj;
		}

		private UIControl CreateComboBox(UIPlatform uiPlatform)
		{
			var obj = new UIControl(uiPlatform, UIControlRole.ComboBox, "ComboBox")
			{
				IncludeNotifyOnValidationError = true,
				IncludeValidatesOnDataErrors = true,
				IncludeValidatesOnExceptions = true
			};

			if (uiPlatform == UIPlatform.WPF)
			{
				obj.ControlProperties.Add(new UIProperty("IsSynchronizedWithCurrentItem", "True"));
			}
			return obj;
		}

		private UIControl CreateDataGrid(UIPlatform uiPlatform)
		{
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

		private UIControl CreateDatePicker(UIPlatform uiPlatform)
		{
			var obj = new UIControl(uiPlatform, UIControlRole.DatePicker, "DatePicker");

			if (uiPlatform == UIPlatform.Silverlight)
			{
				obj.ControlType = "sdk:DatePicker";
			}

			obj.ControlProperties.Add(new UIProperty("SelectedDateFormat", "Short"));

			return obj;
		}

		private void CreateDefaults()
		{
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

		private UIControl CreateGrid(UIPlatform uiPlatform)
		{
			return new UIControl(uiPlatform, UIControlRole.Grid, "Grid");
		}

		private UIControl CreateImage(UIPlatform uiPlatform)
		{
			return new UIControl(uiPlatform, UIControlRole.Image, "Image");
		}

		private UIControl CreateLabel(UIPlatform uiPlatform)
		{
			var obj = new UIControl(uiPlatform, UIControlRole.Label, "Label");

			if (uiPlatform == UIPlatform.Silverlight)
			{
				obj.ControlType = "sdk:Label";
			}

			return obj;
		}

		private UIControl CreateTextBlock(UIPlatform uiPlatform)
		{
			return new UIControl(uiPlatform, UIControlRole.TextBlock, "TextBlock");
		}

		private UIControl CreateTextBox(UIPlatform uiPlatform)
		{
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

		/*	Karl Shifflett notes here:

			'The below three functions took FOREVER to correct until I read this thread.
			'this mess is required because the Deserialize method does not load assemblies the way you "think" it would.
			'the below assembly resolve function allows the Deserialize method to find the assembly its in.
			'the thread has the full story.
			'
			'http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/e5f0c371-b900-41d8-9a5b-1052739f2521/
		 */

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
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

		private object Deserialize(FileStream IncomingData)
		{
			var binaryFormatter = new BinaryFormatter();

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			object result = binaryFormatter.Deserialize(IncomingData);
			AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
			return result;
		}

		private void Serialize(Stream IutputStream, Object Target)
		{
			var binaryFormatter = new BinaryFormatter();
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			binaryFormatter.Serialize(IutputStream, Target);
			AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
		}

		#endregion Serialization
	}
}