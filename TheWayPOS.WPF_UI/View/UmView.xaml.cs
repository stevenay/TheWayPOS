using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using TheWayPOS.WPF_UI.Common.Util;
using DevControls = DevExpress.Xpf.Editors;

namespace TheWayPOS.WPF_UI.View
{
    /// <summary>
    /// Interaction logic for UmView.xaml
    /// </summary>
    public partial class UmView : UserControl
    {
        public UmView()
        {
            InitializeComponent();
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

        private void chkDisposable_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if ((bool)chkDisposable.IsChecked)
            {
                this.cboDisposedUm.IsReadOnly = false;
                this.txtDisposedUmQuantity.IsReadOnly = false;
            }
            else
            {
                this.cboDisposedUm.IsReadOnly = true;
                this.txtDisposedUmQuantity.IsReadOnly = true;
            }
        }

        private void txtDisposedUmQuantity_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textEdit = (DevControls.TextEdit)sender;
            textEdit.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
        }
    }
}
