using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubProfiles.Dtos
{
    public class DeletedGithubProfileDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string ProfileUrl { get; set; }
    }
}
