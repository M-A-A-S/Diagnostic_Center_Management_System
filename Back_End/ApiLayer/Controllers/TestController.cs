using Diagnostic_Center_Management_System_BusinessLayer;
using Diagnostic_Center_Management_System_DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diagnostic_Center_Management_System_ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Tests")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllTests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TestDTO>> GetAllTests()
        {
            List<TestDTO> TestsList = clsTest.GetAllTests();
            if (TestsList.Count == 0)
            {
                return NotFound("No Tests Found!");
            }
            return Ok(TestsList);
        }

        [HttpGet("{Id}", Name = "GetTestById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TestDTO> GetTestById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsTest Test = clsTest.Find(Id);

            if (Test == null)
            {
                return NotFound($"Test with ID {Id} not found");
            }

            TestDTO TestDTO = Test.TestDTO;

            return Ok(TestDTO);
        }

        [HttpPost(Name = "AddTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TestDTO> AddTest(TestDTO newTestDTO)
        {
            if (newTestDTO == null || string.IsNullOrEmpty(newTestDTO.Title))
            {
                return BadRequest("Invalid test data.");
            }

            clsTest Test = new clsTest(new TestDTO(newTestDTO.Id, newTestDTO.Title, newTestDTO.Cost));
            Test.Save();

            newTestDTO.Id = Test.Id;

            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetTestById", new { id = newTestDTO.Id }, newTestDTO);
        }

        [HttpPut("{Id}", Name = "UpdateTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TestDTO> UpdateTest(int Id, TestDTO updatedTestDTO)
        {
            if (Id < 1 || updatedTestDTO == null || string.IsNullOrEmpty(updatedTestDTO.Title))
            {
                return BadRequest("Invalid test data.");
            }

            clsTest Test = clsTest.Find(Id);

            if (Test == null)
            {
                return NotFound($"Test with ID {Id} not found.");
            }

            Test.Title = updatedTestDTO.Title;
            Test.Cost = updatedTestDTO.Cost;

            Test.Save();

            return Ok(Test.TestDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTest(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsTest.DeleteTest(Id))
            {
                return Ok($"Test with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Test with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
