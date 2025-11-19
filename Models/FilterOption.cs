using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace BookDatabase.Models
{
    // class for event handle and sending event when object of this class had changed its values
    public class FilterOption : INotifyPropertyChanged
    {
        public string? Name { get; set; }

        private bool _isSelected;  //JR: mela jsem dojem, ze jsem uz nekde videla pouziti prefixu "f" pro privatni promenne, bylo by fajn to mit vsude sjednocenne a nemit nekde novy a nekde stary styl... je to maly projekt. 
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
