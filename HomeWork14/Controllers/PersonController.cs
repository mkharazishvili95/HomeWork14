using HomeWork14.Models;
using HomeWork14.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HomeWork14.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private static List<Person> _persons;
        private static string _filePath;
        public PersonController()
        {
            _persons = new List<Person>();
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "persons.json");
            if(System.IO.File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
            {
                var json = System.IO.File.ReadAllText(_filePath);
                _persons = JsonSerializer.Deserialize<List<Person>>(json);
                
            }
        }
        [HttpPost("CreatePerson")]
        public ActionResult <Person> CreatePerson(Person person)
        {
            var validator = new PersonValidator();
            var validatorResult = validator.Validate(person);
            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            else if(_persons.Any(x => x.Id == person.Id)) 
            {
                return BadRequest("Person with the same ID already exists!");
            }
            else
            {
                _persons.Add(person);
                SavePersonsToFile();
                return Ok("Person successfully created!");
            }
        }
        [HttpGet("GetByID")]
        public ActionResult <Person> GetByID(int id)
        {
            var person = _persons.FirstOrDefault(x => x.Id == id);
            if(person == null)
            {
                return NotFound("Person not found by this ID!");
            }
            else
            {
                return Ok(person);
            }
        }
        [HttpGet("GetAllPerson")]
        public ActionResult<IEnumerable<Person>> GetAllPerson()
        {
            var person = new List<Person>();
            if(person == null)
            {
                return NotFound("There is no any person in the database!");
            }
            else
            {
                return Ok(_persons);
            }
        }
        [HttpGet("FilterByCountry")]
        public ActionResult<IEnumerable<Person>> FilterByCountry(string Country)
        {
            var filter = _persons.Where(x => x.PersonAddress.Country.ToUpper() == Country.ToUpper());
            if(filter == null)
            {
                return NotFound("There is no any person by this country!");
            }
            else
            {
                return Ok(filter);
            }
        }
        [HttpPut("UpdatePerson")]
        public IActionResult UpdatePerson(Person updatedPerson, int id)
        {
            var person = _persons.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                return NotFound("There is no any person by this ID!");
            }
            else
            {
                var validator = new PersonValidator();
                var validatorResult = validator.Validate(updatedPerson);
                if (!validatorResult.IsValid)
                {
                    return BadRequest(validatorResult.Errors);
                }
                else
                {
                    person.FirstName = updatedPerson.FirstName;
                    person.LastName = updatedPerson.LastName;
                    person.Salary = updatedPerson.Salary;
                    person.JobPosition = updatedPerson.JobPosition;
                    person.WorkExperience = updatedPerson.WorkExperience;
                    person.PersonAddress = updatedPerson.PersonAddress;
                    person.Email = updatedPerson.Email;
                    
                    
                    SavePersonsToFile();
                    return Ok("Person Has Updated!");
                }
            }
        }

        [HttpDelete("DeletePerson")]
        public IActionResult DeletePerson(int id)
        {
            var person = _persons.FirstOrDefault(x => x.Id == id);
            if(person == null)
            {
                return NotFound("Person not found by this ID!");
            }
            else
            {
                _persons.Remove(person);
                SavePersonsToFile();
                return Ok("Person has deleted!");
            }
        }
            private void SavePersonsToFile()
        {
            var json = JsonSerializer.Serialize(_persons);
            System.IO.File.WriteAllText(_filePath, json);
        }
    }
}
