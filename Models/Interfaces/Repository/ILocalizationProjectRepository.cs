﻿using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ILocalizationProjectRepository
    {
        void Add(LocalizationProject locale);
        LocalizationProject GetByID(int Id);
        IEnumerable<LocalizationProject> GetAll();
        Task<IEnumerable<LocalizationProjectForSelectDTO>> GetAllForSelectDTOAsync();
        bool Remove(int Id);
        void Update(LocalizationProject user);
    }
}
