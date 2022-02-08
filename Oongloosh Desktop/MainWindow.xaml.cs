using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Oongloosh_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string revertText;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ConvertText()
        {
            // Main convert text to Oongloosh method
            mainText.Text = mainText.Text.Replace("o", "oo")
                                         .Replace("O", "OO")
                                         .Replace("a", "oo")
                                         .Replace("e", "oo")
                                         .Replace("i", "oo")
                                         .Replace("u", "oo")
                                         .Replace("A", "OO")
                                         .Replace("E", "OO")
                                         .Replace("I", "OO")
                                         .Replace("U", "OO");

            // Set the caret to the end of the text after conversion
            mainText.CaretIndex = mainText.GetLineLength(0);
        }

        private void ConvertClick(object sender, RoutedEventArgs e)
        {
            ConvertText();
        }

        private void CopyTextClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(mainText.Text);
            copyTextButton.Content = "Copied!";

            // Wait for 1.5 seconds before changing the button text back to original, run on separate thread to avoid blocking UI
            new Thread(() =>
            {
                Thread.Sleep(1500);

                Dispatcher.BeginInvoke(new Action(delegate
                {
                    copyTextButton.Content = "Copy";
                }));

            }).Start();
        }

        private void RevertToOriginalTextClick(object sender, RoutedEventArgs e)
        {
            mainText.Text = revertText;
        }

        private void ClearTextClick(object sender, RoutedEventArgs e)
        {
            // Reset everything
            mainText.Text = null;
            revertText = null;
            mainTextHint.Visibility = Visibility.Visible;
        }

        private void MainTextKeyDown(object sender, KeyEventArgs e)
        {
            // Enter key shortcut to convert
            if (e.Key == Key.Enter)
                ConvertText();
        }

        private void MainTextKeyUp(object sender, KeyEventArgs e)
        {
            // When the user types something, store it so we can revert back to what they typed at a later date
            if (e.Key == Key.Enter)
                return;

            revertText = mainText.Text;
        }


        private void TopBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Click and drag to move the window, or double click to maximise/minimise
            if (e.ClickCount == 2)
            {
                if (!(WindowState == WindowState.Maximized))
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }

                return;
            }

            DragMove();
        }

        private void TopBarMouseMove(object sender, MouseEventArgs e)
        {
            // If the user tries to drag and move the window while in maximised mode, return the window state to normal first
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;

                    System.Drawing.Point mousePosition = System.Windows.Forms.Control.MousePosition;
                    Left = mousePosition.X - (Width / 2);
                    Top = mousePosition.Y - (topBar.Height.Value / 2);
                }

                DragMove();
            }
        }

        private void MainTextGotFocus(object sender, RoutedEventArgs e)
        {
            // Hide hint on focus
            mainTextHint.Visibility = Visibility.Hidden;
        }

        private void MainTextLostFocus(object sender, RoutedEventArgs e)
        {
            // Show hint on lost focus unless the textbox is not empty
            if (string.IsNullOrEmpty(mainText.Text))
                mainTextHint.Visibility = Visibility.Visible;
        }

        private void MinimiseButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximiseMinimiseButtonClick(object sender, RoutedEventArgs e)
        {
            // Toggle window state
            if (!(WindowState == WindowState.Maximized))
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindowStateChanged(object sender, EventArgs e)
        {
            // If the window is maximised change the margin on the main grid, otherwise it's too close to the edges
            if (WindowState == WindowState.Maximized)
            {
                mainGrid.Margin = new Thickness(8);

                // Hide the maximise button image and show the restore window button image
                maximiseButton.Visibility = Visibility.Collapsed;
                restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                mainGrid.Margin = new Thickness(0);

                // Show the maximise button image and hide the restore window button image
                maximiseButton.Visibility = Visibility.Visible;
                restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindowActivated(object sender, EventArgs e)
        {
            // Needed for a borderless window with custom chrome window style otherwise there are black bars
            SizeToContent = SizeToContent.Manual;
        }
    }
}
