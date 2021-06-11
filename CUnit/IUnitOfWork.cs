using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.CUnit
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRes Products { get; }
        int Complete();
    }
}
