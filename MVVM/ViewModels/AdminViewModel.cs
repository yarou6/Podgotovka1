using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVM.Models.DTO.Auth;
using MVVM.Models.DTO.Categorys;
using MVVM.Models.DTO.Products;
using MVVM.Services;
using MVVM.Tools;

namespace MVVM.ViewModels;

public partial class AdminViewModel : BaseVM
{
    private readonly ApiService apiService;
    private readonly NavigationService navigationService;

    private string title = string.Empty;
    private string errorMessange = string.Empty;
    private ObservableCollection<CategoryReadDto> categories = new();
    private ObservableCollection<ProductReadDto> products = new();

    private ProductReadDto? selectedProduct;
    private CategoryReadDto? selectedCategory;
    
    private readonly RelayCommand refreshCommand;
    private readonly RelayCommand addProductCommand;
    private readonly RelayCommand addCategoryCommand;
    private readonly RelayCommand deleteProductCommand;
    private readonly RelayCommand deleteCategoryCommand;
    private readonly RelayCommand editProductCommand;
    private readonly RelayCommand editCategoryCommand;


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

    public ProductReadDto? SelectedProduct
    {
        get => selectedProduct;
        set
        {
            if (SetField(ref selectedProduct, value))
            {
                editProductCommand.RaiseCanExecuteChanged();
                deleteProductCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public CategoryReadDto? SelectedCategory
    {
        get => selectedCategory;
        set
        {
            if (SetField(ref selectedCategory, value))
            {
                editCategoryCommand.RaiseCanExecuteChanged();
                deleteCategoryCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand AddProductCommand => addProductCommand;
    public ICommand AddCategoryCommand => addCategoryCommand;
    public ICommand DeleteProductCommand => deleteProductCommand;
    public ICommand DeleteCategoryCommand => deleteCategoryCommand;
    public ICommand EditProductCommand => editProductCommand;
    public ICommand EditCategoryCommand => editCategoryCommand;
    public RelayCommand RefreshCommand => refreshCommand;
    

    public  AdminViewModel(ApiService apiService, AuthResponseDto dto, NavigationService navigationService)
    {
        this.apiService = apiService;
        this.navigationService = navigationService;
        
        Title = $"Admin: {dto.FullName}";
        
        refreshCommand = new RelayCommand(() => LoadAsync());
        
        addProductCommand = new RelayCommand(() =>  AddProductAsync());
        editProductCommand = new RelayCommand(() =>  EditProductAsync(), () => SelectedProduct is not null);
        deleteProductCommand= new RelayCommand(() =>  DeleteProductAsync(), () => SelectedProduct is not null);
        
        addCategoryCommand = new RelayCommand(() =>  AddCategoryAsync());
        editCategoryCommand = new RelayCommand(() =>  EditCategoryAsync(), () => SelectedCategory is not null);
        deleteCategoryCommand= new RelayCommand(() =>  DeleteCategoryAsync(), () =>  SelectedCategory is not  null);
        
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            ErrorMessange = string.Empty;
            
            var products = await apiService.GetProductAsync(includeInactive: true);
            var categories = await apiService.GetCategoriesAsync();
            Categories = new ObservableCollection<CategoryReadDto>(categories);
            Products = new ObservableCollection<ProductReadDto>(products);

            SelectedProduct = Products.FirstOrDefault();
            SelectedCategory = Categories.FirstOrDefault();

        }
        catch(Exception ex)
        {
            ErrorMessange = ex.Message;
        }
    }
    
    
    private async Task AddProductAsync()
    {
        try
        {
            var vm = new ProductEditorViewModel(Categories);
            var result = await navigationService.ShowProductEditorAsync(vm);

            if (result != true)
                return;

            await apiService.CreateProductAsync(vm.ToCreateDto());
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessange = ex.Message;
        }
    }
    
    private async Task AddCategoryAsync()
    {
        try
        {
            var vm = new CategoryEditorViewModel();
            var result = await navigationService.ShowCategoryEditorAsync(vm);

            if (result != true)
                return;

            await apiService.CreateCategoryAsync(vm.ToCreateDto());
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessange = ex.Message;
        }
    }

    
    private async Task EditProductAsync()
    {
        if(SelectedProduct is null)
            return;

        try
        {
            var vm = new ProductEditorViewModel(Categories, SelectedProduct);
            var result = await navigationService.ShowProductEditorAsync(vm);

            if(result != true ) return;
            
            await apiService.UpdateProductAsync(SelectedProduct.Id, vm.ToUpdateDto());
            await LoadAsync();
        }
        catch (Exception e)
        {
            errorMessange = e.Message;
        }
    }
    

    private async Task EditCategoryAsync()
    {
        if(SelectedCategory is null)
            return;

        try
        {
            var vm = new CategoryEditorViewModel(SelectedCategory);
            var result = await navigationService.ShowCategoryEditorAsync(vm);

            if(result != true ) return;
            
            await apiService.UpdateCategoryAsync(SelectedCategory.Id, vm.ToUpdateDto());
            await LoadAsync();
        }
        catch (Exception e)
        {
            errorMessange = e.Message;
        }
    }


    private async Task DeleteProductAsync()
    {
        if(SelectedProduct is null)
            return;

        try
        {
            await apiService.RemoveProductAsync(SelectedProduct.Id);
            await LoadAsync();
        }
        catch (Exception e)
        {
            errorMessange = e.Message;
        }
    } 
    private async Task DeleteCategoryAsync()
    {
        if(SelectedProduct is null)
            return;

        try
        {
            await apiService.RemoveCategoryAsync(SelectedCategory.Id);
            await LoadAsync();
        }
        catch (Exception e)
        {
            errorMessange = e.Message;
        }
    }

    private async Task RefreshAsync() => await LoadAsync();
}
