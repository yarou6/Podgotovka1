using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVM.Models.DTO.Auth;
using MVVM.Services;
using MVVM.Tools;

namespace MVVM.ViewModels;

public class LoginViewModel : BaseVM
{
    private readonly ApiService apiService;
    private readonly NavigationService navigationService;

    private string email = string.Empty;
    private string password = string.Empty;
    private string errorMessange = string.Empty;
    private bool isBusy;

    public string Email
    {
        get => email;
        set => SetField(ref email, value);
    }

    public string Password
    {
        get => password;
        set => SetField(ref password, value);
    }

    public string ErrorMessange
    {
        get => errorMessange;
        set => SetField(ref errorMessange, value);
    }

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (SetField(ref isBusy, value))
            {
                loginCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private readonly RelayCommand loginCommand;
    public ICommand LoginCommand => loginCommand;
    
    public LoginViewModel(ApiService apiService,  NavigationService navigationService)
    {
        this.apiService = apiService;
        this.navigationService = navigationService;
        loginCommand = new RelayCommand(() => _ = LoginAsync(), () => !IsBusy);
    }

    public async Task LoginAsync()
    {
        ErrorMessange = string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessange = "Введите email и password";
            return;
        }

        IsBusy = true;
        try
        {
            var result = await apiService.LoginAsync(new LoginDto
            {
                Email = email,
                Password = password
            });

            if (result is null)
            {
                ErrorMessange = "Неверный email или password";
                return;
            }
            
            if(result.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                navigationService.ShowAdmin(apiService, result);
            else
                navigationService.ShowClient(apiService, result);

        }
        catch (Exception ex)
        {
            ErrorMessange = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

}
