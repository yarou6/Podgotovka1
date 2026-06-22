using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MVVM.Views;

public partial class CategoryEditorWindow : Window
{
    public CategoryEditorWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
        => Close(true);
  

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    => Close(false);
}