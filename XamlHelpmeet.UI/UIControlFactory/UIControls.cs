// file:	UIControlFactory\UIControls.cs
//
// summary:	Implements the controls class
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.UIControlFactory
{
	/// <summary>
	/// 	Controls.
	/// </summary>
	/// <seealso cref="T:System.Collections.ObjectModel.ObservableCollection{XamlHelpmeet.UI.UIControlFactory.UIControl}"/>
	[Serializable]
	public class UIControls : ObservableCollection<UIControl>
	{
		#region Properties

		/// <summary>
		/// 	Gets or sets a value indicating whether the automatic append execute.
		/// </summary>
		/// <value>
		/// 	true if automatic append execute, otherwise false.
		/// </value>
		public bool AutoAppendExecute
		{
			get;
			set;
		}

		#endregion Properties

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the UIControls class.
		/// </summary>
		/// <param name="list">
		/// 	The list.
		/// </param>
		public UIControls(List<UIControl> list)
			: base(list)
		{
			AutoAppendExecute = true;
		}

		/// <summary>
		/// 	Initializes a new instance of the UIControls class.
		/// </summary>
		/// <param name="collection">
		/// 	The collection.
		/// </param>
		public UIControls(IEnumerable<UIControl> collection)
			: base(collection)
		{
			AutoAppendExecute = true;
		}

		/// <summary>
		/// 	Initializes a new instance of the UIControls class.
		/// </summary>
		public UIControls()
		{
			AutoAppendExecute = true;
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// 	Gets user interface control.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// 	Thrown when one or more arguments are outside the required range.
		/// </exception>
		/// <param name="ControlType">
		/// 	Type of the control.
		/// </param>
		/// <param name="uiPlatform">
		/// 	The platform.
		/// </param>
		/// <returns>
		/// 	The user interface control.
		/// </returns>
		public UIControl GetUIControl(ControlType ControlType, UIPlatform uiPlatform)
		{
			UIControlRole uiControlRole;

			switch (ControlType)
			{
				case ControlType.CheckBox:
					uiControlRole = UIControlRole.CheckBox;
					break;

				case ControlType.ComboBox:
					uiControlRole = UIControlRole.ComboBox;
					break;

				case ControlType.Image:
					uiControlRole = UIControlRole.Image;
					break;

				case ControlType.Label:
					uiControlRole = UIControlRole.Label;
					break;

				case ControlType.TextBlock:
					uiControlRole = UIControlRole.TextBlock;
					break;

				case ControlType.TextBox:
					uiControlRole = UIControlRole.TextBox;
					break;

				case ControlType.DatePicker:
					uiControlRole = UIControlRole.DatePicker;
					break;

				default:
					throw new ArgumentOutOfRangeException("ControlType");
			}

			return GetUIControl(uiControlRole, uiPlatform);
		}

		/// <summary>
		/// 	Gets user interface control.
		/// </summary>
		/// <param name="ControlRole">
		/// 	The control role.
		/// </param>
		/// <param name="uiPlatform">
		/// 	The platform.
		/// </param>
		/// <returns>
		/// 	The user interface control.
		/// </returns>
		public UIControl GetUIControl(UIControlRole ControlRole, UIPlatform uiPlatform)
		{
			foreach (var ctrl in this)
			{
				if (ctrl.ControlRole == ControlRole && ctrl.Platform == uiPlatform)
				{
					return ctrl;
				}
			}
			return null;
		}

		/// <summary>
		/// 	Gets user interface controls for platform.
		/// </summary>
		/// <param name="Platform">
		/// 	The platform.
		/// </param>
		/// <returns>
		/// 	The user interface controls for platform.
		/// </returns>
		public List<UIControl> GetUIControlsForPlatform(UIPlatform Platform)
		{
			return (from d in this
					where d.Platform == Platform
					orderby d.ControlRole.ToString()
					select d).ToList();
		}

		#endregion Methods
	}
}