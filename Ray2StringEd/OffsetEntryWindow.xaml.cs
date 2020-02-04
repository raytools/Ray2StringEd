using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Ray2StringEd
{
    /// <summary>
    /// Interaction logic for OffsetEntryWindow.xaml
    /// </summary>
    public partial class OffsetEntryWindow : Window
    {
        public OffsetEntryWindow()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public int Offset { get; private set; }
        public bool Result { get; private set; }

        private void ClickGo(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OffsetBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int offset))
                return;

            Offset = offset;
            Result = true;

            Close();
        }
    }
}
