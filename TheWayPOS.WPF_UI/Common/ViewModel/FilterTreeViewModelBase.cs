using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DevExpress.Data.Filtering;
using System.Windows.Media;

namespace TheWayPOS.WPF_UI.Common.ViewModel
{
    public class FilterItemBase
    {
        Func<ImageSource> getIcon;
        public virtual string Name { get; set; }
        public virtual CriteriaOperator FilterCriteria { get; set; }
        public ImageSource Icon { get { return getIcon(); } }
        public FilterItemBase SetIcon(string uri)
        {
            uri = string.Format("pack://application:,,,/DevExpress.DevAV{0}.Model;component/{1}", AssemblyInfo.VSuffix, uri);
            getIcon = () =>
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Absolute));
            };
            return this;
        }
    }

    public interface IFilterTreeModelPageSpecificSettings
    {
        string CustomFiltersSetting { get; set; }
        string GroupFiltersSetting { get; set; }
        IEnumerable<FilterItemBase> CreateInitialCustomFilters(FilterTreeViewModelBase creator);
        IEnumerable<FilterItemBase> CreateStaticFilters(FilterTreeViewModelBase creator);
    }

    public abstract class FilterTreeViewModelBase
    {
        public class SerializableFilterItem
        {
            public string Name { get; set; }
            public string FilterCriteria { get; set; }
        }
        static FilterTreeViewModelBase()
        {
            // var enums = typeof(EmployeeStatus).Assembly.GetTypes().Where(t => t.IsEnum);
            // foreach (Type e in enums)
            //    EnumProcessingHelper.RegisterEnum(e);
        }

        IFilterTreeModelPageSpecificSettings settings;
        public FilterTreeViewModelBase(IFilterTreeModelPageSpecificSettings settings)
        {
            this.settings = settings;
        }
        public void Init()
        {
            StaticFilters = new ObservableCollection<FilterItemBase>(settings.CreateStaticFilters(this));
            CustomFilters = new ObservableCollection<FilterItemBase>();
            if (!LoadFromSettings(CustomFilters, settings.CustomFiltersSetting))
            {
                CustomFilters = new ObservableCollection<FilterItemBase>(settings.CreateInitialCustomFilters(this));
            }
            Groups = new ObservableCollection<FilterItemBase>();
            if (!LoadFromSettings(Groups, settings.GroupFiltersSetting))
            {

            }
            SelectedItem = StaticFilters.FirstOrDefault();
        }
        public const string StaticFiltersName = "Favorites";
        public const string CustomFiltersName = "Custom Filters";
        public const string GroupFiltersName = "Groups";

        public virtual ObservableCollection<FilterItemBase> StaticFilters { get; set; }
        public virtual ObservableCollection<FilterItemBase> CustomFilters { get; set; }
        public virtual ObservableCollection<FilterItemBase> Groups { get; set; }
        public virtual FilterItemBase SelectedItem { get; set; }

        public virtual void AddNewGroupFilter(FilterItemBase filterItem)
        {
            Groups.Add(filterItem);
            SaveGroupFilters();
        }
        public virtual void AddNewCustomFilter(FilterItemBase filterItem)
        {
            var existing = CustomFilters.FirstOrDefault(fi => fi.Name == filterItem.Name);
            if (existing != null)
            {
                CustomFilters.Remove(existing);
            }
            CustomFilters.Add(filterItem);
            SaveCustomFilters();
        }

        public virtual void DeleteCustomFilter(FilterItemBase filterItem)
        {
            CustomFilters.Remove(filterItem);
            SaveCustomFilters();
        }
        public virtual void DeleteGroupFilter(FilterItemBase filterItem)
        {
            Groups.Remove(filterItem);
            SaveGroupFilters();
        }

        public virtual void ModifyCustomFilter(FilterItemBase filterItem)
        {
            SaveCustomFilters();
        }
        public virtual void ModifyGroupFilter(FilterItemBase filterItem)
        {
            SaveGroupFilters();
        }

        public virtual void DuplicateFilter(FilterItemBase filterItem)
        {
            var newItem = CreateFilterItem("Copy of " + filterItem.Name, filterItem.FilterCriteria);
            CustomFilters.Add(newItem);
            SaveCustomFilters();
        }
        public virtual void ResetCustomFilters()
        {
            if (CustomFilters.Contains(SelectedItem))
                SelectedItem = null;
            CustomFilters = new ObservableCollection<FilterItemBase>(settings.CreateInitialCustomFilters(this));
            settings.CustomFiltersSetting = string.Empty;
            Properties.Settings.Default.Save();
        }

        public abstract FilterItemBase CreateFilterItem(string name, CriteriaOperator filterCriteria);
        public abstract FilterItemBase CreateStaticFilterItem(string name, CriteriaOperator filterCriteria);

        void SaveCustomFilters()
        {
            settings.CustomFiltersSetting = SaveToSettings(CustomFilters);
            Properties.Settings.Default.Save();
        }
        void SaveGroupFilters()
        {
            settings.GroupFiltersSetting = SaveToSettings(Groups);
            Properties.Settings.Default.Save();
        }
        string SaveToSettings(ObservableCollection<FilterItemBase> filters)
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableFilterItem>));
                serializer.Serialize(writer,
                    filters
                    .Select(fi => new SerializableFilterItem { Name = fi.Name, FilterCriteria = CriteriaOperator.ToString(fi.FilterCriteria) })
                    .ToList());
                return sb.ToString();
            }
        }
        bool LoadFromSettings(ObservableCollection<FilterItemBase> filters, string rawSetting)
        {
            filters.Clear();
            if (string.IsNullOrEmpty(rawSetting))
                return false;
            using (XmlReader reader = XmlTextReader.Create(new StringReader(rawSetting)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableFilterItem>));
                var items = (List<SerializableFilterItem>)serializer.Deserialize(reader);
                foreach (SerializableFilterItem sfi in items)
                    filters.Add(CreateFilterItem(sfi.Name, CriteriaOperator.Parse(sfi.FilterCriteria)));
            }
            return true;
        }
    }
}
