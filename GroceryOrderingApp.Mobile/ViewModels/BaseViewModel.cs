using System.ComponentModel;
using System.Runtime.CompilerServices;
using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;
        protected IApiService _apiService;
        protected ISecureStorageService _storageService;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public BaseViewModel()
        {
            _apiService = ServiceHelper.GetService<IApiService>();
            _storageService = ServiceHelper.GetService<ISecureStorageService>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
