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

        private void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Handle Send button click here
        }

        private void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Handle Request button click here
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