using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Data;
using XamlHelpmeet.Extentions;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.UIControlFactory
{
	public class UIControlFactory
	{
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
		private const string STR_TextbindingPathFormat = "Text=\"{{Binding Path={0}{1}}}\"";
		private const string STR_TextBindingPathMode123Format = "Text\"{{Binding Path={0}, Mode={1}{2}{3}}}\"";
		private const string STR_TextBindingPathMode12Format = "Text\"{{Binding Path={0}, Mode={1}{2}}}\"";
		private const string STR_TextFormat = " Text=\"{0}\"";
		private const string STR_UpdateSourceTrigger = ", UpdateSourceTrigger=PropertyChanged";
		private const string STR_UpdateSourceTriggerLostFocus = ", UpdateSourceTrigger=LostFocus";
		private const string STR_WidthFormat = " Width=\"{0}\"";

		#region Declarations

		private static readonly string _saveSettingsFilename = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), @"YoderTools\Xaml Helpmeet\XPTV11.Settings");
		private static readonly string _saveSettingsFolderName = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), @"YoderTools\Xaml Helpmeet\");
		private static UIControlFactory _instance;

		#endregion Declarations

		public static UIControlFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new UIControlFactory();
					_instance.UIControls = _instance.Load();
				}
				return _instance;
				
			}
		}

		public UIControls UIControls
		{
			get;
			set;
		}

		#region UIControl Creators

		public string MakeCheckBox(UIPlatform UIPlatform, int? Column, int? Row,
			string Content, string Path, BindingMode BindingMode)
		{
			var ctrl = GetUIControl(ControlType.CheckBox, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
			if (UIPlatform == UIPlatform.WPF)
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

		public string MakeComboBox(UIPlatform UIPlatform, int? Column,
			int? Row, string Path, BindingMode BindingMode)
		{
			var ctrl = GetUIControl(ControlType.ComboBox, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			if (UIPlatform == UIPlatform.WPF)
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

		public string MakeDatePicker(UIPlatform UIPlatform, int? Column,
			int? Row, string Path, int? Width)
		{
			var ctrl = GetUIControl(ControlType.DatePicker, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
			AppendFormat(sb, STR_WidthFormat, Width);
			AppendFormat(sb, STR_SelectedDateBindingFormat, Path);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		public string MakeImage(UIPlatform UIPlatform, int? Column, int? Row,
			string Path)
		{
			var ctrl = GetUIControl(ControlType.Image, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);
			AppendFormat(sb, STR_SourceBindingFormat, Path);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		public string MakeLabel(UIPlatform UIPlatform, int? Column, int? Row,
			string Content, string StringFormat, string SilverlightVersion)
		{
			var ctrl = GetUIControl(ControlType.Label, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			if (UIPlatform == UIPlatform.WPF || SilverlightVersion.StartsWith("3"))
			{
				if (StringFormat.IsNullOrEmpty())
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

		public string MakeLabelWithoutBinding(UIPlatform UIPlatform,
			int? Column, int? Row, string Content)
		{
			var ctrl = GetUIControl(ControlType.Label, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			AppendFormat(sb, ctrl.ControlType.ToLower().Contains("label") ?
				STR_ContentFormat : STR_TextFormat, Content);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, string.Empty);
		}

		public string MakeTextBlock(UIPlatform UIPlatform, int? Column, int? Row,
			string Path, string StringFormat, string SilverlightVersion)
		{
			if (StringFormat.IsNotNullOrEmpty())
			{
				StringFormat = string.Format(STR_StringFormatFormat, StringFormat.
					Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED).
					Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
			}
			if (UIPlatform == UIPlatform.Silverlight && SilverlightVersion.StartsWith("3"))
			{
				SilverlightVersion = string.Empty;
			}

			var ctrl = GetUIControl(ControlType.TextBlock, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			AppendFormat(sb, STR_TextbindingPathFormat, Path, StringFormat);

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
		}

		public string MakeTextBox(UIPlatform UIPlatform, int? Column, int? Row, string Path,
			BindingMode BindingMode, int? Width, int? MaximumLength, string StringFormat, bool IsSourceNullable,
			string SilverlightVersion)
		{
			if (StringFormat.IsNotNullOrEmpty())
			{
				StringFormat = string.Format(STR_StringFormatFormat, StringFormat
					.Replace(STR_BRACEOPEN, STR_BRACEOPENESCAPED)
					.Replace(STR_BRACECLOSE, STR_BRACECLOSEESCAPED));
			}
			if (UIPlatform == UIPlatform.Silverlight && SilverlightVersion.StartsWith("3"))
			{
				SilverlightVersion = string.Empty;
			}

			var ctrl = GetUIControl(ControlType.TextBlock, UIPlatform);
			var sb = new StringBuilder(1024);

			AppendFormat(sb, STR_GridColumnFormat, Column);
			AppendFormat(sb, STR_GridRowFormat, Row);

			if (UIPlatform == UIPlatform.WPF)
			{
				AppendFormat(sb, STR_TextBindingPathMode123Format, Path, BindingMode,
					BindingMode == BindingMode.TwoWay ? string.Concat(STR_UpdateSourceTriggerLostFocus,
						ctrl.BindingPropertyString, StringFormat) : string.Empty, IsSourceNullable &&
						ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue : string.Empty);
			}
			else
			{
				if (SilverlightVersion.StartsWith("3"))
				{
					AppendFormat(sb, STR_TextBindingPathMode12Format, Path, BindingMode,
						BindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty);
				}
				else if (StringFormat.IsNullOrWhiteSpace())
				{
					AppendFormat(sb, STR_TextBindingPathMode123Format, Path, BindingMode,
						BindingMode == BindingMode.TwoWay ? ctrl.BindingPropertyString : string.Empty,
						ctrl.IncludeTargetNullValueForNullableBindings ? STR_TargetNullValue : string.Empty);
				}
				else
				{
					AppendFormat(sb, STR_TextBindingPathMode123Format, Path, BindingMode,
						BindingMode == BindingMode.TwoWay ? string.Concat(ctrl.BindingPropertyString, StringFormat) :
						string.Empty, IsSourceNullable && ctrl.IncludeTargetNullValueForNullableBindings ?
						STR_TargetNullValue : string.Empty);
				}

				AppendFormat(sb, STR_WidthFormat, Width);

				if (Width == null && UIPlatform == UIPlatform.Silverlight)
				{
					AppendFormat(sb, STR_HorizontalAlignmentStretch);
				}

				AppendFormat(sb, STR_MaxLengthFormat, MaximumLength);
			}

			return ctrl.MakeControlFromDefaults(sb.ToString(), true, Path);
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

		public UIControl GetUIControl(ControlType ControlType, UIPlatform UIPlatform)
		{
			return UIControls.GetUIControl(ControlType, UIPlatform);
		}

		public UIControl GetUIControl(UIControlRole ControlRole, UIPlatform UIPlatform)
		{
			return UIControls.GetUIControl(ControlRole, UIPlatform);
		}

		public List<UIControl> GetUIControlsForPlatform(UIPlatform Platform)
		{
			return UIControls.GetUIControlsForPlatform(Platform);
		}

		//!+ Routine to instantiate UIControls.
		public UIControls Load()
		{
			if (!Directory.Exists(_saveSettingsFolderName))
			{
				Directory.CreateDirectory(_saveSettingsFolderName);
			}

			if (!File.Exists(_saveSettingsFilename))
			{
				//+ If a file exits, create the defaults to
				//+ create the UIControls.
				CreateDefaults();
				Save(false);
				UIUtilities.ShowExceptionMessage("Settings File Created", "Your settings file has been created for you.  You can configure your settings using the Set Control Defaults command.");
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

		private UIControl CreateBorder(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.Border, "Border");
			obj.ControlProperties.Add(new UIProperty("BorderBrush", "LightGray"));
			obj.ControlProperties.Add(new UIProperty("BorderThickness", "1"));
			obj.ControlProperties.Add(new UIProperty("CornerRadius", "10"));
			obj.ControlProperties.Add(new UIProperty("Padding", "10"));
			return obj;
		}

		private UIControl CreateCheckBox(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.CheckBox, "CheckBox")
			{
				IncludeNotifyOnValidationError = true,
				IncludeValidatesOnDataErrors = true,
				IncludeValidatesOnExceptions = true
			};

			return obj;
		}

		private UIControl CreateComboBox(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.ComboBox, "ComboBox")
			{
				IncludeNotifyOnValidationError = true,
				IncludeValidatesOnDataErrors = true,
				IncludeValidatesOnExceptions = true
			};

			if (UIPlatform == UIPlatform.WPF)
			{
				obj.ControlProperties.Add(new UIProperty("IsSynchronizedWithCurrentItem", "True"));
			}
			return obj;
		}

		private UIControl CreateDataGrid(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.DataGrid, "DataGrid");

			if (UIPlatform == UIPlatform.WPF)
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

		private UIControl CreateDatePicker(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.DatePicker, "DatePicker");

			if (UIPlatform == UIPlatform.Silverlight)
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

		private UIControl CreateGrid(UIPlatform UIPlatform)
		{
			return new UIControl(UIPlatform, UIControlRole.Grid, "Grid");
		}

		private UIControl CreateImage(UIPlatform UIPlatform)
		{
			return new UIControl(UIPlatform, UIControlRole.Image, "Image");
		}

		private UIControl CreateLabel(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.Label, "Label");

			if (UIPlatform == UIPlatform.Silverlight)
			{
				obj.ControlType = "sdk:Label";
			}

			return obj;
		}

		private UIControl CreateTextBlock(UIPlatform UIPlatform)
		{
			return new UIControl(UIPlatform, UIControlRole.TextBlock, "TextBlock");
		}

		private UIControl CreateTextBox(UIPlatform UIPlatform)
		{
			var obj = new UIControl(UIPlatform, UIControlRole.TextBox, "TextBox")
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