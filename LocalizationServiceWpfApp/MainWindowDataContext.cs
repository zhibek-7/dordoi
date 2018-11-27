using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                        if (!this._SelectedLSFile.IsFileStringsLoaded)
                        {
                            IsLoadingInProgress = true;
                            StatusLabelText = string.Format("Загрузка строк файла из {0}", this._SelectedLSFile.Name);
                            Task.Run(() => this._SelectedLSFile.LoadStrings()).ContinueWith(_ =>
                            {
                                IsLoadingInProgress = false;
                                StatusLabelText = string.Format("Загрузка строк файла {0} успешно завершена", this._SelectedLSFile.Name);
                                LSStrings = this._SelectedLSFile.LSStrings;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        else LSStrings = this._SelectedLSFile.LSStrings;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

        private ObservableCollection<LSString> _LSStrings;
        public ObservableCollection<LSString> LSStrings
        {
            get { return _LSStrings; }
            set { this._LSStrings = value; OnPropertyChanged("LSStrings"); }
        }

        public MainWindowDataContext()
        {
            this.IsLoadingInProgress = true;
            this.IsFileUploadAllowed = false;
            this.StatusLabelText = "Загрузка списка файлов";
            Task.Run(() =>
            {
                using (db_Entities context = new db_Entities())
                {
                    this.LSFiles = new ObservableCollection<LSFile>(context.LSFile.ToList());
                }
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
                                      this.IsFileUploadAllowed = false;
                                      this.StatusLabelText = "Загрузка файлов в БД";
                                      int successfulLoadCount = 0;
                                      Task.Run(() =>
                                      {
                                          using (db_Entities context = new db_Entities())
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
                                                      LSFile lsf = new LSFile(context, ext, 0, ofd.SafeFileNames[i], null, DateTime.Now, null, null, null, ofd.FileNames[i]);
                                                      Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          this.LSFiles.Add(lsf);
                                                          OnPropertyChanged("LSFiles");
                                                          successfulLoadCount++;
                                                          ProgressBarValue = 100 * (i + 1) / ofd.FileNames.Length;
                                                      });
                                                  }
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
                                          for (int i = 0; i < this._SelectedLSFile.LSStrings.Count; i++)
                                          {
                                              sw.Write(this._SelectedLSFile.LSStrings[i].OriginalString);
                                          }
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
