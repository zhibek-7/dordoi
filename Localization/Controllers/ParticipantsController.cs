﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;
using Utilities;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {

        private readonly ParticipantRepository _participantsRepository;
        private readonly UserActionRepository _userActionRepository;
        private UserRepository ur;
        public ParticipantsController()
        {
            var connectionStr = Settings.GetStringDB();
            this._participantsRepository = new ParticipantRepository(connectionStr);
            _userActionRepository = new UserActionRepository(connectionStr);
            ur = new UserRepository(connectionStr);
        }

        public class GetParticipantsByProjectIdParam
        {
            public string search { get; set; }
            public Guid[] roleIds { get; set; }
            public Guid[] localeIds { get; set; }
            public int? offset { get; set; }
            public int? limit { get; set; }
            public string[] sortBy { get; set; }
            public bool? sortAscending { get; set; }
            public string[] roleShort { get; set; }
        }

        [Authorize]
        [HttpPost("byProjectId/{projectId}/list")]
        public async Task<IEnumerable<ParticipantDTO>> GetParticipantsByProjectIdAsync(
            Guid projectId,
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
        [Authorize]
        [HttpDelete("byProjectId/{projectId}/{userId}")]
        public async Task DeleteParticipant(Guid projectId, Guid userId)
        {
            var name_text = User.Identity.Name;
            Guid user_Id = (Guid)ur.GetID(name_text);
            await _userActionRepository.DeleteParticipantAsync(user_Id, name_text, projectId, userId);
            await this._participantsRepository.SetInactiveAsync(projectId: projectId, userId: userId);
        }

        [Authorize]
        [HttpPost("{projectId}/{userId}/{roleId}")]
        public async Task AddOrActivateParticipant(Guid projectId, Guid userId, Guid roleId)
        {
            var name_text = User.Identity.Name;
            Guid user_Id = (Guid)ur.GetID(name_text);
            await _userActionRepository.AddOrActivateParticipantAsync(user_Id, name_text, projectId, userId, roleId);

            await this._participantsRepository.AddOrActivateParticipant(projectId: projectId, userId: userId, roleId: roleId);
        }

    }
}
