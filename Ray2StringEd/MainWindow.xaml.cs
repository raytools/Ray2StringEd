using System.Windows;
using System.Windows.Controls;

namespace Ray2StringEd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel = new MainViewModel();
        }

        private MainViewModel ViewModel { get; }

        private void ListViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            double workingWidth = listView.ActualWidth - 30;
            double col1 = 80;
            double col3 = 45;
            double col2 = workingWidth - col1 - col3;

            gridView.Columns[0].Width = col1;
            gridView.Columns[1].Width = col2;
            gridView.Columns[2].Width = col3;
        }

        private void GoToOffset(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToOffset();
            StringsList.ScrollIntoView(ViewModel.SelectedItem);
        }
    }
}
