using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
    private readonly ILogger<IPurchaseRepository> _logger;
    private readonly IMongoCollection<PurchaseDto> _purchaseDb;

    public PurchaseRepository(IMongoClient mongoCLient, IMapper mapper, IOptions<SpbsDatabaseSettings> options, ILogger<IPurchaseRepository> logger)
    {
        var client = mongoCLient.GetDatabase(options.Value.DatabaseName);
        _purchaseDb = client.GetCollection<PurchaseDto>(options.Value.PurchasesCollection);
        _mapper = mapper;
        _logger = logger;
    }

    public async Task InsertPurchase(Purchase purchase)
    {
        PurchaseDto purchaseDto = _mapper.Map<PurchaseDto>(purchase);
        await _purchaseDb.InsertOneAsync(purchaseDto);
    }

    public async Task UpdatePurchase(Purchase purchase)
    {
        PurchaseDto purchaseDto = _mapper.Map<PurchaseDto>(purchase);
        var filter = Builders<PurchaseDto>.Filter.Where(p => p.Id == purchase.Id);
        await _purchaseDb.FindOneAndReplaceAsync(filter, purchaseDto);
    }

    public async Task<List<Purchase>> GetPurchasesOfUser(string userId, DateTime? since = null) // TODO: Add until parameter as well
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

    public async Task<Purchase> GetPurchaseById(Guid purchaseId)
    {
        var filter = Builders<PurchaseDto>.Filter.Where(purchase => purchase.Id == purchaseId);
        var result = await _purchaseDb.FindAsync(filter);
        PurchaseDto purchaseDto = await result.FirstAsync();
        return _mapper.Map<Purchase>(purchaseDto);
    }

    public async Task DeletePurchase(Guid purchaseId)
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