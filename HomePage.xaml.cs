using Microsoft.Maui.Controls;
using System;
using Microsoft.Data.SqlClient;
using QRCoder;

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
                string connectionString = DatabaseConfig.ConnectionString;

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

            // Gather the necessary data to include in the QR Code

            string recipientPhoneNumber = MainPage.CurrentUserPhoneNumber;
            int userID = MainPage.CurrentUserID;

            // Create data string including UserID and PhoneNumber
            string qrData = $"{userID},{recipientPhoneNumber}";

            // Generate QR code using QRCoder with PngByteQRCode
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                using (var pngQrCode = new PngByteQRCode(qrCodeData))
                {
                    // Get QR Code as PNG byte array
                    byte[] qrCodeAsPngByteArr = pngQrCode.GetGraphic(20);

                    // Convert the byte array to a MemoryStream
                    using (var stream = new MemoryStream(qrCodeAsPngByteArr))
                    {
                        var qrCodeImageSource = ImageSource.FromStream(() => stream);

                        // Save QR code data to the UsersTable
                        string connectionString = DatabaseConfig.ConnectionString;
                        string updateQuery = @"UPDATE UsersTable SET QRCodeData = @QRCodeData WHERE UserID = @UserID";

                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                await conn.OpenAsync();
                                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                                {
                                    cmd.Parameters.AddWithValue("@QRCodeData", qrData);
                                    cmd.Parameters.AddWithValue("@UserID", userID);
                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", $"Failed to save QR code: {ex.Message}", "OK");
                        }

                        // Display the QR code using an Image control
                        var qrCodePage = new ContentPage
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    new Button
                                    {
                                        Text = "✕",
                                        HorizontalOptions = LayoutOptions.End,
                                        VerticalOptions = LayoutOptions.Center,
                                        BackgroundColor = Colors.Purple,
                                        TextColor = Colors.White,
                                        HeightRequest = 50,
                                        WidthRequest = 50,
                                        CornerRadius = 25,
                                        FontSize = 20,
                                        Command = new Command(async () => await Navigation.PopAsync())
                                    },
                                    new Image
                                    {
                                        Source = qrCodeImageSource,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Start,
                                        HeightRequest = 300,
                                        WidthRequest = 300
                                    }
                                }
                            }
                        };

                        await Navigation.PushAsync(qrCodePage);
                    }
                }
            }
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
