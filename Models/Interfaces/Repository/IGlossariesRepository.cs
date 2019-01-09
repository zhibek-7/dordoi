﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;
//using Models.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<DTO.Glossaries>
    {
        Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync();
    }
}
