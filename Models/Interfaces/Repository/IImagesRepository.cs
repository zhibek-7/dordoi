using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IImagesRepository : IBaseRepositoryAsync<Image>
    {
        Task<bool> UpdateAsync(Image image);
        Task<int> GetFilteredCountAsync(
            Guid userId,
            Guid projectId,
            string imageNameFilter
            );
        Task<IEnumerable<Image>> GetFilteredAsync(
            Guid userId,
            Guid projectId,
            string imageNameFilter,
            int limit,
            int offset
            );
    }
}
