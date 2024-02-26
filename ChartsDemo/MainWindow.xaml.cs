using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ScottPlot;

namespace ChartsDemo;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public ObservableCollection<PieSlice> Slices { get; set; }
    
    private PieSlice? _selectedSlice;
    public PieSlice? SelectedSlice
    {
        get => _selectedSlice;
        set => SetField(ref _selectedSlice, value);
    }
    
    public MainWindow()
    {
        Slices = new ObservableCollection<PieSlice>();
        
        InitializeComponent();
    }
    
    private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedSlice != null) Slices.Remove(SelectedSlice);
        
        SelectedSlice = null;

        InputLabel.Clear();
        InputValue.Clear();
        ColorPicker.SelectedColor = null;
    }
    
    private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
    {
        SelectedSlice = null;

        InputLabel.Clear();
        InputValue.Clear();
        ColorPicker.SelectedColor = null;
    }
    
    private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedSlice != null) return;
        
        if (ColorPicker.SelectedColor == null) return;
        
        var color = ColorPicker.SelectedColor.Value;
        Slices.Add(new PieSlice()
        {
            Value = Convert.ToDouble(InputValue.Text),
            Label = InputLabel.Text,
            FillColor = new Color(color.R, color.G, color.B, color.A)
        });
        
        SelectedSlice = null;

        InputLabel.Clear();
        InputValue.Clear();
        ColorPicker.SelectedColor = null;
    }
    
    private void ButtonShow_OnClick(object sender, RoutedEventArgs e)
    {
        Pie.Plot.Clear();
        
        var pie = Pie.Plot.Add.Pie(Slices);
        Pie.Plot.ShowLegend();
        pie.ExplodeFraction = 0.1;
        Pie.Refresh();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion
}