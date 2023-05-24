using System.Diagnostics;
using System.Reflection;

namespace TextProcessing
{
    public class Program
    {
        private static string _nameAssembly = "TextParser.dll";
        private static string _fullNameClass = "TextParser.UniqWordsCounter";
        private static string _namePrivateMethod = "PrivateGetAllUniqWordsInText";
        private static string _namePublicMethod = "PublicGetAllUniqWordsInText";
        static void Main(string[] args)
        {
            MethodInfo? PrivateGetAllUniqWords = GetMethodFromAssembly(_namePrivateMethod);
            MethodInfo? PublicGetAllUniqWords = GetMethodFromAssembly(_namePublicMethod);
            Stopwatch stopWatch = new Stopwatch();

            Console.WriteLine("Введите полный путь к текстовому файлу:");
            string path = Console.ReadLine();
            if(path.Length == 0) 
            {
                Console.WriteLine("Введена пустая строка.");
                return;
            }

            try
            {
                TxtFileReader txtFileReader = new TxtFileReader(path);
                string text = txtFileReader.ReadFile();
                stopWatch.Start();
                Dictionary<string, int> allUniqWordsCons = PrivateGetAllUniqWords?.Invoke(null, new object[] { text }) as Dictionary<string, int>;
                stopWatch.Stop();
                Console.WriteLine("Время выполнения обычного метода: {0}", stopWatch.Elapsed.ToString());
                stopWatch.Start();
                Dictionary<string, int> allUniqWordsParal = PublicGetAllUniqWords?.Invoke(null, new object[] { text }) as Dictionary<string, int>;
                stopWatch.Stop();
                Console.WriteLine("Время выполнения параллельного метода: {0}", stopWatch.Elapsed.ToString());
                TxtFileWriter txtFileWriter = new TxtFileWriter("TextStatistics.txt");
                txtFileWriter.WriteToFileStatistics(allUniqWordsCons);
                PrintInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static MethodInfo? GetMethodFromAssembly(string nameMethod)
        {
            Assembly asm = Assembly.LoadFrom(_nameAssembly);
            Type? type = asm.GetType(_fullNameClass);
            MethodInfo? getAllUniqWords = type?.GetMethod(nameMethod, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            return getAllUniqWords;
        }

        private static void PrintInfo()
        {
            Console.WriteLine("Количество всех уникальных слов подсчитано!" +
                                "\nТекстовый файл со статистикой находится в папке TextProcessing\\bin/Debug/net7.0" +
                                "\nНажмите любую кнопку для выхода");
        }
    }
}




