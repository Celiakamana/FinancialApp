using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class TransactionAmountSendPage : ContentPage
    {
        public TransactionAmountSendPage()
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
        private async void OnByPhoneButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {
                // Navigate to the SendByPhonePage when "By Phone" is selected and amount greater than a dollar is entered
                await Navigation.PushAsync(new SendByPhonePage(amount));
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.", "OK");
            }
        }

        // Method to handle the "By QR" button click event
        private async void OnByQRButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {
                // Logic for handling QR code transaction (placeholder for future implementation)
                await DisplayAlert("QR Transaction","This button's logic will be implemented later.", "OK");
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.", "OK");
            }
        }
    }
}
