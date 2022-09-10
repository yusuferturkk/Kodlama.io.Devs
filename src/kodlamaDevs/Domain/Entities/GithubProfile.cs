using Core.Persistence.Repositories;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GithubProfile : Entity
    {
        public int UserId { get; set; }
        public string ProfileUrl { get; set; }
        public virtual User? User { get; set; }

        public GithubProfile()
        {

        }

        public GithubProfile(int id, int userId, string profileUrl):this()
        {
            Id = id;
            UserId = userId;
            ProfileUrl = profileUrl;
        }
    }
}
