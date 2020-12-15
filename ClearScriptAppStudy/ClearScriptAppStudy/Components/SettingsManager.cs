using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClearScriptAppStudy.Components
{
    public class SettingsManager<T>
        where T : class, new()
    {
        private readonly string _filePath;

        public SettingsManager(string fileName)
        {
            _filePath = GetLocalFilePath(fileName);
        }

        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }

        public T LoadSettings() =>
            File.Exists(_filePath) ?
                (T)JsonSerializer.Deserialize(File.ReadAllText(_filePath), typeof(T))
                : new T();

        public void SaveSettings(T settings)
        {
            string json = JsonSerializer.Serialize(settings, typeof(T));
            File.WriteAllText(_filePath, json);
        }
    }
}
