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
            await Navigation.PopAsync();
        }

        private void OnQRCodeClicked(object sender, EventArgs e)
        {
            // Handle QR code click here
        }

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Send button is clicked
            await Navigation.PushAsync(new TransactionAmountPage());
        }

        private async void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Request button is clicked
            await Navigation.PushAsync(new TransactionAmountPage());
        }

        private void OnPayBillButtonClicked(object sender, EventArgs e)
        {
            // Handle PayBill button click here
        }

        private void OnHomeIconClicked(object sender, EventArgs e)
        {
            // Navigate to the home page
        }

        private void OnTransactionIconClicked(object sender, EventArgs e)
        {
            // Handle transaction icon click
        }

        private void OnNotificationIconClicked(object sender, EventArgs e)
        {
            // Handle notification icon click
        }
    }
}