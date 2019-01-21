using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {

        private readonly ParticipantRepository _participantsRepository;

        public ParticipantsController()
        {
            this._participantsRepository = new ParticipantRepository();
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
                sortAscending: param.sortAscending ?? true
                );
        }

        [HttpDelete("byProjectId/{projectId}/{userId}")]
        public async Task DeleteParticipant(int projectId, int userId)
        {
            await this._participantsRepository.SetInactiveAsync(projectId: projectId, userId: userId);
        }

        [HttpPost("{projectId}/{userId}/{roleId}")]
        public async Task AddOrActivateParticipant(int projectId, int userId, int roleId)
        {
            await this._participantsRepository.AddOrActivateParticipant(projectId: projectId, userId: userId, roleId: roleId);
        }

    }
}
