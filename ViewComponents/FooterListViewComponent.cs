using figma.DAL;
using figma.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class FooterListViewComponent : ViewComponent
    {
        private readonly IDapper _dapper;

        public FooterListViewComponent(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _dapper.Get<ConfigSites>("select * from ConfigSites", null, CommandType.Text);
            return View(items);
        }
    }
}
