using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWayPOS.WPF_UI.Interface
{
    /// <summary>
    /// The base interface for view models representing a single entity.
    /// </summary>
    /// <typeparam name="TEntity">An entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">An entity primary key type.</typeparam>
    public interface ISingleObjectViewModel<TEntity, TPrimaryKey>
    {

        /// <summary>
        /// The entity represented by a view model.
        /// </summary>
        /// <returns></returns>
        TEntity Entity { get; }

        /// <summary>
        /// The entity primary key value.
        /// </summary>
        /// <returns></returns>
        TPrimaryKey PrimaryKey { get; }

        /// <summary>
        /// Entity Name that this ViewModel Presents
        /// e.g Product, Customer
        /// </summary>
        string EntityName { get; }
    }
}
