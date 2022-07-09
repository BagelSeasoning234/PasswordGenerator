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

namespace PasswordGenerator4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        
        PasswordGenerator PasswordGen;

        EPasswordType TypeOfPasswordToGen = EPasswordType.All;

        public MainWindow()
        {
            InitializeComponent();

            PasswordGen = new PasswordGenerator();
        }

        private void Generate12Chars(object sender, RoutedEventArgs e)
        {
            initializeGeneration(12);
        }

        private void Generate16Chars(object sender, RoutedEventArgs e)
        {
            initializeGeneration(16);
        }

        private void Generate20Chars(object sender, RoutedEventArgs e)
        {
            initializeGeneration(20);
        }

        /// <summary>
        /// Initializes the password generation process.
        /// </summary>
        /// <param name="length">The character length of the password to generate.</param>
        private void initializeGeneration(int length)
        {
            PasswordField.Text = PasswordGen.GeneratePassword(length, TypeOfPasswordToGen);
            PasswordLength.Text = length.ToString();
        }


        /// <summary>
        /// Generates a password with a user specified length.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateTargetLength(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordLength.Text))
                return;

            int length;

            // Makes sure we're dealing with a valid number, 

            if (!int.TryParse(PasswordLength.Text, out length))
                return;

            if (length == 0)
                return;

            if (length < 0)
                return;

            if (length > 32)
            {
                length = 32;
                PasswordLength.Text = length.ToString();
            }

            PasswordField.Text = PasswordGen.GeneratePassword(length, TypeOfPasswordToGen);
        }

        /// <summary>
        /// Updates the password type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePasswordType(object sender, SelectionChangedEventArgs e)
        {
            switch (PasswordTypeBox.SelectedIndex)
            {
                case 0:
                    TypeOfPasswordToGen = EPasswordType.Lowercase;
                    return;
                case 1:
                    TypeOfPasswordToGen = EPasswordType.Uppercase;
                    return;
                case 2:
                    TypeOfPasswordToGen = EPasswordType.Numbers;
                    return;
                case 3:
                    TypeOfPasswordToGen = EPasswordType.AvoidAmbiguous;
                    return;
                case 4:
                    TypeOfPasswordToGen = EPasswordType.AvoidSymbols;
                    return;
                case 5:
                    TypeOfPasswordToGen = EPasswordType.AvoidAmbiguousAndSymbols;
                    return;
                case 6:
                    TypeOfPasswordToGen = EPasswordType.All;
                    return;
            }
        }

        /// <summary>
        /// Copies the given password to the clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyPasswordToClipboard(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordField.Text))
                Clipboard.SetText(PasswordField.Text);
        }

        // Make sure that only numbers are added to the password length field.

        private void PreviewLengthUpdated(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
            
        }

        private void CheckIsNumeric(TextCompositionEventArgs e)
        {
            int result;

            if (!int.TryParse(e.Text, out result))
            {
                e.Handled = true;
            }
        }

        private void PreviewLengthKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
