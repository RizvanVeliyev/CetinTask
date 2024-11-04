using CetinTask.Models;
using CetinTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CetinTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {

        private static List<Person> Persons = new List<Person>
        {
                new Person { Id = 1, Name = "John Doe", LastName = "Software Engineer", Age = 30 },
                new Person { Id = 2, Name = "Jane Smith", LastName = "Data Scientist", Age = 27 }
        };

        private readonly string xmlFilePath;
        private readonly IWebHostEnvironment _env;
        //private readonly PersonService _personService;

        //public PersonsController(PersonService personService)
        //{
        //    _personService = personService;
        //}

        public PersonsController(IWebHostEnvironment env)
        {
            _env = env;
            xmlFilePath = Path.Combine(_env.WebRootPath, "persons.xml");

            if (!Directory.Exists(_env.WebRootPath))
            {
                Directory.CreateDirectory(_env.WebRootPath);
            }

            LoadFromXml();
        }
        private void SaveToXml()
        {
            var serializer = new XmlSerializer(typeof(List<Person>));

            using (var writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, Persons);
            }
        }

        private void LoadFromXml()
        {
            if (!System.IO.File.Exists(xmlFilePath))
            {
                return;
            }

            var serializer = new XmlSerializer(typeof(List<Person>));

            using (var reader = new StreamReader(xmlFilePath))
            {
                Persons = (List<Person>)serializer.Deserialize(reader);
            }
        }

        [HttpGet]
        public ActionResult<List<Person>> GetPersons()
        {
            return Ok(Persons);
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPerson(int id)
        {
            var person = Persons.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        public ActionResult<Person> CreatePerson(Person person)
        {
            Persons.Add(person);
            SaveToXml();
            //_personService.SaveToJson();
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, Person updatedPerson)
        {
            var person = Persons.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            person.Name = updatedPerson.Name;
            person.LastName = updatedPerson.LastName;
            person.Age = updatedPerson.Age;
            SaveToXml();
            //_personService.SaveToJson();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            var person = Persons.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            Persons.Remove(person);
            SaveToXml();
            //_personService.SaveToJson();
            return NoContent();
        }
    }
}
