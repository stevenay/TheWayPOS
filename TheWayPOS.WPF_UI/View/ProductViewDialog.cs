using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWayPOS.WPF_UI.Interface;

namespace TheWayPOS.WPF_UI.View
{
    public class ProductViewDialog : IModelDialog
    {
       // private ProductView view;

        #region Implement_IModelDialog
        public void BindViewModel<TViewModel>(TViewModel viewModel)
        {
            //GetDialog().DataContext = viewModel;
        }

        public void ShowDialog()
        {
           // GetDialog().ShowDialog();
        }

        public void Close()
        {
           // GetDialog().Close();
        }
        #endregion

        //private ProductView GetDialog()
        //{
        //    if (view == null)
        //    {
        //        // create the view if the view does not exist
        //        view = new ProductView();
        //        // view.Closed += new EventHandler(view_Closed);
        //    }

        //    return view;
        //}

        //void view_Closed(object sender, EventArgs e)
        //{
        //    view = null;
        //}
    }
}
