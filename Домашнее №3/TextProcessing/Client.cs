using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace TextProcessing
{
    public class Client
    {
        static async Task Main(string[] args)
        {
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
                // Вызов Web-сервиса происходит в GetWords
                Dictionary<string, int> allUniqWordsWeb = GetWords(text).Result;
                TxtFileWriter txtFileWriter = new TxtFileWriter("TextStatistics.txt");
                txtFileWriter.WriteToFileStatistics(allUniqWordsWeb);
                PrintInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static async Task<Dictionary<string, int>> GetWords(string text)
        {
            StringContent content = CreateStringContent(text);
            using HttpClient client = new HttpClient();
            var responseMessage = await client.PostAsync($"https://localhost:7206/getWords/", content);
            return responseMessage.Content.ReadFromJsonAsync<Dictionary<string, int>>().Result;
        }

        private static StringContent CreateStringContent(string text)
        {
            var stringPayload = JsonConvert.SerializeObject(text);
            StringContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            return content;
        }
        private static void PrintInfo()
        {
            Console.WriteLine("Количество всех уникальных слов подсчитано!" +
                                "\nТекстовый файл со статистикой находится в папке TextProcessing\\bin/Debug/net7.0" +
                                "\nНажмите любую кнопку для выхода");
        }
    }
}




