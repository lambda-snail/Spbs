using AutoMapper;
using Microsoft.Extensions.Options;
using Spbs.Main.Core.Contracts;

using MongoDB.Driver;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Settings;
using Spbs.Main.InfraStructure.DtoModels;

namespace Spbs.Main.InfraStructure.Persistence;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<PurchaseDto> _purchaseDb;

    public PurchaseRepository(IMongoClient mongoCLient, IMapper mapper, IOptions<SpbsDatabaseSettings> options)
    {
        var client = mongoCLient.GetDatabase(options.Value.DatabaseName);
        _purchaseDb = client.GetCollection<PurchaseDto>(options.Value.PurchasesCollection);
        _mapper = mapper;
    }

    public async Task UpsertPurchase(Purchase purchaseModel)
    {
        PurchaseDto purchaseDto = _mapper.Map<PurchaseDto>(purchaseModel);
        if (purchaseDto is null)
        {
            return;
        }
        
        await _purchaseDb.ReplaceOneAsync(
            purchase => purchase.OwnerId == purchaseDto.OwnerId,
            purchaseDto,
            new ReplaceOptions { IsUpsert = true }
        );
    }
    
    public async Task<List<Purchase>> GetPurchasesOfUser(string userId, DateTime? since = null)
    {
        var filter = Builders<PurchaseDto>.Filter.Where(purchase => purchase.OwnerId == userId);

        if (since is not null)
        {
            var dateFilter = Builders<PurchaseDto>.Filter.Where(purchase => purchase.PurchaseDateTime > since);
            filter = Builders<PurchaseDto>.Filter.And(filter, dateFilter);
        }
        
        var result = await _purchaseDb.FindAsync(filter);
        List<PurchaseDto> items = await result.ToListAsync();
        return _mapper.Map<List<Purchase>>(items);
    }

    public async Task DeletePurchase(int purchaseId)
    {
        var filter = Builders<PurchaseDto>.Filter.Where(purchase => purchase.Id == purchaseId);
        await _purchaseDb.DeleteOneAsync(filter);
    }

    public async Task DeleteAllPurchasesOfUser(string userId)
    {
        var filter = Builders<PurchaseDto>.Filter.Where(purchase => purchase.OwnerId == userId);
        await _purchaseDb.DeleteOneAsync(filter);
    }
}