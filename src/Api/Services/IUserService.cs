using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.Abstractions
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(Dictionary<string, string> filter);
    }
}