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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Compute(object sender, RoutedEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

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
                operationLabel.Content = ' ';
                computed = false;
            }

            if (inputTextBox.Text.Length < inputTextBox.MaxLength)
            {
                if (Convert.ToDouble(inputTextBox.Text) == 0 && !inputTextBox.Text.Contains(","))
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

            operationLabel.Content = oper;
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
            operationLabel.Content = ' ';
        }

        private void DecimalPoint(object sender, RoutedEventArgs e)
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
                operationLabel.Content = ' ';
                computed = false;
            }

            if (inputTextBox.Text.Length < inputTextBox.MaxLength && !inputTextBox.Text.Contains(","))
                inputTextBox.Text += ",";
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
            b = Convert.ToDouble(inputTextBox.Text);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (error)
            {
                error = false;
                Clear(sender, e);
                return;
            }

            if (e.Key == Key.Enter)
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
            else if (e.Key == Key.Back)
            {
                RemoveNumber(sender, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemComma || e.Key == Key.OemPeriod)
            {
                DecimalPoint(sender, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Add)
            {
                Operation('+', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Subtract)
            {
                Operation('-', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Multiply)
            {
                Operation('×', null);
                e.Handled = true;
            }
            else if (e.Key == Key.Divide)
            {
                Operation('÷', null);
                e.Handled = true;
            }
        }
    }
}
