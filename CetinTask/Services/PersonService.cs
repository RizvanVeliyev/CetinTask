using CetinTask.Models;
using System.Text.Json;

namespace CetinTask.Services
{
    public class PersonService
    {
        private readonly string jsonFilePath;

        public List<Person> Persons { get; private set; } = new List<Person>();



        public PersonService(IWebHostEnvironment env)
        {
            jsonFilePath = Path.Combine(env.WebRootPath, "persons.json");

            if (!Directory.Exists(env.WebRootPath))
            {
                Directory.CreateDirectory(env.WebRootPath);
            }

            LoadFromJson();
        }

        public void SaveToJson()
        {
            var json = JsonSerializer.Serialize(Persons, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonFilePath, json);
        }

        private void LoadFromJson()
        {
            if (File.Exists(jsonFilePath))
            {
                var json = File.ReadAllText(jsonFilePath);
                Persons = JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
            }
        }
    }
}
