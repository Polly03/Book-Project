using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace BookDatabase.Models
{
    // class for event handle and sending event when object of this class had changed its values
    public class FilterOption : INotifyPropertyChanged
    {
        public string? Name { get; set; }

        private bool FisSelected;  
        public bool IsSelected
        {
            get => FisSelected;
            set { 
                FisSelected = value; 
                OnPropertyChanged(); 
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
