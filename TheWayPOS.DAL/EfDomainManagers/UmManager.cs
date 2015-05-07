namespace TheWayPOS.DAL.EfDomainManagers
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;

    // actually it's Product Repository
    class UmManager : DomainBase<DbContext, Entities.Um, int>, IUmManager
    {
        private readonly DbContext _context;

        public UmManager(DbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}