using System;

namespace XamlHelpmeet
{
	internal static class PkgCmdList
	{
			// CODEWINDOW_CONTEXTMENU
			public const int CreateViewModelCommandFromSelectedClassCommand     = 0x0101;
			public const int AboutCommand                                       = 0x0102;

			// XAMLEDITOR_CONTEXTMENU
			// XAMLEDITOR_CONTEXTMENU_GROUPINTO_CONTEXTMENU
			// XAMLEDITOR_CONTEXTMENU_GROUPINTO_CONTEXTMENU_BORDER_CONTEXTMENU
			public const int GroupIntoBorderNoChildRoot                         = 0x0201;
			public const int GroupIntoBorderWithGridRoot                        = 0x0202;
			public const int GroupIntoBorderWithStackPanelVerticalRoot          = 0x0203;
			public const int GroupIntoBorderWithStackPanelHorizontalRoot        = 0x0204;

			// XAMLEDITOR_CONTEXTMENU_GROUPINTO_CONTEXTMENU (continued)
			public const int GroupIntoCanvas                                    = 0x0301;
			public const int GroupIntoDockPanel                                 = 0x0302;
			public const int GroupIntoGrid                                      = 0x0303;
			public const int GroupIntoScrollViewer                              = 0x0304;
			public const int GroupIntoStackPanelVertical                        = 0x0305;
			public const int GroupIntoStackPanelHorizontal                      = 0x0306;
			public const int GroupIntoUniformGrid                               = 0x0307;
			public const int GroupIntoViewBox                                   = 0x0308;
			public const int GroupIntoWrapPanel                                 = 0x0309;
			public const int GroupIntoGroupBox                                  = 0x030a;

			// XAMLEDITOR_CONTEXTMENU (continued)
			public const int EditGridRowAndColumnsCommand                       = 0x0401;
			public const int ExtractSelectedPropertiesToStyleCommand            = 0x0402;
			public const int CreateBusinessFormCommand                          = 0x0403;
			public const int CreateFormListViewDataGridFromSelectedClassCommand = 0x0404;
			public const int FieldsListFromSelectedClassCommand                 = 0x0405;
			public const int RemoveMarginsCommand                               = 0x0406;
			public const int ChangeGridToFlowLayout                             = 0x0407;
			public const int ChainsawDesignerExtraProperties                    = 0x0408;

			// XAMLEDITOR_CONTEXTMENU_TOOLS_CONTEXTMENU
			public const int ControlDefaultsCommand                             = 0x0501;
	}
}