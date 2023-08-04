

using AdvertisementApp.Business.Mappings.AutoMapper;
using AutoMapper;
using System.Collections.Generic;

namespace AdvertisementApp.Business.ProfileHelper
{
    public class ProfileHelper
    {
        public static List<Profile> GetProfiles()
        {
            return new List<Profile>{
                new ProvidedServiceProfile(),
                new AdvertisementProfile(),
                new GenderProfile(),
                new AppUserProfile(),
                new AppRoleProfile(),
                new AdvertisementAppUserProfile(),
                new AdvertisementAppUserStatusProfile(),
                new MilitaryStatusProfile()
            };
        }
    }
}
