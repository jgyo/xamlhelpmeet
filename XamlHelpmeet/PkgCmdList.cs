using System;

namespace XamlHelpmeet
{
    internal static class PkgCmdList
    {
        // GROUP SYMBOLS
        public const int CodeWindowCtxMenuGroup                             = 0x0100;
        public const int XamlHelpmeetCodeWindowMenuGroup                    = 0x0200;
        public const int XamlWindowCtxMenuGroup                             = 0x0300;
        public const int XamlHelpmeetXamlWindowMenuGroup                    = 0x0400;
        public const int XamlHelpmeetXamlWindowSelectGroup                  = 0x0450;
        public const int XamlHelpmeetXamlWindowSubmenusGroup                = 0x0900;
        public const int ToolsMenuGroup                                     = 0x0500;
        public const int GroupIntoMenuGroup                                 = 0x0600;
        public const int GroupIntoBorderMenuGroup                           = 0x0700;

        // MENU SYMBOLS
        public const int XamlHelpmeetCodeWindowMenu                         = 0x01A0;
        public const int XamlHelpmeetXamlWindowMenu                         = 0x03A0;
        public const int ToolsMenu                                          = 0x04A0;
        public const int GroupIntoMenu                                      = 0x05A0;
        public const int GroupIntoBorderMenu                                = 0x06A0;

        // BUTTON SYMBOLS
        // XamlHelpmeetCodeWindowMenu in XamlHelpmeetCodeWindowMenuGroup
        public const int CreateViewModelFromSelectedClassCommand            = 0x02B1;
        public const int AboutCommand                                       = 0x02B2;

        // XamlHelpmeetXamlWindowMenu in XamlHelpmeetXamlWindowMenuGroup
        public const int EditGridRowAndColumnsCommand                       = 0x04B1;
        public const int ExtractSelectedPropertiesToStyleCommand            = 0x04B2;
        public const int CreateBusinessFormCommand                          = 0x04B3;
        public const int CreateFormListViewDataGridFromSelectedClassCommand = 0x04B4;
        public const int FieldsListFromSelectedClassCommand                 = 0x04B5;
        public const int RemoveMarginsCommand                               = 0x04B6;
        public const int ChangeGridToFlowLayoutCommand                      = 0x04B7;
        public const int ChainsawDesignerExtraPropertiesCommand             = 0x04B8;

        // New select commands in XamlHelpmeetXamlWindowSelectGroup
        public const int SelectContainingControlCommand                     = 0x04D1;
        public const int WidenSelectionCommand                              = 0x04D2;
        public const int NarrowSelectionCommand                             = 0x04D3;

        // ToolsMenu in ToolsMenuGroup
        public const int ControlDefaultsCommand                             = 0x05B1;
        //             AboutCommand (duplicate in this menu)                       

        // GroupIntoMenu in GroupIntoMenuGroup
        public const int GroupIntoCanvasCommand                             = 0x06B1;
        public const int GroupIntoDockPanelCommand                          = 0x06B2;
        public const int GroupIntoGridCommand                               = 0x06B3;
        public const int GroupIntoScrollViewerCommand                       = 0x06B4;
        public const int GroupIntoStackPanelVerticalCommand                 = 0x06B5;
        public const int GroupIntoStackPanelHorizontalCommand               = 0x06B6;
        public const int GroupIntoUniformGridCommand                        = 0x06B7;
        public const int GroupIntoViewBoxCommand                            = 0x06B8;
        public const int GroupIntoWrapPanelCommand                          = 0x06B9;
        public const int GroupIntoGroupBoxCommand                           = 0x06BA;


        // GroupIntoBorderMenu in GroupIntoBorderMenuGroup
        public const int GroupIntoBorderNoChildRootCommand                  = 0x07B1;
        public const int GroupIntoBorderWithGridRootCommand                 = 0x07B2;
        public const int GroupIntoBorderWithStackPanelVerticalRootCommand   = 0x07B3;
        public const int GroupIntoBorderWithStackPanelHorizontalRootCommand = 0x07B4;
    }
}