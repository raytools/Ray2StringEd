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
        public OffsetEntryWindow(long previousOffset)
        {
            InitializeComponent();
            Loaded += (sender, e) => FocusTextBox();

            OffsetBox.Text = previousOffset > 0 ? previousOffset.ToString("X") : string.Empty;
        }

        public long Offset { get; private set; }
        public bool Result { get; private set; }

        private void ClickGo(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(OffsetBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int offset))
            {
                Offset = offset;
                Result = true;

                Close();
            }
            else FocusTextBox();
        }

        private void FocusTextBox()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            OffsetBox.SelectAll();
        }
    }
}
