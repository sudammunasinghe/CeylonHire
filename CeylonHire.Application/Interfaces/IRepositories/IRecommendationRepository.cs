using CeylonHire.Application.DTOs.RecommendedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IRecommendationRepository
    {
        Task<List<RecommendedUserDto>> GetRecommendedUsersAsync(int jobId);
    }
}
