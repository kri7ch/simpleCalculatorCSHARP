using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace simpleСalculator
{
    public partial class MainWindow : Window
    {
        private readonly char[] signs = new char[] { '+', '-', '*', '/' };
        private const char comma = ',';

        public MainWindow()
        {
            InitializeComponent();
            tb_result.Text = "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string content = button.Content.ToString();
                if (Array.Exists(signs, element => element.ToString() == content))
                {
                    HandleSignInput(content[0]);
                }
                else if (content == comma.ToString())
                {
                    AddCharacter(comma);
                }
                else
                {
                    AddNumber(content);
                }
            }
        }

        private void AddNumber(string number)
        {
            // Если вводимое число является "-" и результат пуст, это начало отрицательного числа.
            if (number == "-" && (tb_result.Text == "0" || tb_result.Text == ""))
            {
                tb_result.Text = number; // Начинаем новое отрицательное число
            }
            else if (tb_result.Text == "0" || tb_result.Text == "")
            {
                tb_result.Text = number; // Если 0 или пустое, то просто добавляем
            }
            else
            {
                tb_result.Text += number; // Добавляем к существующему значению
            }
        }

        private void HandleSignInput(char addedSign)
        {
            if (tb_result.Text.EndsWith(comma.ToString()))
            {
                tb_result.Text = tb_result.Text.Substring(0, tb_result.Text.Length - 1);
            }

            // Обновите логику для учета предыдущего знака (например, если мы только что ввели отрицательное число)
            if (IsLastCharacterSign())
            {
                return; // если последний символ - знак, не добавляем новый
            }

            // Проверка на ввод (-)
            if (addedSign == '-' && (tb_result.Text == "0" || tb_result.Text == ""))
            {
                tb_result.Text += addedSign; // Добавляем знак минус как первый символ
            }
            else if (!IsLastCharacterSign())
            {
                tb_result.Text += addedSign; // Иначе просто добавляем знак
            }
        }

        private void AddCharacter(char character)
        {
            if (tb_result.Text.EndsWith(character.ToString()))
            {
                return;
            }

            if (IsLastCharacterSign())
            {
                return;
            }

            if (tb_result.Text.Length > 0 && char.IsDigit(tb_result.Text[tb_result.Text.Length - 1]))
            {
                int lastSignIndex = tb_result.Text.LastIndexOfAny(signs);
                string currentNumber = lastSignIndex >= 0 ? tb_result.Text.Substring(lastSignIndex + 1) : tb_result.Text;

                if (!currentNumber.Contains(character.ToString()))
                {
                    tb_result.Text += character;
                }
            }
            else if (tb_result.Text == "")
            {
                tb_result.Text = "0,";
            }
        }

        private double CalculateResult(string input)
        {
            double secondNumber = 0;
            char operatorChar = '\0';
            foreach (char sign in signs)
            {
                if (input.Contains(sign))
                {
                    operatorChar = sign;
                    break;
                }
            }

            if (operatorChar == '\0')
            {
                return 0;
            }

            string[] parts = input.Split(operatorChar);

            if (parts.Length != 2) return 0;

            MessageBox.Show(parts[0].ToString());
            double firstNumber = Convert.ToDouble(parts[0]);

            if(parts[1] == "")
            {
                secondNumber = firstNumber;
            }
            else
            {
                secondNumber = Convert.ToDouble(parts[1]);
            }

            switch (operatorChar)
            {
                case '+':
                    return firstNumber + secondNumber;
                case '-':
                    return firstNumber - secondNumber;
                case '*':
                    return firstNumber * secondNumber;
                case '/':
                    return secondNumber != 0 ? firstNumber / secondNumber : 0;
                default:
                    return 0;
            }
        }

        private void result()
        {
            string input = tb_result.Text;
            double result = CalculateResult(input);
            tb_result.Text = result.ToString();
        }

        private void btn_result_Click(object sender, RoutedEventArgs e)
        {
            result();
        }

        private bool IsLastCharacterSign()
        {
            return tb_result.Text.Length > 0 && Array.Exists(signs, element => element.ToString() == tb_result.Text[tb_result.Text.Length - 1].ToString());
        }

        private void btn_AC_Click(object sender, RoutedEventArgs e)
        {
            tb_result.Clear();
        }

        private void btn_acFull_Click(object sender, RoutedEventArgs e)
        {
            tb_result.Text = tb_result.Text.Substring(0, tb_result.Text.Length - 1);
        }

        private void tb_result_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_result.Text == "" || tb_result.ToString().Contains("E"))
            {
                tb_result.Text = "0";
            }

            if (tb_result.Text.Length >= 20)
            {
                tb_result.Text = tb_result.Text.Substring(0, tb_result.Text.Length - 1);
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste || e.Command == ApplicationCommands.Cut)
            {
                e.Handled = true;
            }
        }
        private void tb_result_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = false;
            }
            else if (e.Key == Key.Enter)
            {
                result();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }


        private void tb_result_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;

            if (Array.Exists(signs, element => element.ToString() == e.Text))
            {
                HandleSignInput(e.Text[0]);
            }
            else if (e.Text == comma.ToString())
            {
                AddCharacter(comma);
            }
            else if (int.TryParse(e.Text, out int num))
            {
                AddNumber(e.Text);
            }
        }
    }
}