using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Ray2StringEd.WPF;

namespace Ray2StringEd
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            ChooseFileCommand = new RelayCommand(ChooseFile);
            ReadFixCommand = new RelayCommand(ReadFix);
            WriteFixCommand = new RelayCommand(WriteFix);
        }

        public ObservableCollection<FixString> FixStrings { get; set; }
        public string FixPath { get; set; }
        public FixString SelectedItem { get; set; }

        public ICommand ChooseFileCommand { get; }
        public ICommand ReadFixCommand { get; }
        public ICommand WriteFixCommand { get; }

        public void ChooseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
                FixPath = dialog.FileName;
        }

        public void ReadFix()
        {
            if (!File.Exists(FixPath)) return;
            
            FixManager fix = new FixManager(FixPath);
            FixStrings = new ObservableCollection<FixString>(fix.ReadFix());
        }

        public void WriteFix()
        {
            if (!File.Exists(FixPath)) return;

            FixManager fix = new FixManager(FixPath);
            fix.WriteFix(FixStrings);

            MessageBox.Show($"Written {FixStrings.Count} strings to {FixPath}.");
        }

        public void GoToOffset()
        {
            OffsetEntryWindow dialog = new OffsetEntryWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dialog.ShowDialog();

            if (!dialog.Result) return;

            FixString foundOffset = FixStrings.FirstOrDefault(x => x.Offset == dialog.Offset);
            if (foundOffset != null) SelectedItem = foundOffset;
        }
    }
}