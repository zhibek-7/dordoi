using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.Participants;

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

        [HttpGet("byProjectId/{projectId}")]
        public async Task<IEnumerable<Participant>> GetParticipantsByProjectIdAsync(
            int projectId,
            [FromQuery] string search,
            [FromQuery] int[] roleIds,
            [FromQuery] int[] localeIds,
            [FromQuery] int? offset,
            [FromQuery] int? limit,
            [FromQuery] string[] sortBy,
            [FromQuery] bool? sortAscending
            )
        {
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._participantsRepository
                        .GetAllByProjectIdCountAsync(
                            projectId: projectId,
                            search: search,
                            roleIds: roleIds,
                            localeIds: localeIds))
                    .ToString());
            return await this._participantsRepository.GetByProjectIdAsync(
                projectId: projectId,
                search: search,
                roleIds: roleIds,
                localeIds: localeIds,
                limit: limit ?? 25,
                offset: offset ?? 0,
                sortBy: sortBy,
                sortAscending: sortAscending ?? true
                );
        }

        [HttpDelete("byProjectId/{projectId}/{userId}")]
        public async Task DeleteParticipant(int projectId, int userId)
        {
            await this._participantsRepository.SetInactiveAsync(projectId: projectId, userId: userId);
        }

    }
}
