using ComplexCompressor.Algorythms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Runtime.CompilerServices;
using ComplexCompressor.Model;
using ComplexCompressor.Core;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Numerics;
using ComplexCompressor.Encryptor;

namespace ComplexCompressor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel mainModel;
        private FileDetails selectedFile;

        private int selectedAlg;
        private bool encreption;

        public ObservableCollection<FileDetails> StandartFiles { get; set; }
        public ObservableCollection<FileDetails> CompressedFiles { get; set; }
        public ObservableCollection<FileDetails> StandartCompressedFiles { get; set; }
        public ObservableCollection<FileDetails> DecompressedFiles { get; set; }

        public MainViewModel()
        {
            mainModel = new MainModel();
            StandartFiles = new ObservableCollection<FileDetails>();
            CompressedFiles = new ObservableCollection<FileDetails>();
            StandartCompressedFiles = new ObservableCollection<FileDetails>();
            DecompressedFiles = new ObservableCollection<FileDetails>();
        }

        public bool Encreption
        {
            get => encreption;
            set
            {
                encreption = value;
                OnPropertyChanged(nameof(Encreption));
            }
        }

        public FileDetails SelectedFile
        {
            get => selectedFile;
            set
            {
                selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }
        public int SelectedAlg
        {
            get => selectedAlg;
            set
            {
                selectedAlg = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }
        public int WindowSize
        {
            get { return mainModel.WindowSize; }
            set
            {
                mainModel.WindowSize = value;
                OnPropertyChanged();
            }
        }
        public int BufferSize
        {
            get { return mainModel.BufferSize; }
            set
            {
                mainModel.BufferSize = value;
                OnPropertyChanged();
            }
        }
        public int WindowSizeSliderValue
        {
            get { return mainModel.WindowSizeSliderValue; }
            set
            {
                mainModel.WindowSizeSliderValue = value;
                WindowSize = value;
                OnPropertyChanged();

            }
        }
        public int BufferSizeSliderValue
        {
            get { return mainModel.BufferSizeSliderValue; }
            set
            {
                mainModel.BufferSizeSliderValue = value;
                BufferSize = value;
                OnPropertyChanged();

            }
        }
        private RelayCommand addFile;
        public RelayCommand AddFile
        {
            get
            {
                return addFile ??
                  (addFile = new RelayCommand(obj =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog();
                      openFileDialog.Filter = "Text files (*.txt)|*.txt";

                      if (openFileDialog.ShowDialog() == true)
                      {
                          StandartFiles.Add(new FileDetails
                          {
                              FilePath = openFileDialog.FileName,
                              CompressionSpeed = "",
                              CompressionType = ""
                          });
                      }
                  }));
            }
        }
        private RelayCommand compressFile;
        public RelayCommand CompressFile
        {
            get
            {
                return compressFile ??
                  (compressFile = new RelayCommand(obj =>
                  {
                      switch (selectedAlg)
                      {
                          case 0://LZ77
                              if (!Encreption)
                              {
                                  LZ77 lz77 = new LZ77();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);
                                  var compressed = lz77.Compress(text, mainModel.WindowSize, mainModel.BufferSize);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz77STNDRT";
                                  binarySave(lz77, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ77 Standart"
                                  });
                              }
                              else if (Encreption)
                              {
                                  LZ77 lz77 = new LZ77();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);

                                  //шифрование
                                  string BinaryString = FFT_Complex_Encryptor.StringToBinary(text);
                                  Complex[] encrypted = FFT_Complex_Encryptor.FourierTransform(BinaryString);

                                  var compressed = lz77.Compress(FFT_Complex_Encryptor.EncryptedToString(encrypted), mainModel.WindowSize, mainModel.BufferSize);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz77STNDRTFFT";
                                  binarySave(lz77, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ77 Standart FFT"
                                  });
                              }

                              break;
                          case 1://LZ78
                              if (!Encreption)
                              {
                                  LZ78 lz78 = new LZ78();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);
                                  var compressed = lz78.Compress(text);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz78STNDRT";
                                  binarySave(lz78, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ78 Standart"
                                  });
                              }
                              else if (Encreption)
                              {
                                  LZ78 lz78 = new LZ78();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);

                                  //шифрование
                                  string BinaryString = FFT_Complex_Encryptor.StringToBinary(text);
                                  Complex[] encrypted = FFT_Complex_Encryptor.FourierTransform(BinaryString);

                                  var compressed = lz78.Compress(FFT_Complex_Encryptor.EncryptedToString(encrypted));
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz78STNDRTFFT";
                                  binarySave(lz78, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ78 Standart FFT"
                                  });
                              }
                              break;
                          case 2://LZW
                              if (!Encreption)
                              {
                                  LZW lzw = new LZW();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);
                                  var compressed = lzw.Compress(text);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lzw";
                                  binarySave(lzw, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "lzw"
                                  });
                              }
                              else if (Encreption)
                              {
                                  LZW lzw = new LZW();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);

                                  //шифрование
                                  string BinaryString = FFT_Complex_Encryptor.StringToBinary(text);
                                  Complex[] encrypted = FFT_Complex_Encryptor.FourierTransform(BinaryString);

                                  var compressed = lzw.Compress(FFT_Complex_Encryptor.EncryptedToString(encrypted));
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lzwFFT";
                                  binarySave(lzw, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "lzw FFT"
                                  });
                              }
                              break;
                          case 3://LZ77C
                              if (!Encreption)
                              {
                                  LZ77_Complex lz77C = new LZ77_Complex();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);
                                  var compressed = lz77C.Compress(text);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz77CMPLX";
                                  binarySave(lz77C, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ77 Complex"
                                  });
                              }
                              else if (Encreption)
                              {
                                  LZ77_Complex lz77C = new LZ77_Complex();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);

                                  //шифрование
                                  string BinaryString = FFT_Complex_Encryptor.StringToBinary(text);
                                  Complex[] encrypted = FFT_Complex_Encryptor.FourierTransform(BinaryString);

                                  var compressed = lz77C.Compress(FFT_Complex_Encryptor.EncryptedToString(encrypted));
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz77CMPLXFFT";
                                  binarySave(lz77C, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ77 Complex FFT"
                                  });
                              }
                              break;
                          case 4://LZ78C
                              if (!Encreption)
                              {
                                  LZ78_Complex lz78C = new LZ78_Complex();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);
                                  var compressed = lz78C.Compress(text);
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz78CMPLX";
                                  binarySave(lz78C, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ78 Complex"
                                  });
                              }
                              else if (Encreption)
                              {
                                  LZ78_Complex lz78C = new LZ78_Complex();
                                  Stopwatch sw = Stopwatch.StartNew();
                                  string text = File.ReadAllText(SelectedFile.FilePath);

                                  //шифрование
                                  string BinaryString = FFT_Complex_Encryptor.StringToBinary(text);
                                  Complex[] encrypted = FFT_Complex_Encryptor.FourierTransform(BinaryString);

                                  var compressed = lz78C.Compress(FFT_Complex_Encryptor.EncryptedToString(encrypted));
                                  sw.Stop();
                                  string directoryPath = System.IO.Path.GetDirectoryName(SelectedFile.FilePath);
                                  string outputPath = directoryPath + $"\\Compresed_{SelectedFile.FileName}.lz78CMPLXFFT";
                                  binarySave(lz78C, outputPath);
                                  CompressedFiles.Add(new FileDetails
                                  {
                                      FilePath = outputPath,
                                      CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                      CompressionType = "LZ77 Complex FFT"
                                  });
                              }
                              break;
                      }


                  }));
            }
        }
        private RelayCommand decompressionCommand;
        public RelayCommand DecompressionCommand
        {
            get
            {
                return decompressionCommand ??
                  (decompressionCommand = new RelayCommand(obj =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog
                      {
                         // Filter = "LZ77 Files (*.lz77STNDRT;*.lz77CN);*.lz77STNDRTFFT|*.lz77STNDRT;*.lz77CN;*.lz77STNDRTFFT",
                          Title = "Open compressed File"
                      };

                      if (openFileDialog.ShowDialog() == true)
                      {

                          using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                          {
                              string extension = System.IO.Path.GetExtension(openFileDialog.FileName);
                              switch (extension)
                              {
                                  case ".lz77STNDRT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ77 Standart"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ77 lz77 = (LZ77)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz77.Decompress(lz77.Compressed);
                                          sw.Stop();
                                          File.WriteAllText(outputPath, decompressed);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;

                                  case ".lz77STNDRTFFT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ77 Standart FFT"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ77 lz77 = (LZ77)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_FFT_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz77.Decompress(lz77.Compressed);
                                          //дешифровка
                                          Complex[] b = FFT_Complex_Encryptor.StringToEncrypted(decompressed);
                                          string finalResult = FFT_Complex_Encryptor.BinaryToString(FFT_Complex_Encryptor.InverseFourierTransform(b));
                                          sw.Stop();
                                          File.WriteAllText(outputPath, finalResult);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;
                                  case ".lz78STNDRT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ78 Standart"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ78 lz78 = (LZ78)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz78.Decompress(lz78.compressed);
                                          sw.Stop();
                                          File.WriteAllText(outputPath, decompressed);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;

                                  case ".lz78STNDRTFFT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ78 Standart FFT"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ78 lz78 = (LZ78)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_FFT_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz78.Decompress(lz78.compressed);
                                          //дешифровка
                                          Complex[] b = FFT_Complex_Encryptor.StringToEncrypted(decompressed);
                                          string finalResult = FFT_Complex_Encryptor.BinaryToString(FFT_Complex_Encryptor.InverseFourierTransform(b));
                                          sw.Stop();
                                          File.WriteAllText(outputPath, finalResult);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;
                                  case ".lzw":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "lzw"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZW lzw = (LZW)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lzw.Decompress(lzw.compressed);
                                          sw.Stop();
                                          File.WriteAllText(outputPath, decompressed);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;

                                  case ".lzwFFT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "lzw FFT"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZW lzw = (LZW)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_FFT_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lzw.Decompress(lzw.compressed);
                                          //дешифровка
                                          Complex[] b = FFT_Complex_Encryptor.StringToEncrypted(decompressed);
                                          string finalResult = FFT_Complex_Encryptor.BinaryToString(FFT_Complex_Encryptor.InverseFourierTransform(b));
                                          sw.Stop();
                                          File.WriteAllText(outputPath, finalResult);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;
                                  case ".lz77CMPLX":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ77 Complex"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ77_Complex lz77C = (LZ77_Complex)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz77C.Decompress(lz77C.compressed);
                                          sw.Stop();
                                          File.WriteAllText(outputPath, decompressed);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;

                                  case ".lz77CMPLXFFT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ77 Complex FFT"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ77_Complex lz77Cfft = (LZ77_Complex)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_FFT_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz77Cfft.Decompress(lz77Cfft.compressed);
                                          //дешифровка
                                          Complex[] b = FFT_Complex_Encryptor.StringToEncrypted(decompressed);
                                          string finalResult = FFT_Complex_Encryptor.BinaryToString(FFT_Complex_Encryptor.InverseFourierTransform(b));
                                          sw.Stop();
                                          File.WriteAllText(outputPath, finalResult);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;
                                  case ".lz78CMPLX":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ78 Complex"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ78_Complex lz78C = (LZ78_Complex)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz78C.Decompress(lz78C.compressed);
                                          sw.Stop();
                                          File.WriteAllText(outputPath, decompressed);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;

                                  case ".lz78CMPLXFFT":
                                      {
                                          StandartCompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = openFileDialog.FileName,
                                              CompressionSpeed = "",
                                              CompressionType = "LZ78 Complex FFT"
                                          });
                                          BinaryFormatter formatter = new BinaryFormatter();
                                          LZ78_Complex lz78Cfft = (LZ78_Complex)formatter.Deserialize(fs);


                                          string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                          string outputPath = directoryPath + $"\\Decompresed_FFT_{System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)}.txt";

                                          Stopwatch sw = Stopwatch.StartNew();
                                          string decompressed = lz78Cfft.Decompress(lz78Cfft.compressed);
                                          //дешифровка
                                          Complex[] b = FFT_Complex_Encryptor.StringToEncrypted(decompressed);
                                          string finalResult = FFT_Complex_Encryptor.BinaryToString(FFT_Complex_Encryptor.InverseFourierTransform(b));
                                          sw.Stop();
                                          File.WriteAllText(outputPath, finalResult);
                                          DecompressedFiles.Add(new FileDetails
                                          {
                                              FilePath = outputPath,
                                              CompressionSpeed = sw.ElapsedMilliseconds.ToString(),
                                              CompressionType = ""
                                          });
                                      }
                                      break;
                              }
                          }
                      }
                  }));
            }
        }
        private void binarySave<T>(T data, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FileDetails FileDetails
        {
            get => default;
            set
            {
            }
        }

        public MainModel MainModel
        {
            get => default;
            set
            {
            }
        }

        public LZ77 LZ77
        {
            get => default;
            set
            {
            }
        }

        public RelayCommand RelayCommand
        {
            get => default;
            set
            {
            }
        }
    }
}
