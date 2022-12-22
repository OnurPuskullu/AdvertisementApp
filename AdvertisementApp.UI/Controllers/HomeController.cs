using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisementApp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProvidedServiceService _providedServiceService;
        private readonly IAdvertisementService _advetisementService;

        public HomeController(IProvidedServiceService providedServiceService, IAdvertisementService advetisementService)
        {
            _providedServiceService = providedServiceService;
            _advetisementService = advetisementService;
        }

        public async Task<IActionResult> Index()
        {
            var response=await _providedServiceService.GetAllAsync();
            return this.ResponseView(response);
        }
        public async Task<IActionResult> HumanResource()
        {
            var response=await _advetisementService.GetActiveAsync();
            return this.ResponseView(response);
        }
    }
}
