using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Settings;
using Spbs.Main.InfraStructure.DtoModels;

namespace Spbs.Main.InfraStructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<UserDto> _userDb;

    public UserRepository(IMongoClient mongoCLient, IMapper mapper, IOptions<SpbsDatabaseSettings> options)
    {
        var client = mongoCLient.GetDatabase(options.Value.DatabaseName);
        _userDb = client.GetCollection<UserDto>(options.Value.UsersCollection);
        _mapper = mapper;
    }

    public async Task<User> GetUserById(Guid userId)
    {
        var filter = Builders<UserDto>.Filter.Where(user => user.Id == userId);
        var result = _userDb.FindAsync(filter);
        UserDto userDto = await result.Result.FirstAsync();
        return _mapper.Map<User>(userDto);
    }

    /// <summary>
    ///  Replace the user document in the db with the provided one, or insert if it cannot be found. Currently this replaces
    /// the entire document but future versions could specify which fields that should be updated, and only update those.
    /// </summary>
    public async Task UpsertUserData(User user)
    {
        UserDto userDto = _mapper.Map<UserDto>(user);
        var filter = Builders<UserDto>.Filter.Where(u => u.Id == user.Id);
        await _userDb.ReplaceOneAsync(filter, userDto, new ReplaceOptions() { IsUpsert = true});
    }
}