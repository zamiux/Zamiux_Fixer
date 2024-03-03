using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class ShowTagsViewComponent : ViewComponent
    {
        private readonly ZamiuxFixerDbContext context;

        public ShowTagsViewComponent(ZamiuxFixerDbContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Reslut = context.QuestionTags
                .Select(x => x.Tag)
                .Distinct()
                .Take(25)
                .ToList();
            return View("ShowTags", Reslut);
        }
    }
}
