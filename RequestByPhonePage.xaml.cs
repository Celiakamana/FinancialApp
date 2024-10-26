// RequestByPhonePage.xaml.cs
using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class RequestByPhonePage : ContentPage
    {
        public RequestByPhonePage()
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

        // Method to handle the Request button click event
        private void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling request action by phone
            // Placeholder for future implementation
        }
    }
}


