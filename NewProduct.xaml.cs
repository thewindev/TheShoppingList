﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TheShoppingList.Classes;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TheShoppingList
{
    public sealed partial class NewProduct
    {
        public enum InputMode
        {
            Add,
            Edit
        }

        private Product _product;

        public bool ProductAdded { get; set; }

        public InputMode Mode { get; set; }

        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        
        public NewProduct()
        {
            InitializeComponent();
            Loaded += PopupLoaded;
            ProductAdded = false;
            var helper = new InputPaneHelper();
            helper.SubscribeToKeyboard(true);
            helper.AddShowingHandler(txtProductName, CustomKeyboardHandler);
            helper.AddShowingHandler(txtPrice, CustomKeyboardHandler);
            helper.AddShowingHandler(txtQuantity, CustomKeyboardHandler);
            helper.AddShowingHandler(txtShopName, CustomKeyboardHandler);
            helper.SetHidingHandler(InputPanelHiding);
            quantityType.SelectedIndex = 0;
            txtPriceLabel.Text = "Price(" + Utils.GetCountryInfo().CurrencySymbol + "):";
        }

        private void InputPanelHiding(InputPane input, InputPaneVisibilityEventArgs e)
        {
            this.Margin = new Thickness(0);
        }

        private void CustomKeyboardHandler(object sender, InputPaneVisibilityEventArgs e)
        {
            this.Margin = new Thickness(0, -100, 0, 0);
        }



        private void PopupLoaded(object sender, RoutedEventArgs e)
        {
            if (Mode == InputMode.Add)
            {
                btnIsBought.IsOn = false;
                _product = new Product();
                btnSave.SetValue(AutomationProperties.NameProperty, "Add");
            }
            else
            {
                FillProperties();
                btnSave.SetValue(AutomationProperties.NameProperty, "Save");
            }
            SetWindowSize();
        }

        private void SetWindowSize()
        {
            var size = Application.Current.Resources["newListSize"] as Point;
            if (size != null)
            {
                transparentBorder.Width = size.Width;
                transparentBorder.Height = size.Height;
                newProductBorder.Width = transparentBorder.Width;
                transparentBorder.Visibility = Visibility.Visible;
            }
        }

        private void FillProperties()
        {
            if (Product == null)
            {
                CloseControl();
            }
            if (Product != null)
            {
                btnIsBought.IsOn = Product.IsBought;
                txtProductName.Text = Product.Title;
                txtPrice.Text = Product.Price.ToString();
                txtQuantity.Text = Product.Quantity.ToString();
                if (Product.ShopName != null)
                    txtShopName.Text = Product.ShopName;
                quantityType.SelectedIndex = Utils.IndexFromQuantityType(Product.QuantityType);
            }
        }

        

        private void OnSaveProductDetails(object sender, RoutedEventArgs e)
        {
           
            Product = new Product();
            if (txtProductName.Text == string.Empty)
            {
                new MessageDialog("You must type the product name").ShowAsync();
                Product = null;
                return;
            }
            int index = quantityType.SelectedIndex;
            var type = QuantityType.Default;
            switch (index)
            {
                case 0:
                    type = QuantityType.pcs;
                    break;
                case 1:
                    type = QuantityType.kg;
                    break;
                case 2:
                    type = QuantityType.l;
                    break;
                case 3:
                    type = QuantityType.m;
                    break;
                case 4:
                    type = QuantityType.ft;
                    break;
                case 5:
                    type = QuantityType.lb;
                    break;
            }
            
            if (txtProductName.Text != string.Empty)
                Product.Title = txtProductName.Text;
            if (txtPrice.Text != string.Empty)
                if (Utils.IsNumber(txtPrice.Text))
                    Product.Price = Convert.ToDouble(txtPrice.Text);
                else
                {
                    new MessageDialog("The price must have only digits!").ShowAsync();
                    return;
                }
            if (txtQuantity.Text != string.Empty)
                if (Utils.IsNumber(txtQuantity.Text))
                    Product.Quantity = Convert.ToDouble(txtQuantity.Text);
                else
                {
                    new MessageDialog("The quantity must have only digits!").ShowAsync();
                    return;
                }

            Product.QuantityType = type;
            if (txtShopName.Text != string.Empty)
                Product.ShopName = txtShopName.Text;
            txtProductName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtShopName.Text = string.Empty;
            quantityType.SelectedIndex = -1;
            Product.IsBought = btnIsBought.IsOn;
            btnCancelOrClose.SetValue(AutomationProperties.NameProperty, "Close");

            ProductAdded = true;
            
            
            OnNewProductAdded(new ProductAddedArgs { Product = Product });
            txtProductName.Focus(FocusState.Programmatic);
        }

        private void ProductNameChanged(object sender, TextChangedEventArgs e)
        {

            if (txtProductName.Text.Length > 30)
            {
                txtPrice.BorderBrush = new SolidColorBrush(Colors.Red);
                var color = new Color();
                //string colorcode = "#FFD87474";
                //int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
                Color clr = Color.FromArgb(0xFF, 0xD8, 0x74, 0x74);
                txtPrice.Background = new SolidColorBrush(clr);
                btnSave.IsEnabled = false;
            }
            else
            {
                txtPrice.BorderBrush = new SolidColorBrush(Colors.Black);
                txtPrice.Background = new SolidColorBrush(Colors.White);
                if (btnSave.IsEnabled == false)
                    btnSave.IsEnabled = true;
            }


        }


        private void DigitTextChanged(TextBox textBox, TextBlock warningText)
        {
            if (Utils.IsNumber(textBox.Text) == false)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                textBox.Background = new SolidColorBrush(Colors.IndianRed);
                warningText.Visibility = Visibility.Visible;
            }
            else
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Transparent);
                textBox.Background = new SolidColorBrush(Colors.White);
                warningText.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Product = null;
            CloseControl();
        }

        public event ProductAdded NewProductAdded;

        public void OnNewProductAdded(ProductAddedArgs args)
        {
            ProductAdded handler = NewProductAdded;
            if (Mode == InputMode.Edit)
                CloseControl();
            if (handler != null) handler(this, args);
        }

        private void CloseControl()
        {
            var p = Parent as Popup;
            if (p != null) p.IsOpen = false;
        }

        private void newListBorder_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (sender.Equals(txtPrice))
            {
                txtPriceWarning.Visibility = Visibility.Visible;
                DigitTextChanged(txtPrice, txtPriceWarning);
            }
            else if (sender.Equals(txtQuantity))
            {
                txtQuantityWarning.Visibility = Visibility.Visible;
                DigitTextChanged(txtQuantity, txtQuantityWarning);
            }
            else if (sender.Equals(txtProductName))
                ProductNameChanged(null, null);
        }

        private void QuantityTypeChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            foreach (var textbox in FindVisualChildren<TextBox>(containerGrid))
            {
                textbox.Text = string.Empty;
            }
        }


    }

    public delegate void ProductAdded(object sender, ProductAddedArgs args);

    public class ProductAddedArgs
    {
        public Product Product { get; set; }
    }
}