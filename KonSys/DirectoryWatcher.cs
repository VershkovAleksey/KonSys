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
            Console.WriteLine("Ждите");
            Console.SetOut(outName);
            GetDirectoryList(path);
            Console.WriteLine("\n\nОбщий размер проверяемого каталога: {0} Гб", Math.Round(TotalSize / 1024 / 1024 / 1024, 3));
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
                    
                    int i = 1;
                    foreach (DirectoryInfo directoryInfo in directoryArray)
                    {
                        Console.WriteLine("\n\nВход на {0} каталог от корня {1}",i,dirName );
                        Console.WriteLine("Каталог: " + directoryInfo.Name);
                        i++;
                        GetSizeOfFile(directoryInfo.FullName);

                    }
                    int j = 0;
                    for (j = 0; j < files.Length; j++)
                    {
                        transitionalFileSize += files[j].Length;
                        try { 
                            string mime = CheckMimeType.GetMimeFromFile(Convert.ToString(files[j]));
                            Console.WriteLine("Файл: {0} | Размер {1} Мб | Mime-Type: {2}", files[j].Name, Math.Round((Convert.ToDouble(files[j].Length)) / 1024 / 1024, 2), mime);
                            
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка чтения файла");
                        }
                        continue;
                    }
                    //foreach (FileInfo file in files)
                    //{
                    //    transitionalFileSize += file.Length;
                    //    string mime = CheckMimeType.GetMimeFromFile(Convert.ToString(file));
                    //    Console.WriteLine("Файл: {0} | Размер {1} Мб | Mime-Type: {2}", file.Name, Math.Round((Convert.ToDouble(file.Length)) / 1024 / 1024, 2), mime);
                    //}
                    i = 1;
                    
                    foreach (DirectoryInfo curDirectory in directoryArray)
                    {
                        //рекурсивно вызываем наш метод
                        GetDirectoryList(curDirectory.FullName);

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
                    try { 
                        //Записываем размер файла в байтах
                        transitionalFileSize += file.Length;
                        string mime = CheckMimeType.GetMimeFromFile(Convert.ToString(file));
                        Console.WriteLine("Файл: {0} | Размер {1} Мб | Mime-Type: {2}", file.Name,Math.Round((Convert.ToDouble(file.Length))/1024/1024,2), mime);
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка чтения файла");
                    }
                    continue;
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
