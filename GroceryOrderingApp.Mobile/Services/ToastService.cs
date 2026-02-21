namespace GroceryOrderingApp.Mobile.Services
{
    public interface IToastService
    {
        Task ShowSuccess(string message);
        Task ShowError(string message);
        Task ShowInfo(string message);
    }

    public class ToastService : IToastService
    {
        public async Task ShowSuccess(string message)
        {
            await ShowToast(message, Colors.Green, 3000);
        }

        public async Task ShowError(string message)
        {
            await ShowToast(message, Colors.Red, 5000);
        }

        public async Task ShowInfo(string message)
        {
            await ShowToast(message, Colors.Blue, 3000);
        }

        private async Task ShowToast(string message, Color backgroundColor, int duration)
        {
            try
            {
                // Use MAUI's built-in DisplayAlert as fallback
                // In production, consider using a Toast plugin
                var mainWindow = Application.Current?.MainPage;

                if (mainWindow != null)
                {
                    var toastLabel = new Label
                    {
                        Text = message,
                        TextColor = Colors.White,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        Padding = new Thickness(10),
                        FontSize = 14
                    };

                    var toastBox = new Frame
                    {
                        BackgroundColor = backgroundColor,
                        CornerRadius = 10,
                        HasShadow = true,
                        Padding = new Thickness(0),
                        Margin = new Thickness(20, 10),
                        Content = toastLabel
                    };

                    var mainLayout = mainWindow as Layout;
                    if (mainLayout != null)
                    {
                        mainLayout.Add(toastBox, 0, 0);

                        // Auto-hide after duration
                        await Task.Delay(duration);
                        mainLayout.Remove(toastBox);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Toast Error: {ex.Message}");
            }
        }
    }
}
