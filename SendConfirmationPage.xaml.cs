using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;


namespace FinancialApp
{
    public partial class SendConfirmationPage : ContentPage
    {
        private string firstName;
        private string lastName;
        private decimal amount;

        public SendConfirmationPage(string firstName, string lastName, decimal amount)
        {
            InitializeComponent();

            // Initializing the fields
            this.firstName = firstName;
            this.lastName = lastName;
            this.amount = amount;


            // Display user initials and amount to send
            UserInitialsLabel.Text = GetInitials(firstName, lastName);
            UserNameLabel.Text = $"{firstName} {lastName}";
            DispalyAmountLabel.Text = $"$ {amount}";
        }

        // Get initials from the user's first and last names
        private string GetInitials(string firstName, string lastName)
        {
            return $"{firstName[0]}{lastName[0]}";
        }

        // Back button event handler
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Cancel button event handler
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransactionsPage());
        }

        // Send button click event handler
        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Proceed with sending money
            await DisplayAlert("Success", $"You have successfully sent ${amount} to {firstName} {lastName}", "OK");
            await Navigation.PushAsync(new TransactionsPage());
        }
    }
}

   