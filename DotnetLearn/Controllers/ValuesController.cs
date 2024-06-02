using Azure;
using DotnetLearn.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotnetLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    

    public class ValuesController : ControllerBase
    {

        private readonly IMLogger _myLogger;

        public ValuesController(IMLogger myLogger)
        {
            _myLogger = myLogger;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult  <IEnumerable<StudentDTO>> GetStudents()
        {
            //var students = new List<StudentDTO>();
            //foreach(var item in CollageRepository.Students)
            //{
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id= item.Id,
            //        StudentName=item.StudentName,
            //        Address=item.Address,
            //        Email=item.Email
            //    };

            //    students.Add(obj);
            //}

            var students = CollageRepository.Students.Select(s => new StudentDTO()
            {
                Id=s.Id,
                StudentName=s.StudentName,
                Address=s.Address,
                Email=s.Email,
            });

            return Ok(students);
        }


        [HttpGet]
        [Route("{id}", Name = "getstudentbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError )]
        public ActionResult<Student>  GetStudentsByID(int id)
        {
            //BadReques -400 
            if (id <= 0)
                return BadRequest();

            var student =CollageRepository.Students.Where(n=>n.Id == id).FirstOrDefault();

            var StudentDTO = new StudentDTO
            {
                Id=student.Id,
                StudentName=student.StudentName,
                Address=student.Address,
                Email=student.Email
            };


            if (student == null)
                return NotFound($"The Student with id {id} not founds");

             return Ok (student);
        }

        [HttpGet("{name:alpha}", Name = "getstudentbyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> GetStudentsByName(string name)
        {
            return Ok (CollageRepository.Students.Where(n => n.StudentName == name ).FirstOrDefault());
        }

        [HttpDelete("{id}", Name = "deletestudentbyids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult< bool> DeleteStudent(int id)
        {
            var Students= CollageRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            CollageRepository.Students.Remove(Students);

            return Ok( true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
            if (model == null)
                return BadRequest();

            int newId = CollageRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email
            };
            CollageRepository.Students.Add(student);
            model.Id = student.Id;

            return CreatedAtRoute("getstudentbyid", new { id = model.Id }, model);
            //return Ok(student);

        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent([FromBody]StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existingStudent=CollageRepository.Students.Where(s=>s.Id==model.Id).FirstOrDefault();

             if (existingStudent == null)
                return BadRequest();

            existingStudent.StudentName = model.StudentName;
            existingStudent.Address=model.Address;
            existingStudent.Email = model.Email;
          //  return Ok(existingStudent);
            return NoContent();
        
        }



        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id  , [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existingStudent = CollageRepository.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();


            var StudentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };
            patchDocument.ApplyTo(StudentDTO, ModelState);
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName = StudentDTO.StudentName;
            existingStudent.Address = StudentDTO.Address;
            existingStudent.Email = StudentDTO.Email;
            //  return Ok(existingStudent);
            return NoContent();

        }


        [HttpGet]
        public ActionResult Index()
        {
            _myLogger.Log("Index methodds started ");
            return Ok();


        }
    }
}
