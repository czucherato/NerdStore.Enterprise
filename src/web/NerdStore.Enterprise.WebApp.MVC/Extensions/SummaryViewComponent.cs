using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}