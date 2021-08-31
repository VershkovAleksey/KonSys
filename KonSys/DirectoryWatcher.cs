using System;
using System.IO;
using System.Web;

namespace KonSys
{
    public class DirectoryWatcher
    {
        private double totalSize = 0;
        public double TotalSize
        {
            get
            {
                return totalSize;
            }
            set
            {
                totalSize = value;
            }
        }
        private double fileSizeInCatalog = 0;
        public double FileSizeInCatalog
        {
            get
            {
                return fileSizeInCatalog;
            }
            set
            {
                fileSizeInCatalog = value;
            }
        }
        
        public void Writer(string path)
        {
            var defaultOut = Console.Out;

            var outFileRtf = Path.Combine(Environment.CurrentDirectory, "Result.txt");
            var outFileHtml = Path.Combine(Environment.CurrentDirectory, "Result.html");
            var outName = new StreamWriter(outFileRtf);
            Console.SetOut(outName);
            GetDirectoryList(path);
            Console.WriteLine("Общий размер проверяемого каталога: {0} Гб", Math.Round(TotalSize / 1024 / 1024 / 1024, 3));
            outName.Close();
            SautinSoft.RtfToHtml converter = new SautinSoft.RtfToHtml();

            converter.OutputFormat = SautinSoft.RtfToHtml.eOutputFormat.HTML_5;
            converter.ConvertFile(outFileRtf, outFileHtml);
            Console.SetOut(defaultOut);
            Console.WriteLine("Завершено");
            Console.ReadKey();
        }
       
         void GetDirectoryList(string dirName)
        {
            
            
            //string dirName = @"G:\";
            if (Directory.Exists(dirName))
            {
                try
                {
                    
                    DirectoryInfo directory = new DirectoryInfo(dirName);
                    DirectoryInfo[] directoryArray = directory.GetDirectories();
                    FileInfo[] files = directory.GetFiles();
                    double transitionalFileSize = 0;
                    foreach (FileInfo file in files)
                    {
                        transitionalFileSize += file.Length;
                        string mime = CheckMimeType.GetMimeFromFile(Convert.ToString(file));
                        Console.WriteLine("Файл: {0} | Размер {1} Мб | Mime-Type: {2}", file.Name, Math.Round((Convert.ToDouble(file.Length)) / 1024 / 1024, 2), mime);
                    }
                    int i = 1;
                    foreach (DirectoryInfo directoryInfo in directoryArray)
                    {
                        Console.WriteLine("\nПорядковый номер каталога:" + i);
                        Console.WriteLine(directoryInfo.Name);
                        i++;
                        GetSizeOfFile(directoryInfo.FullName);

                    }
                    i = 1;
                    foreach (DirectoryInfo directoryFile in directoryArray)
                    {
                        //рекурсивно вызываем наш метод
                        GetDirectoryList(directoryFile.FullName);
                    }
                   
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine("Директория не найдена. Ошибка: " + ex.Message);
                }
                //UnauthorizedAccessException - отсутствует доступ к файлу или папке
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
                }
                //Во всех остальных случаях
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка. Обратитесь к администратору. Ошибка: " + ex.Message);
                }
                
            }
        }

        

        void GetSizeOfFile(string folder)
        {
            try
            {
                DirectoryInfo directory = new(folder);
                FileInfo[] files = directory.GetFiles();
                CheckMimeType checkMimeType = new();
                double transitionalFileSize = 0; // счетчик размера файлов в каталоге
                
                foreach (FileInfo file in files)
                {
                    //Записываем размер файла в байтах
                    transitionalFileSize += file.Length;
                    string mime = CheckMimeType.GetMimeFromFile(Convert.ToString(file));
                    Console.WriteLine("Файл: {0} | Размер {1} Мб | Mime-Type: {2}", file.Name,Math.Round((Convert.ToDouble(file.Length))/1024/1024,2), mime);

                }
                Console.WriteLine("Размер файлов в каталоге: {0} Гб", Math.Round((transitionalFileSize/1024/1024/1024),3));
                TotalSize += transitionalFileSize;
                transitionalFileSize = 0; // обнуление счетчика размера
               
            }

            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Директория не найдена. Ошибка: " + ex.Message);
            }
            //UnauthorizedAccessException - отсутствует доступ к файлу или папке
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Отсутствует доступ. Ошибка: " + ex.Message);
            }
            //Во всех остальных случаях
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка. Обратитесь к администратору. Ошибка: " + ex.Message);
            }

        }
       
    }
}
