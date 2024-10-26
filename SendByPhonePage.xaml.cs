// SendByPhonePage.xaml.cs
using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class SendByPhonePage : ContentPage
    {
        public SendByPhonePage()
        {
            InitializeComponent();
        }

        // Method to handle the Back button click event
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the previous page
            await Navigation.PopAsync();
        }

        // Method to handle the Cancel button click event
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to TransactionsPage
            await Navigation.PushAsync(new TransactionsPage());
        }

        // Method to handle the Send button click event
        private void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling send action by phone
            // Placeholder for future implementation
        }
    }
}

