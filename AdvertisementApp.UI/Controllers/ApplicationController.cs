using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Dtos;
using AdvertisementApp.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvertisementApp.UI.Controllers
{
    public class ApplicationController : Controller
    {
        public readonly IAdvertisementService _advertisementService;

        public ApplicationController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        public async Task<IActionResult> List()
        {
            var response = await _advertisementService.GetAllAsync();
            return this.ResponseView(response);
        }

        public IActionResult Create()
        {
            return View(new AdvertisementCreateDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisementCreateDto dto)
        {
            var response = await _advertisementService.CreateAsync(dto);
            return this.ResponseRedirectToAction(response, "List");
        }
        public async Task<IActionResult> Update(int id)
        {
            var response = await _advertisementService.GetByIdAsync<AdvertisementUpdateDto>(id);
            return this.ResponseView(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdvertisementUpdateDto dto)
        {
            var response = await _advertisementService.UpdateAsync(dto);
            return this.ResponseRedirectToAction(response, "List");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var response = await _advertisementService.RemoveAsync(id);
            return this.ResponseRedirectToAction(response, "List");
        }
    }
}
