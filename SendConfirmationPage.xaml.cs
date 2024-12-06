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
        private string recipientPhoneNumber;

        public SendConfirmationPage(string firstName, string lastName, string recipientPhoneNumber, decimal amount)
        {
            InitializeComponent();

            // Initializing the fields
            this.firstName = firstName;
            this.lastName = lastName;
            this.amount = amount;
            this.recipientPhoneNumber = recipientPhoneNumber;

            // Display user initials and amount to send
            UserInitialsLabel.Text = GetInitials(firstName, lastName);
            UserNameLabel.Text = $"{firstName} {lastName}";
            DispalyAmountLabel.Text = $"$ {amount:F2}";
           
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
            try
            {
                // Update the sender's balance (debit)
                UpdateBalance(amount, MainPage.CurrentUserPhoneNumber, isCredit: false);

                // Update the recipient's balance (credit)
                UpdateBalance(amount, recipientPhoneNumber, isCredit: true);

                // Proceed with sending money
                await DisplayAlert("Success", $"You have successfully sent ${amount} to {firstName} {lastName}", "OK");
                await Navigation.PushAsync(new TransactionsPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to complete the transaction: {ex.Message}", "OK");
            }
        }

        //To update balance after transction
        public static void UpdateBalance(decimal amount, string PhoneNumber, bool isCredit)
        {
            try
            {
                // Connection string to the UserRegistrationDB database
                string connectionString = DatabaseConfig.ConnectionString;

                // Update query to increment or decrement the user's balance
                string updateQuery = @"UPDATE UsersTable SET Balance = Balance + @Amount WHERE PhoneNumber = @PhoneNumber";
                if (!isCredit)
                {
                    updateQuery = @"UPDATE UsersTable SET Balance = Balance - @Amount WHERE PhoneNumber = @PhoneNumber";
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        cmd.Parameters.AddWithValue("@Amount", amount);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error 
                Console.WriteLine($"Error updating balance: {ex.Message}");
            }
        }
    }
}

   