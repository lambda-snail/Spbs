using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;

namespace Spbs.Ui.Features.Users.Repositories;

public interface IUserRepository
{
    Task<User?> GetById(Guid id);
    Task<User?> UpsertUser(User user);
}

public class UserRepository : CosmosRepositoryBase<User>, IUserRepository
{
    public UserRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<UserRepository> logger) : base(client, options, CosmosTypeConstants.SpbsUser, logger)
    {
    }

    public Task<User?> UpsertUser(User user)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = user.UserId;
        }

        return Upsert(user);
    }
}