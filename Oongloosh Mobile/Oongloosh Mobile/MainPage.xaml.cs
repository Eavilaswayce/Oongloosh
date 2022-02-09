using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Oongloosh_Mobile
{
    public partial class MainPage : ContentPage
    {
        string revertText = null;
        bool updateText = true;

        public MainPage()
        {
            InitializeComponent();
        }

        public void ConvertText()
        {
            if (string.IsNullOrEmpty(mainText.Text))
                return;

            updateText = false;

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
        }

        private void ConvertClick(object sender, EventArgs e)
        {
            ConvertText();
        }

        private void CopyTextClick(object sender, EventArgs e)
        {
            Clipboard.SetTextAsync(mainText.Text);
            copyTextButton.Text = "Copied!";

            // Wait for 1.5 seconds before changing the button text back to original, run on separate thread to avoid blocking UI
            new Thread(() =>
            {
                Thread.Sleep(1500);

                Dispatcher.BeginInvokeOnMainThread(new Action(delegate
                {
                    copyTextButton.Text = "Copy";
                }));

            }).Start();
        }

        private void RevertToOriginalTextClick(object sender, EventArgs e)
        {
            // Revert back to original text that the user typed
            mainText.Text = revertText;
        }

        private void ClearTextClick(object sender, EventArgs e)
        {
            // Reset everything
            mainText.Text = null;
            revertText = null;
            mainTextHint.IsVisible = true;
        }

        private void MainTextTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!updateText)
            {
                updateText = true;
                return;
            }

            revertText = mainText.Text;
        }

        private void MainTextFocused(object sender, FocusEventArgs e)
        {
            // Hide main text hint when user selects textbox
            mainTextHint.IsVisible = false;
        }

        private void MainTextUnfocused(object sender, FocusEventArgs e)
        {
            // Show main text hint unless there is text inside of the main text textbox
            if (string.IsNullOrEmpty(mainText.Text))
                mainTextHint.IsVisible = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(100);

            mainText.Unfocus();
        }
    }
}