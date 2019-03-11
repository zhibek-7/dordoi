using System;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IImagesRepository : IBaseRepositoryAsync<Image>
    {
    }
}
