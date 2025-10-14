using CommunityToolkit.Mvvm.ComponentModel;

namespace SneakersShop.Helpers
{
    public class BindableField<T> : ObservableObject
    {
        private T _value = default!;
        public T Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(Error));
                ValueChanged?.Invoke(value);
            }
        }

        private string? _error;
        public string? Error
        {
            get => _error;
            set
            {
                SetProperty(ref _error, value);
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrWhiteSpace(Error);

        public event Action<T>? ValueChanged;
    }
}
