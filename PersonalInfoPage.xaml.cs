using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using System;

namespace FinancialApp
{
    public partial class PersonalInfoPage : ContentPage
    {
        public PersonalInfoPage()
        {
            InitializeComponent();
            LoadUserData();

        }

        // Method to load user data from the database when the page is loaded
        private void LoadUserData()
        {
            try
            {
                // Connection string to the UserRegistrationDB database
                string connectionString = DatabaseConfig.ConnectionString;

                // Query to get the user details for the current user
                string query = "SELECT PhoneNumber, FirstName, LastName, Email FROM UsersTable WHERE PhoneNumber = @PhoneNumber";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", MainPage.CurrentUserPhoneNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PhoneNumberEntry.Text = reader["PhoneNumber"].ToString();
                                FirstNameEntry.Text = reader["FirstName"].ToString();
                                LastNameEntry.Text = reader["LastName"].ToString();
                                EmailEntry.Text = reader["Email"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
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

        // Save button click event handler to update user email and password
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Validate that email and password entries are not empty
            if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text) || string.IsNullOrEmpty(ConfirmPasswordEntry.Text))
            {
                await DisplayAlert("Validation Error", "Email and Password fields cannot be empty.", "OK");
                return;
            }

            // Validate that passwords match
            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                return;
            }

            try
            {
                // Connection string to the UserRegistrationDB database
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserRegistrationDB;Integrated Security=True";

                // Update query to update user's email and password
                string updateQuery = "UPDATE UsersTable SET Email = @Email, PasswordHash = @PasswordHash WHERE PhoneNumber = @PhoneNumber";

                // Hash the password
                string hashedPassword = HashPassword(PasswordEntry.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", MainPage.CurrentUserPhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", EmailEntry.Text);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        cmd.ExecuteNonQuery();
                    }
                }

                await DisplayAlert("Success", "Your information has been updated successfully.", "OK");

                // Navigate back to MainPage (Login Page)
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update information: {ex.Message}", "OK");
            }
        }

        // Method to hash the password using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
