using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MVVM.Views;

public partial class ProductEditorWindow : Window
{
    public ProductEditorWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
        => Close(true);

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
        =>  Close(false);    
}