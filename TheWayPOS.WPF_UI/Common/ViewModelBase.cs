using System;
using System.Dynamic;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using DevExpress.Mvvm;
using TheWayPOS.WPF_UI.Interface;

namespace TheWayPOS.WPF_UI.Common
{
    public abstract class ViewModelBase : DynamicObject, INotifyPropertyChanged, INotifyDirtyData, ISupportServices
    {
        #region Constructor
        public ViewModelBase()
        {

        }
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        // This is for DynamicObject Reflection
        // But I have decided not to include Reflection Dynamic Properties
        protected object WrappedDomainEntity
        {
            get;
            set;
        }
        #endregion

        #region INotifyPropertyChanged_Events
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            VerifyPropertyName(propertyName);
            // to make sure the Handle is thread-safe
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Warns the developer if this Object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(String propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this Object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }
        #endregion

        #region INotifyDirtyData_Implementation
        // PropertyChangedEventHandler class as a convenient way to pass the property name to a subscriber.
        public event PropertyChangedEventHandler DirtyStatusChanged;

        // changes is our internal dictionary which holds the changed properties and their original values.
        protected Dictionary<String, Object> _changes = new Dictionary<String, Object>();

        /// <summary>
        /// Returns the original value of the property so it can be compared to the current
        /// value or used to restore the original value
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetChangedData(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                !_changes.ContainsKey(propertyName)) return null;
            return _changes[propertyName];
        }

        /// <summary>
        /// Clears the record of changed properties and their original values.
        /// </summary>
        /// <remarks>Call this method when the data in the model is saved.</remarks>
        public void ClearChangedData()
        {
            _changes.Clear();

            // Raise property changed on HasChangedData in case something is bound to that property
            OnPropertyChanged("HasChangedData");
        }

        /// <summary>
        /// Returns true if one or more monitored properties has changed.
        /// </summary>
        public bool HasChangedData
        {
            get
            {
                return _changes.Count > 0;
            }
        }

        // CheckDataChange should be called in property setters BEFORE the property value is set. It will
        // check to see if it already has a memory of the properties original value. If not, it will inspect
        // the property to get the original value and then save that back raising the DirtyStatusChanged event
        // in the process. If the new value is the same as the original value, the property will be removed from
        // the list of dirty properties.
        protected void CheckDataChange(Object newPropertyValue, [CallerMemberName]string propertyName = "")
        {
            // If we were passed an empty property name, eject.
            if (string.IsNullOrWhiteSpace(propertyName))
                return;

            // Check to see if the property already exists in the dictionary...
            if (_changes.ContainsKey(propertyName))
            {
                object test = _changes[propertyName];
                // Already exists in the change collection
               if (_changes[propertyName].Equals(newPropertyValue))
                {
                    // The old value and the new value match
                    _changes.Remove(propertyName);
                    RaiseDataChanged(propertyName);
                }
                else
                {
                    // New value is different than the original value...
                    // Don't do anything because we already know this value changed.
                }
            }
            else
            {
                // Key is not in the dictionary. Get the original value and save it back
                _changes.Add(propertyName, GetPropertyValue(propertyName));
                RaiseDataChanged(propertyName);
            }
        }

        // Raises the events to notify interested parties
        // that one or more monitored properties are now dirty
        private void RaiseDataChanged(string propertyName)
        {
            // Raise the DirtyStatusChanged event passing the name of the changed property
            if (DirtyStatusChanged != null)
                DirtyStatusChanged(this, new PropertyChangedEventArgs(propertyName));

            // Raise property changed on HasChangedData in case something is bound to that property
            // OnPropertyChanged("HasChangedData");
        }

        // Internal method which will get the value of the specified property
        protected object GetPropertyValue(string PropertyName)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
                return null;
            Type valueType = this.GetType();
            PropertyInfo propInfo = valueType.GetProperty(PropertyName);
            if (propInfo == null) { return null; }
            return propInfo.GetValue(this, null);
        }

        #endregion

        #region DynamicObject Reflection Methods
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.WrappedDomainEntity == null)
            {
                result = null;
                return false;
            }

            string propertyName = binder.Name;
            PropertyInfo property = this.WrappedDomainEntity.GetType().GetProperty(propertyName);

            if (property == null || property.CanRead == false)
            {
                result = null;
                return false;
            }

            result = property.GetValue(this.WrappedDomainEntity, null);
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {

            string propertyName = binder.Name;
            PropertyInfo property =
              this.WrappedDomainEntity.GetType().GetProperty(propertyName);

            if (property == null || property.CanWrite == false)
                return false;

            property.SetValue(this.WrappedDomainEntity, value, null);

            this.OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Service Container Implementation Methods

        IServiceContainer serviceContainer;
        IServiceContainer ISupportServices.ServiceContainer { get { return ServiceContainer; } }
        protected IServiceContainer ServiceContainer { get { return serviceContainer ?? (serviceContainer = CreateServiceContainer()); } }

        protected virtual IServiceContainer CreateServiceContainer()
        {
            return new ServiceContainer(this);
        }
        protected virtual T GetService<T>() where T : class
        {
            return GetService<T>(ServiceSearchMode.PreferLocal);
        }
        protected virtual T GetService<T>(string key) where T : class
        {
            return GetService<T>(key, ServiceSearchMode.PreferLocal);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual T GetService<T>(ServiceSearchMode searchMode) where T : class
        {
            return ServiceContainer.GetService<T>(searchMode);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual T GetService<T>(string key, ServiceSearchMode searchMode) where T : class
        {
            return ServiceContainer.GetService<T>(key, searchMode);
        }

        #endregion
    }
}
