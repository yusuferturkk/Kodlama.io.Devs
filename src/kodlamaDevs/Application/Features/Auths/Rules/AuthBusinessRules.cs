using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using Core.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auths.Rules
{
    public class AuthBusinessRules
    {
        IUserRepository _repository;

        public AuthBusinessRules(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task EmailCanNotBeDuplicatedWhenRegistered(string email)
        {
            User? user = await _repository.GetAsync(u => u.Email == email);
            if (user != null) throw new BusinessException("Mail already exists.");
        }

        public void CheckIfUserExists(User user)
        {
            if (user == null) throw new BusinessException("User is not found");
        }

        public void CheckIfPasswordIsCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (!HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
            {
                throw new BusinessException("Password is not correct");
            }
        }
    }
}
