using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LocalizationServiceWpfApp
{
    class MainWindowDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand LoadFilesCommand;
        private RelayCommand SaveFileCommand;

        private ObservableCollection<LSFile> _LSFiles;
        public ObservableCollection<LSFile> LSFiles
        {
            get { return _LSFiles; }
            set { this._LSFiles = value; OnPropertyChanged("LSFiles"); }
        }

        private LSFile _SelectedLSFile;
        public LSFile SelectedLSFile
        {
            set
            {
                try
                {
                    this._SelectedLSFile = value;
                    OnPropertyChanged("isFileSelected");
                    if (this._SelectedLSFile != null)
                    {
                        if (!this._SelectedLSFile.IsFileContentLoaded)
                        {
                            IsLoadingInProgress = true;
                            StatusLabelText = string.Format("Загрузка подстрок перевода из {0}", this._SelectedLSFile.Name);
                            Task.Run(() => this._SelectedLSFile.LoadTranslationSubstrings()).ContinueWith(_ =>
                            {
                                IsLoadingInProgress = false;
                                StatusLabelText = string.Format("Загрузка подстрок перевода {0} успешно завершена", this._SelectedLSFile.Name);
                                this.TranslationSubstrings = this._SelectedLSFile.TranslationSubstrings;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        else this.TranslationSubstrings = this._SelectedLSFile.TranslationSubstrings;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        ObservableCollection<TranslationSubstring> _translationSubstrings;
        public ObservableCollection<TranslationSubstring> TranslationSubstrings
        {
            get { return this._translationSubstrings; }
            set { this._translationSubstrings = value; OnPropertyChanged("TranslationSubstrings"); }
        }

        public bool isFileSelected
        {
            get { return this._SelectedLSFile != null; }
        }

        private bool _isLoadingInProgress;
        public bool IsLoadingInProgress
        {
            get { return this._isLoadingInProgress; }
            set { this._isLoadingInProgress = value; OnPropertyChanged("IsLoadingInProgress"); }
        }

        private bool _isFileUploadAllowed;
        public bool IsFileUploadAllowed
        {
            get { return this._isFileUploadAllowed; }
            set { this._isFileUploadAllowed = value; OnPropertyChanged("IsFileUploadAllowed"); }
        }

        private string _statusLabelText;
        public string StatusLabelText
        {
            get { return this._statusLabelText; }
            set { this._statusLabelText = value; OnPropertyChanged("StatusLabelText"); }
        }

        private double _progressBarValue;
        public double ProgressBarValue
        {
            get { return this._progressBarValue; }
            set { this._progressBarValue = value; OnPropertyChanged("ProgressBarValue"); }
        }

        public MainWindowDataContext()
        {
            this.IsLoadingInProgress = true;
            this.IsFileUploadAllowed = false;
            this.StatusLabelText = "Загрузка списка файлов";
            Task.Run(() =>
            {
                LSFiles = new ObservableCollection<LSFile>();
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["LocaliztionService"].ConnectionString);
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand("SELECT \"ID\", \"ID_LocalizationProject\", \"Name\", \"Description\" , \"DateOfChange\", \"StringsCount\", \"Version\", \"Priority\", \"ID_FolderOwner\", \"Encoding\", \"IsFolder\", \"OriginalFullText\" FROM \"Files\"", conn);
                NpgsqlDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    int ID = r.GetInt32(0);
                    int ID_LocalizationProject = r.GetInt32(1);
                    string Name = r.GetString(2);
                    string Description = r.IsDBNull(3) ? null : r.GetString(3);
                    DateTime? DateOfChange;
                    if (r.IsDBNull(4)) DateOfChange = null; else DateOfChange = r.GetDateTime(4);
                    int? StringsCount;
                    if (r.IsDBNull(5)) StringsCount = null; else StringsCount = r.GetInt32(5);
                    int? Version;
                    if (r.IsDBNull(6)) Version = null; else Version = r.GetInt32(6);
                    int? Priority;
                    if (r.IsDBNull(7)) Priority = null; else Priority = r.GetInt32(7);
                    int? ID_FolderOwner;
                    if (r.IsDBNull(8)) ID_FolderOwner = null; else ID_FolderOwner = r.GetInt32(8);
                    string LSFEncoding = r[9] == DBNull.Value ? null : r.GetString(9);
                    bool IsFolder = r.GetBoolean(10);
                    string OriginalFullText = r.IsDBNull(11) ? null : r.GetString(11);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LSFiles.Add(new LSFile(ID, ID_LocalizationProject, Name, Description, DateOfChange, StringsCount, Version, Priority, ID_FolderOwner, LSFEncoding, IsFolder, OriginalFullText));
                    });
                }
                r.Close();
                conn.Close();
            }).ContinueWith(_ =>
            {
                this.IsLoadingInProgress = false;
                this.IsFileUploadAllowed = true;
                this.StatusLabelText = "Список файлов успешно загружен";
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public RelayCommand LoadFiles
        {
            get
            {
                return LoadFilesCommand ??
                    (this.LoadFilesCommand = new RelayCommand((obj) =>
                          {
                              try
                              {

                                  Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                                  ofd.Multiselect = true;
                                  if (ofd.ShowDialog() == true)
                                  {
                                      ProgressBarValue = 0;
                                      this.IsFileUploadAllowed = false;
                                      this.StatusLabelText = "Загрузка файлов в БД";
                                      int successfulLoadCount = 0;
                                      Task.Run(() =>
                                      {

                                          for (int i = 0; i < ofd.FileNames.Length; i++)
                                          {
                                              string ext = ofd.SafeFileNames[i].Split('.').Last().ToLower();
                                              if (Regex.IsMatch(ext, Data.AllowedFileFormats))
                                              {
                                                  Application.Current.Dispatcher.Invoke(() =>
                                                  {
                                                      this.StatusLabelText = string.Format("Загружется файл {0}", ofd.FileNames[i]);
                                                  });
                                                  LSFile lsf = new LSFile(ext, 0, ofd.SafeFileNames[i], null, DateTime.Now, null, null, null, ofd.FileNames[i]);
                                                  Application.Current.Dispatcher.Invoke(() =>
                                                  {
                                                      this.LSFiles.Add(lsf);
                                                      OnPropertyChanged("LSFiles");
                                                      successfulLoadCount++;
                                                      ProgressBarValue = 100 * (i + 1) / ofd.FileNames.Length;
                                                  });
                                              }
                                          }

                                      }).ContinueWith(_ =>
                                      {
                                          this.StatusLabelText = string.Format("Загружено файлов: {0} из {1}", successfulLoadCount, ofd.FileNames.Length);
                                          this.IsFileUploadAllowed = true;
                                      }, TaskScheduler.FromCurrentSynchronizationContext());

                                  }
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                          }));
            }

        }

        public RelayCommand SaveFile
        {
            get
            {
                return SaveFileCommand ??
                          (this.SaveFileCommand = new RelayCommand((obj) =>
                          {
                              try
                              {
                                  Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                                  Match fm = Regex.Match(this._SelectedLSFile.Name, "(.*)\\.(.*)");
                                  sfd.FileName = fm.Groups[1].Value;
                                  sfd.Filter = string.Format("{0}-files (*.{0})|*.{0}", fm.Groups[2].Value);
                                  if (sfd.ShowDialog() == true)
                                  {
                                      using (StreamWriter sw = new StreamWriter(File.Open(sfd.FileName, FileMode.CreateNew), Encoding.GetEncoding(this._SelectedLSFile.LSFEncoding)))
                                      {
                                              sw.Write(this._SelectedLSFile.OriginalFullText);
                                      }
                                  }
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                          }));
            }

        }
    }
}
