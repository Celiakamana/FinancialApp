using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class TransactionsPage : ContentPage
    {
        public TransactionsPage()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to LoginPage(MainPage
            await Navigation.PushAsync(new MainPage());
        }

        private async void OnQRCodeClicked(object sender, EventArgs e)
        {
            // Handle QR code click here
            await DisplayAlert("My QR Code", "This button's logic will be implemented later.", "OK");
        }

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Send button is clicked
            await Navigation.PushAsync(new TransactionAmountSendPage());
        }

        private async void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Request button is clicked
            await Navigation.PushAsync(new TransactionAmountRequestPage());
        }

        private async void OnPayBillButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling Pay Bill (placeholder for future implementation)
            await DisplayAlert("Pay Bill", "This button's logic will be implemented later.", "OK");
        }

        private async void OnHomeIconClicked(object sender, EventArgs e)
        {
            // Navigate to the home page
            await Navigation.PushAsync(new HomePage());
        }

        private async  void OnTransactionIconClicked(object sender, EventArgs e)
        {
            // Handle transaction icon click
            await DisplayAlert("Recent Transactions List", "This button's logic will be implemented later.", "OK");
        }

        private async void OnNotificationIconClicked(object sender, EventArgs e)
        {
            // Handle Notifications icon click
            await DisplayAlert("Recent Notifications List", "This button's logic will be implemented later.", "OK");
        }
    }
}