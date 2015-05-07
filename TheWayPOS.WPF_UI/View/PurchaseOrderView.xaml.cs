using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;
using System.Text.RegularExpressions;
using TheWayPOS.WPF_UI.Common.Util;
using TheWayPOS.WPF_UI.ViewModel;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.Xpf.Data;
using DevExpress.Data;

namespace TheWayPOS.WPF_UI.View
{
    /// <summary>
    /// Interaction logic for PurchaseInvoiceView.xaml
    /// </summary>
    public partial class PurchaseOrderView : UserControl
    {
        public PurchaseOrderView()
        {
            InitializeComponent();
        }

        private void TableView_CellValueChanging(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            (sender as TableView).PostEditor();
        }

        void PopupTestEdit_PopupOpened(object sender, RoutedEventArgs e)
        {
            // string str = "";
        }

        private void toggleButtonInverseVideo_Checked(object sender, EventArgs e)
        {
            // popupTestEdit.Text = "I have been clicked";
        }

        #region EventRelatedMethods

        private void gridControlPod_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            // e.IsValid = ((PurchaseOrderDetailViewModel)e.Row).ReMatchProductConflict;
        }
        private void cboSupplier_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // cboSupplier.Text will return the value before making the Selection
            // New Row will only be added if there is no Supplier selected already
            if (cboSupplier.Text == string.Empty)
            {
                this.AddNewRow();
            }
            else
            {
                // If Supplier is already selected, the system will automatically Rematch the Products
                this.ReMatchLineItems();
            }
        }

        private void PurchaseOrderDetailsView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            gridControlPod.SetCellValue(e.RowHandle, "UnitPrice", 10);
            gridControlPod.SetCellValue(e.RowHandle, "Discontinued", false);
            gridControlPod.SetCellValue(e.RowHandle, "UnitsOnOrder", 1);
        }
        private void gridControlPurchaseOrderDetails_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int visibleIndex = gridControlPod.GetRowVisibleIndexByHandle(tableViewPod.FocusedRowHandle);
            if (e.Key == Key.Enter && visibleIndex == (gridControlPod.VisibleRowCount - 1))
            {
                this.AddNewRow();
            }
            if (e.Key == Key.Tab && visibleIndex == (gridControlPod.VisibleRowCount - 1) && gridControlPod.CurrentColumn.VisibleIndex == (tableViewPod.VisibleColumns.Count - 1))
            {
                this.AddNewRow();
            }
        }

        #endregion
        
        #region HelperMethods

        internal void AddNewRow()
        {
            var vm = (PurchaseOrderHeaderViewModel) this.DataContext;
            var addedOrderDetail = vm.PurchaseOrderDetailCollection.AddNewOrderDetail();

            // That is the correct way to check object is null
            if (addedOrderDetail != null)
            {
                gridControlPod.CurrentItem = addedOrderDetail;
                gridControlPod.CurrentColumn = gridControlPod.Columns[0];
            }
        }
        internal void ValidateAllRows()
        {
            try
            {
                for (int i = 0; i < gridControlPod.VisibleRowCount - 1; i++)
                {
                    ((IDataProviderOwner)gridControlPod).RaiseValidatingCurrentRow(
                        new ValidateControllerRowEventArgs(gridControlPod.GetRowHandleByListIndex(i), gridControlPod.GetRow(i))
                    );
                }
            }
            catch
            {
            }
        }

        internal void ReMatchLineItems()
        {
            var vm = (PurchaseOrderHeaderViewModel)this.DataContext;
            vm.PurchaseOrderDetailCollection.ReMatchProductsbySupplier();

            ValidateAllRows();
        }

        #endregion
    }   
}
