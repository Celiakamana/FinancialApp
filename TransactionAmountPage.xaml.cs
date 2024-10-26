using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class TransactionAmountPage : ContentPage
    {
        public TransactionAmountPage()
        {
            InitializeComponent();
        }

        // Method to handle the "Back" button click event
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the previous page
            await Navigation.PopAsync();
        }

        // Method to handle the "Cancel" button click event
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to TransactionsPage
            await Navigation.PushAsync(new TransactionsPage());
        }

        // Method to handle the "By Phone" button click event
        private void OnByPhoneButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling transactions by phone
            // Placeholder for future implementation
        }

        // Method to handle the "By QR" button click event
        private void OnByQRButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling transactions by QR code
            // Placeholder for future implementation
        }
    }
}
