using Api.Services.Auth0;
using AutoMapper;
using Models;

namespace Api.Infrastructure
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserModel>(); // means you want to map from User to UserModel
        }
    }
}
