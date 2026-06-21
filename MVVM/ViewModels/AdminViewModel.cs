using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVM.Models.DTO.Auth;
using MVVM.Models.DTO.Categorys;
using MVVM.Models.DTO.Products;
using MVVM.Services;
using MVVM.Tools;

namespace MVVM.ViewModels;

public class AdminViewModel : BaseVM
{
    private readonly ApiService apiService;

    private string title = string.Empty;
    private string errorMessange = string.Empty;
    private ObservableCollection<CategoryReadDto> categories = new();
    private ObservableCollection<ProductReadDto> products = new();

    public string Title
    {
        get => title;
        set => SetField(ref title, value);
    }

    public string ErrorMessange
    {
        get => errorMessange;
        set => SetField(ref errorMessange, value);
    }

    public ObservableCollection<ProductReadDto> Products
    {
        get => products;
        set => SetField(ref products,  value);
    }

    public ObservableCollection<CategoryReadDto> Categories 
    {
        get => categories;
        set => SetField(ref categories,  value);
    }


    public ICommand RefreshCommand { get; }

    public  AdminViewModel(ApiService apiService, AuthResponseDto dto)
    {
        this.apiService = apiService;
        Title = $"Admin: {dto.FullName}";
        RefreshCommand = new RelayCommand(() => _ = RefreshAsync());
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            var products = await apiService.GetProductAsync(includeInactive: true);
            var categories = await apiService.GetCategoriesAsync();
            Categories = new ObservableCollection<CategoryReadDto>(categories);
            

        }
        catch(Exception ex)
        {
            ErrorMessange = ex.Message;
        }
    }

    private async Task RefreshAsync() => await LoadAsync();
}
