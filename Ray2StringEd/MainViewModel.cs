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
            ImportCsvCommand = new RelayCommand(ImportCsv);
            ExportCsvCommand = new RelayCommand(ExportCsv);
            CopyOffsetCommand = new RelayCommand(CopyOffset);
            CopyTextCommand = new RelayCommand(CopyText);
        }

        private long _previousOffset;

        private FixManager Manager { get; set; }

        public ObservableCollection<FixString> FixStrings { get; set; }
        public string FixPath { get; set; }
        public FixString SelectedItem { get; set; }

        public ICommand ChooseFileCommand { get; }
        public ICommand ReadFixCommand { get; }
        public ICommand WriteFixCommand { get; }
        public ICommand ImportCsvCommand { get; }
        public ICommand ExportCsvCommand { get; }
        public ICommand CopyOffsetCommand { get; }
        public ICommand CopyTextCommand { get; }

        private void ChooseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog { FileName = "Fix.sna", Filter = "SNA File (*.sna)|*.sna|All Files (*.*)|*.*" };
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
                FixPath = dialog.FileName;
        }

        private void ReadFix()
        {
            if (!File.Exists(FixPath)) return;
            
            Manager = new FixManager(FixPath);
            FixStrings = new ObservableCollection<FixString>(Manager.ReadFix());
        }

        private void WriteFix()
        {
            if (Manager == null || !File.Exists(FixPath)) return;

            Manager.BackupFix();
            Manager.WriteFix(FixStrings);

            MessageBox.Show($"Written {FixStrings.Count} strings to {FixPath}.");
        }

        private void ImportCsv()
        {
            if (FixStrings == null) return;

            OpenFileDialog dialog = new OpenFileDialog { Filter = "Comma-separated values (*.csv)|*.csv" };
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
                CsvUtils.ImportCsv(FixStrings, dialog.FileName);
        }

        private void ExportCsv()
        {
            if (FixStrings == null) return;

            SaveFileDialog dialog = new SaveFileDialog { Filter = "Comma-separated values (*.csv)|*.csv" };
            dialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(dialog.FileName)) return;

            CsvUtils.ExportCsv(FixStrings, dialog.FileName);
            MessageBox.Show($"Strings successfully exported to {dialog.FileName}");
        }

        public void CopyOffset()
        {
            Clipboard.SetText(SelectedItem?.Offset.ToString("X") ?? string.Empty);
        }

        public void CopyText()
        {
            Clipboard.SetText(SelectedItem?.Text ?? string.Empty);
        }

        public void GoToOffset()
        {
            OffsetEntryWindow dialog = new OffsetEntryWindow(_previousOffset)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dialog.ShowDialog();

            if (!dialog.Result) return;

            FixString foundOffset = FixStrings.FirstOrDefault(x => x.Offset == dialog.Offset);
            if (foundOffset != null)
            {
                SelectedItem = foundOffset;
                _previousOffset = foundOffset.Offset;
            }
        }
    }
}