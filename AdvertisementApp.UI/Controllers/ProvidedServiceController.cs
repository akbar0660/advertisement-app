using AdvertisementApp.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvertisementApp.UI.Controllers
{
    public class ProvidedServiceController : Controller
    {
        private readonly IProvidedServiceService _providedService;

        public ProvidedServiceController(IProvidedServiceService providedService)
        {
            _providedService = providedService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _providedService.GetAllAsync();
            return View();
        }
    }
}
