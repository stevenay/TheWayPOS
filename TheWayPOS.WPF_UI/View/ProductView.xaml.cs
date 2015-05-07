using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using System.Text.RegularExpressions;
using TheWayPOS.WPF_UI.Common.Util;

namespace TheWayPOS.WPF_UI.View
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private void TableView_CellValueChanging(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            (sender as TableView).PostEditor();
        }

        // TextPreviewInput Handler
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        // DataObject.Pasting Handler
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            text = MyanNumToEngNumConverter.MyantoEng(text);
            Regex regex = new Regex("[^0-9.-]+"); // regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
