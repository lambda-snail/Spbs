using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

public partial class InstitutionSelectorComponent : SelectableListComponent<Institution>
{
    private string _country = "se";

    private string? _institutionFilter = string.Empty;
    private string? InstitutionFilter
    {
        get => _institutionFilter;
        set
        {
            _institutionFilter = value;
            FilterOnChange();
        }
    }

    private List<Institution>? _allInstitutionsBackup = null; // When filtering, store the original institutions list here
    private List<Institution>? _institutions = null;
    protected override List<Institution>? GetList() => _institutions;
    
#pragma warning disable CS8618
    [Inject] public IMapper Mapper { get; set; }
    [Inject] public INordigenApiClient Client { get; set; }

    [Parameter]
    public RenderFragment? ParentToolBar { get; set; }
    private MudTextField<string> _searchBar;
#pragma warning restore CS8618

    public RenderFragment GetToolbarComponents()
    {
        return builder =>
        {
            builder.OpenComponent<MudStack>(0);
            builder.AddAttribute(1, "Row", true);
            builder.AddAttribute(2, "Spacing", 2);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(builder2 =>
            {
                // Text field                
                builder2.OpenComponent<MudTextField<string>>(4);
                builder2.AddAttribute(5, "Class", "pb-5 ps-2");
                builder2.AddAttribute(6, "Variant", Variant.Text);
                builder2.AddAttribute(7, "Placeholder", "Search Institution");
                builder2.AddAttribute(8, "Value", InstitutionFilter);
                builder2.AddAttribute(9, "ValueChanged", EventCallback.Factory.Create(this,
                    Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers
                        .CreateInferredEventCallback(this, __value => InstitutionFilter = __value,
                            InstitutionFilter)));
                builder2.AddComponentReferenceCapture(10, (__value) => { _searchBar = (MudTextField<string>)__value; });
                builder2.CloseComponent();
                
                // Button
                builder2.OpenComponent<MudIconButton>(11);
                builder2.AddAttribute(12, "Icon", Icons.Material.Outlined.Search);
                builder2.AddAttribute(13, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, FilterOnChange));
                builder2.AddAttribute(14, "Color", Color.Secondary);
                builder2.CloseComponent();
            }));
            builder.CloseComponent();
        };
    }
    
    protected override async Task OnInitializedAsync()
    {
        await GetListOfInstitutions();
    }

    private async Task GetListOfInstitutions()
    {
        var aspsps = await Client.GetListOfInstitutionsAsync(_country);
        _institutions = Mapper.Map<List<Institution>>(aspsps);

#if DEBUG
        _institutions.Add(new Institution { Name = "Sandbox", Id = "SANDBOXFINANCE_SFIN0000", Bic = string.Empty });
#endif
        
        _allInstitutionsBackup = _institutions;
        StateHasChanged();
    }
    
    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    public Institution? GetSelectedInstitution()
    {
        int? i = GetSelected();
        if (i is null || _institutions is null)
        {
            return null;
        }

        return _institutions[i.Value];
    }

    private void FilterOnChange()
    {
        if (string.IsNullOrWhiteSpace(_institutionFilter))
        {
            _institutions = _allInstitutionsBackup;
        }
        else
        {
            string filterToLower = _institutionFilter.ToLower();
            _institutions = _allInstitutionsBackup?
                .Where(i => i.Name.ToLower().Contains(filterToLower) || i.Bic.ToLower().Contains(filterToLower))
                .ToList();    
        }

        StateHasChanged();
    }
}