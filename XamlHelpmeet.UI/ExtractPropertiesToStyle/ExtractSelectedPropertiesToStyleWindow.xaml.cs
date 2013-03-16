// file:	ExtractPropertiesToStyle\ExtractSelectedPropertiesToStyleWindow.xaml.cs
//
// summary:	Implements the extract selected properties to style window.xaml class
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Commands;
using XamlHelpmeet.Extensions;
using XamlHelpmeet.UI.Utilities;

// namespace: XamlHelpmeet.UI.ExtractPropertiesToStyle
//
// summary:	ExtractSelectedPropertiesToStyleWindow and supporting members.
namespace XamlHelpmeet.UI.ExtractPropertiesToStyle
{
	/// <summary>
	/// 	Interaction logic for ExtractSelectedPropertiesToStyleWindow.xaml.
	/// </summary>
	/// <seealso cref="T:System.Windows.Window"/>
	public partial class ExtractSelectedPropertiesToStyleWindow : Window
	{
		#region Fields

		private RelayCommand _extractCommand;
		#endregion Fields

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the ExtractSelectedPropertiesToStyleWindow
		/// 	class.
		/// </summary>
		/// <param name="Document">
		/// 	The document.
		/// </param>
		/// <param name="IsSilverlight">
		/// 	true if this ExtractSelectedPropertiesToStyleWindow is silverlight.
		/// </param>
		/// <param name="SilverlightVersion">
		/// 	The silverlight version.
		/// </param>
		public ExtractSelectedPropertiesToStyleWindow(System.Xml.XmlDocument Document, bool IsSilverlight, string SilverlightVersion)
		{
			HasStyleSet = false;
			this.IsSilverlight = IsSilverlight;
			this.SilverlightVersion = SilverlightVersion;
			this.Document = Document;
			ExtractedProperties = new List<ExtractProperty>();

			GetProperties();
			DataContext = this;

			InitializeComponent();

			if (!(IsSilverlight && SilverlightVersion.StartsWith("3")))
				return;

			tbIsSilverlight3RequiredAstrick.Visibility = System.Windows.Visibility.Visible;
			tbIsSilverlight3RequiredMessage.Visibility = System.Windows.Visibility.Visible;
		}

		/// <summary>
		/// 	Initializes a new instance of the ExtractSelectedPropertiesToStyleWindow
		/// 	class.
		/// </summary>
		public ExtractSelectedPropertiesToStyleWindow()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// 	Gets the document.
		/// </summary>
		/// <value>
		/// 	The document.
		/// </value>
		public XmlDocument Document
		{
			get;
			private set;
		}

		/// <summary>
		/// 	Gets the extract command.
		/// </summary>
		/// <value>
		/// 	The extract command.
		/// </value>
		public ICommand ExtractCommand
		{
			get
			{
				if (_extractCommand == null)
					_extractCommand = new RelayCommand(ExtractExecute, CanExtractExecute);

				return _extractCommand;
			}
		}

		/// <summary>
		/// 	Gets the extracted properties.
		/// </summary>
		/// <value>
		/// 	The extracted properties.
		/// </value>
		public List<ExtractProperty> ExtractedProperties
		{
			get;
			private set;
		}

		private bool HasStyleSet
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets a value indicating whether this ExtractSelectedPropertiesToStyleWindow is
		/// 	silverlight.
		/// </summary>
		/// <value>
		/// 	true if this ExtractSelectedPropertiesToStyleWindow is silverlight, otherwise
		/// 	false.
		/// </value>
		public bool IsSilverlight
		{
			get;
			private set;
		}

		/// <summary>
		/// 	Gets the silverlight version.
		/// </summary>
		/// <value>
		/// 	The silverlight version.
		/// </value>
		public string SilverlightVersion
		{
			get;
			private set;
		}

		/// <summary>
		/// 	Gets or sets the name of the style.
		/// </summary>
		/// <value>
		/// 	The name of the style.
		/// </value>
		public string StyleName
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets the name of the type.
		/// </summary>
		/// <value>
		/// 	The name of the type.
		/// </value>
		public string TypeName
		{
			get;
			private set;
		}

		#endregion Properties

		#region Methods

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void GetProperties()
		{
			TypeName = Document.ChildNodes[0].Name;

			foreach (XmlAttribute atr in Document.ChildNodes[0].Attributes)
			{
				// First IF lists attributes or attribute characteristics that
				// exist in properties that should not be extracted and placed
				// in a style.
				if (atr.Name.Contains(".") ||
					atr.Name == "Name" ||
					atr.Name == "x:Name" ||
					atr.Name.StartsWith("xmlns") ||
					atr.Name.StartsWith("Command") ||
					atr.Name.StartsWith("Click"))
				{
					// Developers may add more checks here as desired, i.e.,
					// to prevent an extraction.
				}
				else if (atr.Name != "Style")
				{
					ExtractedProperties.Add(new ExtractProperty(atr));
				}
				else
				{
					HasStyleSet = true;
					UIUtilities.ShowInformationMessage("Root Element Has Style", "The selected root UI Element already has a style property. The style will not be set when Extract is executed.");
				}
			}
		}

		private void InsertStyleProperty()
		{
			if (!StyleName.IsNotNullOrEmpty() || HasStyleSet != false)
				return;

			var newAttribute = Document.CreateAttribute("Style");
			newAttribute.Value = string.Format("{{StaticResource {0}}}", StyleName);
			Document.ChildNodes[0].Attributes.Append(newAttribute);
		}

		private void RemoveSelectedProperties()
		{
			foreach (var obj in ExtractedProperties)
			{
				if (obj.IsSelected == false)
					continue;

				Document.ChildNodes[0].Attributes.Remove(obj.XmlAttribute);
			}
		}

		#endregion Methods

		#region Command Methods

		private bool CanExtractExecute(object obj)
		{
			if (IsSilverlight && SilverlightVersion == "3.0" && StyleName.IsNullOrEmpty())
				return false;

			return (from x in ExtractedProperties
					where x.IsSelected
					select x).Count() > 0;
		}

		private void ExtractExecute(object obj)
		{
			if (!CanExtractExecute(obj))
				return;

			RemoveSelectedProperties();
			InsertStyleProperty();
			DialogResult = true;
		}

		#endregion Command Methods
	}
}