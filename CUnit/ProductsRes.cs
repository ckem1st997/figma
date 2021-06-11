using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Data;
using figma.Models;

namespace figma.CUnit
{
    public class ProductsRes : GenericRepository<Products>, IProductRes
    {
        public ProductsRes(ShopProductContext context) : base(context)
        {

        }

    }
}