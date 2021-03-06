﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double a = 0;
        private double b = 0;
        private char oper = ' ';
        private bool changed = true; 
        private bool computed = false;
        private bool error = false;
        private string separator;
        private ObservableCollection<Result> historyList;

        private string current;

        public MainWindow()
        {
            InitializeComponent();
            separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            Resources.Add("decimalSeparator", separator);
            historyList = new ObservableCollection<Result>();
            historyListBox.ItemsSource = historyList;
        }

        private void Compute(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if(!changed && oper != ' ')
                current = a.ToString() + " " + oper.ToString();

            switch (oper)
            {
                case '+':
                    a += b;
                    break;
                case '-':
                    a -= b;
                    break;
                case '×':
                    a *= b;
                    break;
                case '÷':
                    if(b != 0)
                        a /= b;
                    else
                    {
                        inputTextBox.Text = "Nie można dzielić przez zero";
                        current = "";
                        error = true;
                        return;
                    }
                    break;
                case ' ':
                    a = b;
                    break;
            }
            changed = false;
            computed = true;
            current += " " + b.ToString() + " = " + a.ToString();
            historyList.Add(new Result() { Text = current });
            historyLabel.Content = current;
            current = "";
            inputTextBox.Text = a.ToString();
        }
        private void AddNumber(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (!changed)
                ClearEntry(sender, e);

            if(computed)
            {
                ClearEntry(sender, e);
                oper = ' ';
                computed = false;
            }

            if (inputTextBox.Text.Length < 16)
            {
                if (Convert.ToDouble(inputTextBox.Text) == 0 && !inputTextBox.Text.Contains(separator))
                    inputTextBox.Text = "";

                if (sender.GetType() == typeof(Button))
                    inputTextBox.Text += ((Button)sender).Content;
                else
                    inputTextBox.Text += (int)sender - 34;

                b = Convert.ToDouble(inputTextBox.Text);
            }
        }
        private void Operation(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (oper == ' ')
                a = b;
            else if (changed)
                Compute(sender, e);

            if (sender.GetType() == typeof(Button))
                oper = ((Button)sender).Content.ToString()[0];
            else
                oper = (char)sender;

            if (changed)
                current = b.ToString() + " " + oper.ToString();
            else
                current = a.ToString() + " " + oper.ToString();

            historyLabel.Content = current;
            changed = false;
            computed = false;
        }
        private void RemoveNumber(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (inputTextBox.Text.Length > 0)
            {
                inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
                if (inputTextBox.Text.Length == 0)
                    ClearEntry(sender, e);
                b = Convert.ToDouble(inputTextBox.Text);
            }
        }
        private void ClearEntry(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            changed = true;

            inputTextBox.Text = "0";
            b = 0;
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            inputTextBox.Text = "0";
            a = 0;
            b = 0;
            oper = ' ';
            current = "";
            historyLabel.Content = "";
        }
        private void DecimalSep(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (!changed)
                ClearEntry(sender, e);

            if (computed)
            {
                ClearEntry(sender, e);
                oper = ' ';
                computed = false;
            }
            string temp = inputTextBox.Text + separator;
            if (Double.TryParse(temp, out double num))
                inputTextBox.Text += separator;
        }
        private void OpositeNumber(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (inputTextBox.Text[0] == '-')
                inputTextBox.Text = inputTextBox.Text.Remove(0,1);
            else
                inputTextBox.Text = inputTextBox.Text.Insert(0,"-");

            if(changed)
                b = Convert.ToDouble(inputTextBox.Text);
            else
                a = Convert.ToDouble(inputTextBox.Text);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (e.Key == Key.Back)
            {
                RemoveNumber(sender, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal || (e.Key == Key.OemComma && separator == ",") || (e.Key == Key.OemPeriod && separator == ".") )
            {
                DecimalSep(sender, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Add || ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.OemPlus))
            {
                Operation('+', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                Operation('-', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Multiply || ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.D8))
            {
                Operation('×', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Divide || e.Key == Key.OemQuestion)
            {
                Operation('÷', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Enter || e.Key == Key.OemPlus)
            {
                Compute(sender, null);
                e.Handled = true;
            }
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                AddNumber(e.Key, null);
                e.Handled = true;
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                AddNumber(e.Key - 40, null);
                e.Handled = true;
            }
        }
        private void ShowHistory(object sender, RoutedEventArgs e)
        {
            if (TheGrid.ColumnDefinitions.Count == 4)
            {
                var column = new ColumnDefinition();
                column.Width = new GridLength(4, GridUnitType.Star);
                TheGrid.ColumnDefinitions.Add(column);
                this.Width += this.Width - 16;
                this.MinWidth = 684;
                historyListBox.Visibility = Visibility.Visible;
            }
            else
            {
                var column = TheGrid.ColumnDefinitions[4];
                this.MinWidth = 350;
                this.Width = this.Width / 2 + 8;
                TheGrid.ColumnDefinitions.RemoveAt(4);
                historyListBox.Visibility = Visibility.Hidden;
            }
        }
        private void historyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (historyListBox.SelectedItem != null)
            {
                string text = ((Result)historyListBox.SelectedItem).Text;
                inputTextBox.Text = text.Substring(text.IndexOf("=") + 2);

                if(!computed || oper == ' ')
                    b = Double.Parse(text.Substring(text.IndexOf("=") + 2));
                else
                    a = Double.Parse(text.Substring(text.IndexOf("=") + 2));
                
                changed = true;
            }
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            Binding binding = new Binding("ActualWidth");
            binding.Source = historyListBox;
            binding.Converter = new CustomConverter();
            grid.SetBinding(Grid.WidthProperty, binding);
        }
        private void historyListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            historyListBox.UnselectAll();
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
