using System.Collections.ObjectModel;
using System.Linq;
using MVVM.Models.DTO.Categorys;
using MVVM.Models.DTO.Products;
using MVVM.Tools;

namespace MVVM.ViewModels;

public class ProductEditorViewModel : BaseVM
{
    private string name;
    
    private string description = string.Empty;
    private decimal price = 0;
    private uint stock = 0;
    
    private CategoryReadDto selectedCategory;
    
    public ObservableCollection<CategoryReadDto> Categories { get; set; }

    public string Name
    {
        get => name;
        set => SetField(ref name, value);
    }
    public string Description 
    {
        get => description;
        set => SetField(ref description, value);
    }
    public decimal Price
    {
        get => price;
        set => SetField(ref price, value);
    }

    public uint Stock
    {
        get => stock;
        set => SetField(ref stock, value);
    }
    
    public CategoryReadDto SelectedCategory { get => selectedCategory; set => SetField(ref selectedCategory, value); }

    public ProductEditorViewModel(ObservableCollection<CategoryReadDto> categories)
    {
        Categories = categories;
        SelectedCategory = Categories.FirstOrDefault();
    }
    
    public ProductEditorViewModel(ObservableCollection<CategoryReadDto> categories, Models.DTO.Products.ProductReadDto product)
    {
        Categories = categories;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        Stock = product.Stock;
        SelectedCategory = Categories.FirstOrDefault(c => c.Id == product.CategoryId) ?? Categories.FirstOrDefault();
    }


    public ProductCreateDto ToCreateDto() => new()
    {
        Name = Name,
        Description = Description,
        Price = Price,
        Stock = Stock,
        CategoryId = SelectedCategory?.Id ?? 0,
    };
    
    public ProductUpdateDto ToUpdateDto() => new()
    {
        Name = Name,
        Description = Description,
        Price = Price,
        Stock = Stock,
        CategoryId = SelectedCategory?.Id ?? 0,
        IsActive = true
    };
}