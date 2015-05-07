using System;
using System.ComponentModel;

namespace TheWayPOS.WPF_UI.Interface
{
    public interface INotifyDirtyData
    {
        event PropertyChangedEventHandler DirtyStatusChanged;

        Object GetChangedData(string propertyName);
        
        void ClearChangedData();
        
        bool HasChangedData { get; }
    }
}
