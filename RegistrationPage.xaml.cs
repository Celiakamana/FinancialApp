using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;


namespace FinancialApp
{
    public partial class RegistrationPage : ContentPage
    {
        //for storing user info for other pages
        public static string CurrentUserLastname { get; set; } = string.Empty;
        public static string CurrentUserFirstname { get; set; } = string.Empty;
        public RegistrationPage()
        {
            InitializeComponent();
        }

        //Button click event
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            //Getting input values from the Entry fields
            string phoneNumber = PhoneNumberEntry.Text;
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validating that all fields are filled
            if (string.IsNullOrEmpty(phoneNumber) ||
                string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "All fields must be filled", "OK");
                return;
            }

            // Check if the passwords match
            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }
            // to Hash the password
            string hashedPassword = HashPassword(password);

            //try/catch block to handle registration exceptions/failures
            try
            {
                //Connection string to the Users database
                string connectionString = DatabaseConfig.ConnectionString;

                //query to add new user to the UsersTable
                string insertQuery = @"INSERT INTO UsersTable (PhoneNumber, FirstName, LastName, Email, PasswordHash)
                                       VALUES (@PhoneNumber, @FirstName, @LastName, @Email, @PasswordHash); ";

                //openining the connection to save data
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    //commands to save the data
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        //SQL injection Protection
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        // Executing the query
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            // Success

                            //first dynamically set user info to be used later
                            RegistrationPage.CurrentUserFirstname = firstName;
                            RegistrationPage.CurrentUserLastname = lastName;

                            await DisplayAlert("Success", "Registration Successful!", "OK");
                            await Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            // In case something goes wrong
                            await DisplayAlert("Error", "Registration Failed", "OK");
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

        // method to hash the password using SHA256
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

        // Back Button Click Event
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

}