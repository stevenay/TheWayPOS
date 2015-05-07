using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWayPOS.DAL
{
    interface IUnitOfWorkFactory<TUnitOfWork>
    {
        TUnitOfWork CreateNew();
    }
}
