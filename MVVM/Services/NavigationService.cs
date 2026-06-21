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
        => ShowWindow(new AdminWindow { DataContext = new AdminViewModel(apiService, dto) });

    public void ShowClient(ApiService apiService, AuthResponseDto dto)
        => ShowWindow(new ClientWindow { DataContext = new ClientViewModel(apiService, dto) });

    private static void ShowWindow(Window nextWindow)
    {
        if(Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var currentWindow = desktop.MainWindow;
        desktop.MainWindow = currentWindow;
        nextWindow.Show();
        currentWindow?.Close();
    }
}
