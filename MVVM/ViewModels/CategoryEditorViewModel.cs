using MVVM.Models.DTO.Categorys;
using MVVM.Tools;

namespace MVVM.ViewModels;

public class CategoryEditorViewModel : BaseVM
{
    private string _name;
    private string _description;

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public CategoryEditorViewModel()
    {
        
    }

    public CategoryEditorViewModel(CategoryReadDto category)
    {
        Name = category.Name;
        Description = category.Description;
    }

    public CategoryCreateDto ToCreateDto() => new()
    {
        Name = Name,
        Description = Description
    };

    public CategoryUpdateDto ToUpdateDto() => new()
    {
        Name = Name,
        Description = Description
    };
}