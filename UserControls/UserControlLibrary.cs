using BookDatabase.Models;
using System.Collections.ObjectModel;

using System.Windows.Controls;

namespace BookDatabase
{
    public class UserControlLibrary: UserControl
    {
        public UserControlLibrary()
        {

        }

        public string DoFilter(ObservableCollection<FilterOption> list)
        {
            List<string> selected = GetSelected(list);
            return selected.Count == 0 ? "" :
                string.Join(",", selected.Select(a => $"'{a.Replace("'", "''")}'"));
        }

        private List<string> GetSelected(ObservableCollection<FilterOption> list)
        {
            return list.Where(a => a.IsSelected).Select(a => a.Name).ToList() as List<string>;
        }

    }


}
