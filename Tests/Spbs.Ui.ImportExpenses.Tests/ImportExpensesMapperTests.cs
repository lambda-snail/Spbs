using AutoMapper;
using Spbs.Ui.Features.BankIntegration.ImportExpenses.Mapping;

namespace Spbs.Ui.ImportExpenses.Tests;

public class ImportExpensesMapperTests
{
    private readonly MapperConfiguration _configuration;
    private readonly IMapper _mapper;

    public ImportExpensesMapperTests()
    {
        _configuration = new MapperConfiguration(
            cfg => cfg.AddProfile(new ImportExpensesViewModelMapper())
        );

        _mapper = _configuration.CreateMapper();
    }
    
    [Fact(DisplayName = "Test that the mapper configuration is valid")]
    public void MapperConfiguration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }
}
