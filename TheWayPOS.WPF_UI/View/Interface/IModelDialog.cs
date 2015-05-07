using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWayPOS.WPF_UI.Interface
{
    public interface IModelDialog
    {
        void BindViewModel<TViewModel>(TViewModel viewModel); // bind to ViewModel

        void ShowDialog(); // show the model window

        void Close(); // close the dialog
    }
}
