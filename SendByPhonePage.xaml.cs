using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using System;
using System.Security.Cryptography;
using System.Text;


namespace FinancialApp
{
    public partial class SendByPhonePage : ContentPage
    {
        private decimal amount; // Store the amount to be sent from previous page

        public SendByPhonePage(decimal amount)
        {
            InitializeComponent();
            this.amount = amount; // Initialize the amount field
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
        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Retrieve user input
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string recipientphoneNumber = PhoneNumberEntry.Text;

            // Check if all fields are filled
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(recipientphoneNumber))
            {
                await DisplayAlert("Error", "All fields must be filled", "OK");
                return;
            }

            // Validate that the user is not sending money to themselves
            if (recipientphoneNumber == MainPage.CurrentUserPhoneNumber)
            {
                await DisplayAlert("Error", "You cannot send money to your own account.", "OK");
                return;
            }

            try
            {
                // Connection string to the UserRegistrationDB database
                string connectionString = DatabaseConfig.ConnectionString;

                // SQL query to verify if the provided information matches a user in the database
                string query = @"SELECT COUNT(1) FROM UsersTable WHERE FirstName = @FirstName AND LastName = @LastName AND PhoneNumber = @PhoneNumber";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Prevent SQL injection
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", recipientphoneNumber);

                        // Execute the query
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // If a match is found, navigate to SendConfirmationPage
                        if (count == 1)
                        {
                            await Navigation.PushAsync(new SendConfirmationPage(firstName, lastName, recipientphoneNumber, amount));
                        }
                        else
                        {
                            // If no match is found, show an error message
                            await DisplayAlert("Error", "User not found", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during database interaction
                await DisplayAlert("Error", $"Database error: {ex.Message}", "OK");
            }
        }

        // Found easier way: Prevent user from sending or requesting to their own account
        public bool ValidateRecipient(string recipientPhoneNumber)
        {
            string currentUserPhoneNumber = "UserPhoneNumberHere"; // will get number dynamically and set to the current user's phone number
            return recipientPhoneNumber != currentUserPhoneNumber;
        }
    }

} 