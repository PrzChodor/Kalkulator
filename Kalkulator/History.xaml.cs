using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy History.xaml
    /// </summary>
    public partial class History : Window
    {
        public History()
        {
            InitializeComponent();
            historyListBox.ItemsSource = ((MainWindow)Application.Current.MainWindow).HistoryList;
        }

        private void historyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (historyListBox.SelectedItem != null)
            {
                string text = ((Result)historyListBox.SelectedItem).Text;
                ((MainWindow)Application.Current.MainWindow).historyLabel.Content = text;
                ((MainWindow)Application.Current.MainWindow).inputTextBox.Text = text.Substring(text.IndexOf("=") + 2);
                ((MainWindow)Application.Current.MainWindow).Oper = ' ';
                ((MainWindow)Application.Current.MainWindow).B = Double.Parse(text.Substring(text.IndexOf("=") + 2));
            }
        }

        private void Window_GotMouseCapture(object sender, MouseEventArgs e)
        {
            historyListBox.UnselectAll();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            Grid g = (Grid)sender; 
            Binding b = new Binding("ActualWidth");
            b.Source = historyListBox;
            b.Converter = new CustomConverter();

            g.SetBinding(Grid.WidthProperty, b);
        }

    }

    public class Result : INotifyPropertyChanged
    {
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class CustomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) - 40;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
