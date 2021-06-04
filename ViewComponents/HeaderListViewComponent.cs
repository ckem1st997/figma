using figma.DAL;
using figma.Models;
using figma.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class HeaderListViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapper _dapper;


        public HeaderListViewComponent(UnitOfWork context, IHttpContextAccessor httpContextAccessor, IDapper dapper)
        {
            _dapper = dapper;
            _unitOfWork = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = _httpContextAccessor.HttpContext.Request.Cookies["CartID"] == null ? "" : _httpContextAccessor.HttpContext.Request.Cookies["CartID"];
            var items = new HeaderViewModel()
            {
                //ConfigSites = _unitOfWork.ConfigSiteRepository.GetAync().Result.FirstOrDefault(),
                //ArticleCategories = await _unitOfWork.ArticleCategoryRepository.GetAync(),
                //ProductCategories = await _unitOfWork.ProductCategoryRepository.GetAync(a => a.Active && a.Home, q => q.OrderBy(a => a.Soft)),
                //Carts = await _unitOfWork.CartRepository.GetAync(a => a.CartID == id)
                ConfigSites = await _dapper.GetAync<ConfigSites>("select * from ConfigSites", null, CommandType.Text),
                //  ArticleCategories = await _unitOfWork.ArticleCategoryRepository.GetAync(),
                ProductCategories = await _dapper.GetAllAync<ProductCategories>("select ParentId,Name,ProductCategorieID from ProductCategories where Active=1  and Home=1 order by Soft", null, CommandType.Text),
                Carts = await _dapper.GetAync<int>("select COUNT(*) from Carts where CartID='" + id + "'", null, CommandType.Text)
            };
            return View(items);
        }
    }
}
