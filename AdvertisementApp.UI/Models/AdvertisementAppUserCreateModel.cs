using AdvertisementApp.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace AdvertisementApp.UI.Models
{
    public class AdvertisementAppUserCreateModel
    {
        public int AppUserId { get; set; }
        public int AdvertisementId { get; set; }
        public int AdvertisementAppUserStatusId { get; set; } = (int)AdvertisementAppUserStatusType.Applied;
        public int MilitaryStatusId { get; set; }
        public DateTime? EndDate { get; set; }
        public int WorkExperience { get; set; }
        public IFormFile CvPath { get; set; }
    }
}
