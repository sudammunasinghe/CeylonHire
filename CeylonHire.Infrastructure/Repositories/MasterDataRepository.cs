using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Infrastructure.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Select_JobMasterData;
        public MasterDataRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobMasterData = _queryLoader.Load("MasterData", "Select_JobMasterData.sql");
        }

        public async Task<JobMasterDataResult?> GetJobMasterDataAsync()
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(_Select_JobMasterData);

            return new JobMasterDataResult
            {
                jobTypes = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                jobModes = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                experienceLevels = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                skills = (await multi.ReadAsync<MasterDataEntity>()).ToList()
            };
        }
    }
}
