using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MVVM.Models.DTO.Auth;
using MVVM.Services;
using MVVM.ViewModels;
using MVVM.Views;

namespace MVVM.Services;

public sealed class NavigationService
{
    public void ShowAdmin(ApiService apiService, AuthResponseDto dto)
        => ShowWindow(new AdminWindow { DataContext = new AdminViewModel(apiService, dto, this) });

    public void ShowClient(ApiService apiService, AuthResponseDto dto)
        => ShowWindow(new ClientWindow { DataContext = new ClientViewModel(apiService, dto) });

    public async Task<bool?> ShowCategoryEditorAsync(CategoryEditorViewModel vm)
    {
        var window = new CategoryEditorWindow { DataContext = vm };
        
        if(Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return false;
        
        return await window.ShowDialog<bool?>(desktop.MainWindow);
    }
    
    public async Task<bool?> ShowProductEditorAsync(ProductEditorViewModel vm)
    {
        var window = new ProductEditorWindow() { DataContext = vm };
        
        if(Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return false;
        
        return await window.ShowDialog<bool?>(desktop.MainWindow);
    }

    private static void ShowWindow(Window nextWindow)
    {
        if(Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var currentWindow = desktop.MainWindow;
        desktop.MainWindow = nextWindow;
        nextWindow.Show();
        currentWindow?.Close();
    }
}
