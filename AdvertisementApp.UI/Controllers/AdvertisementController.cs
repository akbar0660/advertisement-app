using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Common.Enums;
using AdvertisementApp.Dtos;
using AdvertisementApp.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdvertisementApp.UI.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAdvertisementAppUserService _advertisementAppUserService;

        public AdvertisementController(IAppUserService appUserService, IAdvertisementAppUserService advertisementAppUserService)
        {
            _appUserService = appUserService;
            _advertisementAppUserService = advertisementAppUserService;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var list = await _advertisementAppUserService.GetListAsync(AdvertisementAppUserStatusType.Applied);
            return View(list);
        }




        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Apply(int advertisementId)
        {
            var userId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);
            var userResponse = await _appUserService.GetByIdAsync<AppUserListDto>(userId);
            var userInfo = userResponse.Data;
            ViewBag.GenderId = userResponse.Data.GenderId;


            var items = Enum.GetValues(typeof(MilitaryStatusType));

            var statuses = new List<MilitaryStatusListDto>();

            foreach (int item in items)
            {
                statuses.Add(new MilitaryStatusListDto
                {
                    Id = item,
                    Definition = Enum.GetName(typeof(MilitaryStatusType), item)
                });
            }

            ViewBag.MilitaryStatuses = statuses;

            return View(new AdvertisementAppUserCreateModel
            {
                AdvertisementId = advertisementId,
                AppUserId = userId
            });
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Apply(AdvertisementAppUserCreateModel model)
        {
            AdvertisementAppUserCreateDto dto = new();
            if (model.CvPath != null)
            {
                var filename = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(model.CvPath.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cvFiles", filename + extName);
                var stream = new FileStream(path, FileMode.Create);
                await model.CvPath.CopyToAsync(stream);
                dto.CvPath = path;
            }
            dto.AdvertisementAppUserStatusId = model.AdvertisementAppUserStatusId;
            dto.AppUserId = model.AppUserId;
            dto.AdvertisementId = model.AdvertisementId;
            dto.EndDate = model.EndDate;
            dto.WorkExperience = model.WorkExperience;
            dto.MilitaryStatusId = model.MilitaryStatusId;

            var response = await _advertisementAppUserService.CreateAsync(dto);
            if (response.ResponseType == Common.ResponseType.ValidationError)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var userId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);
                var userResponse = await _appUserService.GetByIdAsync<AppUserListDto>(userId);
                var userInfo = userResponse.Data;
                ViewBag.GenderId = userResponse.Data.GenderId;

                var items = Enum.GetValues(typeof(MilitaryStatusType));

                var statuses = new List<MilitaryStatusListDto>();

                foreach (int item in items)
                {
                    statuses.Add(new MilitaryStatusListDto
                    {
                        Id = item,
                        Definition = Enum.GetName(typeof(MilitaryStatusType), item)
                    });
                }

                ViewBag.MilitaryStatuses = statuses;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus(int advertisementAppUserId, AdvertisementAppUserStatusType type)
        {
            await _advertisementAppUserService.SetStatusAsync(advertisementAppUserId, type);
            return RedirectToAction("List", "Advertisement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovedList()
        {
            var list = await _advertisementAppUserService.GetListAsync(AdvertisementAppUserStatusType.Interview);
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectedList()
        {
            var list = await _advertisementAppUserService.GetListAsync(AdvertisementAppUserStatusType.Bad);
            return View(list);
        }

    }
}
