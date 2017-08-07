using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWMTaskTracker.DAL
{
    // generic db object for storing key, value pairs
    class DbEnumeration
    {
        public long Id { get; }
        public string Name { get; }

        public DbEnumeration(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
