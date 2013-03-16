using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.Enums;
using XamlHelpmeet.Extensions;
using Mono;
using XamlHelpmeet.UI.DynamicForm;

namespace XamlHelpmeet.UI.Utilities
{
	public class UIHelpers
	{
		public static bool IsMicrosoftAssembly(string Name)
		{
			// Original moved in an extension method and the original
			// method is wrapping that for use where the extension
			// method cannot be used.
			return Name.IsMicrosoftAssembly();
		}

		public static DynamicFormEditor DynamicFormEditorFactory(PropertyInformation pi)
		{
			var listBoxContent = new DynamicFormListBoxContent();
			listBoxContent.AssociatedLabel = ParsePropertyNameForLabel(pi.Name);
			listBoxContent.CanWrite = pi.CanWrite;

			if (pi.CanWrite)
			{
				listBoxContent.BindingMode = BindingMode.TwoWay;
			}
			else
			{
				listBoxContent.BindingMode = BindingMode.OneWay;
			}

			listBoxContent.BindingPath = pi.Name;

			if (pi.TypeName.IndexOf("Boolean") == -1)
			{
				if (pi.CanWrite)
				{
					listBoxContent.ControlType = DynamicFormControlType.TextBox;
				}
				else
				{
					listBoxContent.ControlType = DynamicFormControlType.TextBlock;
				}
			}
			else
			{
				listBoxContent.ControlType = DynamicFormControlType.CheckBox;
			}

			listBoxContent.DataType = pi.TypeName;
			listBoxContent.TypeNamespace = pi.TypeNamespace;

			if (listBoxContent.DataType.Contains("Int32"))
			{
				listBoxContent.DataType = "Integer";
			}
			else if (listBoxContent.DataType.Contains("Int16"))
			{
				listBoxContent.DataType = "Short";
			}
			else if (listBoxContent.DataType.Contains("Int64"))
			{
				listBoxContent.DataType = "Long";
			}

			if (pi.TypeName.Contains("Decimal"))
			{
				listBoxContent.StringFormat = "{0:c}";
			}
			else if (pi.TypeName.Contains("Date"))
			{
				listBoxContent.StringFormat = "{0:d}";
			}
			else
			{
				listBoxContent.StringFormat = string.Empty;
			}
			return new DynamicFormEditor()
			{
				DataContext = listBoxContent
			};
		}

		public static Window FindAnscestorWindow(DependencyObject DependencyObject)
		{
			while (DependencyObject != null)
			{
				DependencyObject = VisualTreeHelper.GetParent(DependencyObject);
				if (DependencyObject is Window)
				{
					break;
				}
			}
			return DependencyObject as Window;
		}

		public static ListCollectionView GetSampleFormats()
		{
			var obj = new List<SampleFormat>();
			obj.Add(new SampleFormat("Date",
			                         "12/25/1965",
			                         "{0:d}"));
			obj.Add(new SampleFormat("Date",
			                         "Saturday, December 25, 1965",
			                         "{0:D}"));
			obj.Add(new SampleFormat("Date",
			                         "Saturday, December 25, 1965 7:25 AM",
			                         "{0:f}"));
			obj.Add(new SampleFormat("Date",
			                         "Saturday, December 25, 1965 7:25:42 AM",
			                         "{0:F}"));
			obj.Add(new SampleFormat("Date",
			                         "12/25/1965 7:25 AM",
			                         "{0:g}"));
			obj.Add(new SampleFormat("Date",
			                         "12/25/1965 7:25:42 AM",
			                         "{0:G}"));
			obj.Add(new SampleFormat("Date",
			                         "December 25",
			                         "{0:M}"));
			obj.Add(new SampleFormat("Date",
			                         "Sat, 25 Dec 1965 7:25:42 GMT",
			                         "{0:R}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "$75,234.89",
			                         "{0:c}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "75234.89",
			                         "{0:F}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "75234",
			                         "{0:F0}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "75234.8933",
			                         "{0:F4}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "75,234.89",
			                         "{0:N}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "75,234.8933",
			                         "{0:N4}"));
			obj.Add(new SampleFormat("Double, Decimal",
			                         "7,523,489.33 %",
			                         "{0:P}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "42",
			                         "{0:D}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "00042",
			                         "{0:D5}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "42",
			                         "{0:F0}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "2a",
			                         "{0:x}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "2A",
			                         "{0:X}"));
			obj.Add(new SampleFormat("Integer, Short",
			                         "00002A",
			                         "{0:X6}"));

			var sampleFormatList = new ListCollectionView(obj);
			sampleFormatList.GroupDescriptions.Add(new PropertyGroupDescription("DataType"));
			sampleFormatList.SortDescriptions.Add(new SortDescription("DataType", ListSortDirection.Ascending));
			sampleFormatList.SortDescriptions.Add(new SortDescription("Example", ListSortDirection.Descending));

			return sampleFormatList;
		}

		public static string[] GetSortedEnumNames(Type T)
		{
			if (T.IsEnum)
			{
				var strout = Enum.GetNames(T);
				Array.Sort(strout);
				return strout;
			}
			throw new ArgumentException("Must be an enum.", "T");
		}

		public static string ParsePropertyNameForLabel(string ToParse)
		{
			var sb = new StringBuilder(256);
			var foundUpperCase = false;
			var onlyUpperCase = true;

			for (int i = 0; i < ToParse.Length; i++)
			{
				if (!ToParse.IsNotUpper(i))
					continue;
				onlyUpperCase = false;
				break;
			}

			if (onlyUpperCase)
			{
				return ToParse;
			}

			for (int i = 0; i < ToParse.Length; i++)
			{
				if (!foundUpperCase && ToParse.IsUpper(i))
				{
					foundUpperCase = true;
					if (i == 0)
					{
						sb.Append(ToParse[i]);
					}
					else
					{
						sb.Append(" ");
						sb.Append(ToParse[i]);
					}
					continue;
				}
				if (!foundUpperCase)
				{
					continue;
				}
				if (ToParse.IsUpper(i))
				{
					sb.Append(" ");
					sb.Append(ToParse[i]);
				}
				else if (ToParse.IsLetterOrDigit(i))
				{
					sb.Append(ToParse[i]);
				}
			}

			return sb.ToString();
		}
	}
}
