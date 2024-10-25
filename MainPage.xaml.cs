using System;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace FinancialApp
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        // For clearing password (during navigation).
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PasswordLoginEntry.Text = string.Empty;  // Clearing the password field
        }

        // This method will be executed when the 'Register' button is clicked.
        private async void OnRegistrationButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        // This method will be executed when the 'Login' button is clicked.
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Getting input values from the Entry fields.
            string phoneNumber = PhoneNumberLoginEntry.Text;
            string password = PasswordLoginEntry.Text;

            // Validating that both fields are filled.
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both phone number and password", "OK");
                return;
            }

            // Hash the entered password to match it with the hashed password stored in DB.
            string hashedPassword = HashPassword(password);

            try
            {
                // Connection string to the database.
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserRegistrationDB;Integrated Security=True";
                // Query to check if the phone number and hashed password match an existing record.
                string query = @"SELECT COUNT(1) FROM UsersTable WHERE PhoneNumber = @PhoneNumber AND PasswordHash = @PasswordHash";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Prevent SQL injection.
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        // Execute the query.
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // If a match is found, redirect to TransactionsPage.
                        if (count == 1)
                        {
                            await Navigation.PushAsync(new TransactionsPage());
                        }
                        else
                        {
                            // If no match is found, show an error message.
                            await DisplayAlert("Error", "Invalid phone number or password", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during login (e.g., database interaction).
                await DisplayAlert("Error", $"Database error: {ex.Message}", "OK");
            }
        }

        // Method to hash the password using SHA256 (same hashing method used during registration).
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

