using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TheWayPOS.WPF_UI.ViewModel
{
    public class TileViewModel
    {
        public string Caption { get; set; }
        public ObservableCollection<TileViewModel> Children { get; set; }
        public ICommand NavigateCommand { get; set; }
        public bool IsHasChildren { get { return Children.Count != 0; } }
        public String GlyphUri { get; set; }
        public String Group { get; set; }
        public int ItemWidth { get; set; }
        /// <summary>
        /// Initializes a new instance of the TileObject class.
        /// </summary>
        public TileViewModel()
        {
            Caption = String.Empty;
            Children = new ObservableCollection<TileViewModel>();
            NavigateCommand = null;
            GlyphUri = String.Empty;
            Group = String.Empty;
            ItemWidth = 0;
        }
    }
}
