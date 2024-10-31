using Microsoft.Maui.Controls;

namespace FinancialApp
{
    public partial class TransactionAmountSendPage : ContentPage
    {
        public TransactionAmountSendPage()
        {
            InitializeComponent();
            AmountEntry.TextChanged += OnAmountEntryTextChanged;
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
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }

        // Method to handle the "By QR" button click event
        private async void OnByQRButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {
              

                // Logic for handling QR code transaction (placeholder for future implementation)
                await DisplayAlert("QR Transaction", "This button's logic will be implemented later.", "OK");
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }

        //Method to handle "By USSD" button click event
        private async void OnByUSSDButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {


                // Logic for handling QR code transaction (placeholder for future implementation)
                await DisplayAlert("USSD Transaction", "This button's logic will be implemented later.", "OK");
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }
        // Event handler to format the AmountEntry with two decimal places
        private void OnAmountEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            //just digits
            if (decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                string currentText = e.NewTextValue;

                // Remove any non-digit characters
                currentText = string.Concat(currentText.Where(char.IsDigit));

                // If the field is not empty, treat it as a fixed decimal with two decimal places
                if (currentText.Length > 0)
                {
                    // Format as two decimal places, treating the input as a whole number in cents
                    decimal value = decimal.Parse(currentText) / 100;
                    AmountEntry.Text = value.ToString("0.00");
                    AmountEntry.CursorPosition = AmountEntry.Text.Length;
                }
                else
                {
                    // If field is empty, reset to zero with two decimal places
                    AmountEntry.Text = "0.00";
                    AmountEntry.CursorPosition = AmountEntry.Text.Length;
                }
            }

            // Re-add the event handler
            AmountEntry.TextChanged += OnAmountEntryTextChanged;


            
        }
    }
}
