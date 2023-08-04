using AdvertisementApp.DataAccess.Interfaces;
using AdvertisementApp.DataAccess.Repositories;
using AdvertisementApp.Entities;
using System.Threading.Tasks;

namespace AdvertisementApp.DataAccess.UnitOfWork
{
    public interface IUow
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        Task SaveChangesAsync();
    }
}
