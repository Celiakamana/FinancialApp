using Microsoft.Maui.Controls;
using System;
using Microsoft.Data.SqlClient;

namespace FinancialApp
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        // Back Button Click Event
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Cancel Button Click Event
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransactionsPage());
        }

        // My QR Code Click Event
        private async void OnMyQRCodeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("My QR", "This button's logic will be implemented later.", "OK");
        }

        // Account & Routing Click Event
        private async void OnAccountRoutingClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Account & Routing", "This button's logic will be implemented later.", "OK");
        }

        // Add Click Event
        private async void OnAddClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Add", "This button's logic will be implemented later.", "OK");
        }

        // Withdraw Click Event
        private async void OnWithdrawClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Withdraw", "This button's logic will be implemented later.", "OK");
        }

        // Personal Click Event
        private async void OnPersonalClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Personal", "This button's logic will be implemented later.", "OK");
        }

        // Limits Click Event
        private async void OnLimitsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Limits", "This button's logic will be implemented later.", "OK");
        }

        // Documents Click Event
        private async void OnDocumentsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Documents", "This button's logic will be implemented later.", "OK");
        }

        // Link Bank Click Event
        private async void OnLinkBankClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Link Bank", "This button's logic will be implemented later.", "OK");
        }

        // Security Click Event
        private async void OnSecurityClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Security", "This button's logic will be implemented later.", "OK");
        }

        // Support Click Event
        private async void OnSupportClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Support", "This button's logic will be implemented later.", "OK");
        }

        // Request Debit Card Click Event
        private async void OnRequestDebitCardClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Request Debit Card", "This button's logic will be implemented later.", "OK");
        }

        // Sign Out Button Click Event
        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            // Navigate back to MainPage (Login Page)
            await Navigation.PushAsync(new MainPage());
        }

        // Update Balance After Transactions
       public static void UpdateBalance(decimal amount, string PhoneNumber, bool isCredit)
       {
                try
                {
                    // Connection string to the UserRegistrationDB database
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserRegistrationDB;Integrated Security=True";

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
