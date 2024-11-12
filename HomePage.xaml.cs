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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchAndDisplayBalance();
        }

        // Fetch the balance from the database and update the BalanceLabel
        private void FetchAndDisplayBalance()
        {
            try
            {
                // Connection string to the UserRegistrationDB database
                string connectionString = "Data Source=personal\\SQLEXPRESS;Initial Catalog=UserRegistrationDB;Integrated Security=True;Trust Server Certificate=True";

                // Query to get the balance of the current user
                string query = @"SELECT Balance FROM UsersTable WHERE PhoneNumber = @PhoneNumber";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", MainPage.CurrentUserPhoneNumber);

                        var balance = cmd.ExecuteScalar();
                        if (balance != null)
                        {
                            BalanceLabel.Text = $"${Convert.ToDecimal(balance):F2}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching balance: {ex.Message}");
            }
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
            await Navigation.PushAsync(new PersonalInfoPage());
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
       
    }
}
