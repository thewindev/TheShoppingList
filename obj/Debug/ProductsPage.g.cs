﻿

#pragma checksum "C:\Users\Bogdan\Documents\Visual Studio 2012\Projects\TheShoppingList\TheShoppingList\ProductsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "12A79E1650B602147B6403316A45B4A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheShoppingList
{
    partial class ProductsPage : global::TheShoppingList.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 22 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.EditProduct;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RemoveProduct;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 24 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddProduct;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 25 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnAddToCart_Click_1;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 50 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Popup)(target)).Closed += this.PopupClosed;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 104 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.productsList_SelectionChanged_1;
                 #line default
                 #line hidden
                #line 104 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).RightTapped += this.productsList_RightTapped_1;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 107 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).RightTapped += this.Grid_RightTapped_1;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 65 "..\..\ProductsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


