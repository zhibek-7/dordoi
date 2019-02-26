﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : BaseController
    {

        private readonly ParticipantRepository _participantsRepository;
        private readonly UserActionRepository _userActionRepository;
        public ParticipantsController()
        {
            this._participantsRepository = new ParticipantRepository(Settings.GetStringDB());
            _userActionRepository = new UserActionRepository(Settings.GetStringDB());
        }

        public class GetParticipantsByProjectIdParam
        {
            public string search { get; set; }
            public int[] roleIds { get; set; }
            public int[] localeIds { get; set; }
            public int? offset { get; set; }
            public int? limit { get; set; }
            public string[] sortBy { get; set; }
            public bool? sortAscending { get; set; }
            public string[] roleShort { get; set; }
        }
        [HttpPost("byProjectId/{projectId}/list")]
        public async Task<IEnumerable<ParticipantDTO>> GetParticipantsByProjectIdAsync(
            int projectId,
            [FromBody] GetParticipantsByProjectIdParam param
            )
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._participantsRepository
                        .GetAllByProjectIdCountAsync(
                            projectId: projectId,
                            search: param.search,
                            roleIds: param.roleIds,
                            localeIds: param.localeIds))
                    .ToString());
            return await this._participantsRepository.GetByProjectIdAsync(
                projectId: projectId,
                search: param.search,
                roleIds: param.roleIds,
                localeIds: param.localeIds,
                limit: param.limit ?? 25,
                offset: param.offset ?? 0,
                sortBy: param.sortBy,
                sortAscending: param.sortAscending ?? true,
                roleShort: param.roleShort
                );
        }

        [HttpDelete("byProjectId/{projectId}/{userId}")]
        public async Task DeleteParticipant(int projectId, int userId)
        {
            _userActionRepository.AddOrActivateParticipantAsync(300, "Test user", projectId, userId);//TODO поменять на пользователя когда будет реализована авторизация
            await this._participantsRepository.SetInactiveAsync(projectId: projectId, userId: userId);
        }

        [HttpPost("{projectId}/{userId}/{roleId}")]
        public async Task AddOrActivateParticipant(int projectId, int userId, int roleId)
        {
            _userActionRepository.DeleteParticipantAsync(300, "Test user", projectId, userId);//TODO поменять на пользователя когда будет реализована авторизация
            await this._participantsRepository.AddOrActivateParticipant(projectId: projectId, userId: userId, roleId: roleId);
        }

    }
}
