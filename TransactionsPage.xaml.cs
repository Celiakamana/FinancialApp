using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using QRCoder;
using System.IO;



namespace FinancialApp
{
    public partial class TransactionsPage : ContentPage
    {
        public TransactionsPage()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to LoginPage(MainPage
            await Navigation.PushAsync(new MainPage());
        }

        private async void OnQRCodeClicked(object sender, EventArgs e)
        {
            // Handle MYQR code
            // gather data
            string firstName = RegistrationPage.CurrentUserFirstname;
            string lastName = RegistrationPage.CurrentUserLastname;
            string recipientPhoneNumber = MainPage.CurrentUserPhoneNumber;
           //create data string
            string qrData = $"{firstName},{lastName},{recipientPhoneNumber}";

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
                        // Convert the stream to an ImageSource for .NET MAUI
                        var qrCodeImageSource = ImageSource.FromStream(() => stream);

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

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Send button is clicked
            await Navigation.PushAsync(new TransactionAmountSendPage());
        }

        private async void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Request button is clicked
            await Navigation.PushAsync(new TransactionAmountRequestPage());
        }

        private async void OnPayBillButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling Pay Bill (placeholder for future implementation)
            await DisplayAlert("Pay Bill", "This button's logic will be implemented later.", "OK");
        }

        private async void OnHomeIconClicked(object sender, EventArgs e)
        {
            // Navigate to the home page
            await Navigation.PushAsync(new HomePage());
        }

        private async  void OnTransactionIconClicked(object sender, EventArgs e)
        {
            // Handle transaction icon click
            await DisplayAlert("Recent Transactions List", "This button's logic will be implemented later.", "OK");
        }

        private async void OnNotificationIconClicked(object sender, EventArgs e)
        {
            // Handle Notifications icon click
            await DisplayAlert("Recent Notifications List", "This button's logic will be implemented later.", "OK");
        }
    }
}