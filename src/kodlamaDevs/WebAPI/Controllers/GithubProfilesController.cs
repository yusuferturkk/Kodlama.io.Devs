using Application.Features.GithubProfiles.Commands.CreateGithubProfile;
using Application.Features.GithubProfiles.Commands.DeleteGithubProfile;
using Application.Features.GithubProfiles.Commands.UpdateGithubProfile;
using Application.Features.GithubProfiles.Dtos;
using Application.Features.GithubProfiles.Models;
using Application.Features.GithubProfiles.Queries.GetListGithubProfile;
using Core.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubProfilesController : BaseController
    {
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateGithubProfileCommand command)
        {
            CreatedGithubProfileDto result = await Mediator.Send(command);
            return Created("", result);
        }

        [HttpPost("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteGithubProfileCommand command)
        {
            DeletedGithubProfileDto result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateGithubProfileCommand command)
        {
            UpdatedGithubProfileDto result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListGithubProfileQuery query = new GetListGithubProfileQuery { PageRequest = pageRequest };
            GithubProfileListModel result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
