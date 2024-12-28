using Diagnostic_Center_Management_System_BusinessLayer;
using Diagnostic_Center_Management_System_DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diagnostic_Center_Management_System_ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Specialties")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllSpecialties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SpecialtyDTO>> GetAllSpecialties()
        {
            List<SpecialtyDTO> SpecialtiesList = clsSpecialty.GetAllSpecialties();
            if (SpecialtiesList.Count == 0)
            {
                return NotFound("No Specialties Found!");
            }
            return Ok(SpecialtiesList);
        }

        [HttpGet("{Id}", Name = "GetSpecialtyById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SpecialtyDTO> GetSpecialtyById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsSpecialty Specialty = clsSpecialty.Find(Id);

            if (Specialty == null)
            {
                return NotFound($"Specialty with ID {Id} not found");
            }

            SpecialtyDTO SpecialtyDTO = Specialty.SpecialtyDTO;

            return Ok(SpecialtyDTO);
        }

        [HttpPost(Name = "AddSpecialty")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<SpecialtyDTO> AddSpecialty(SpecialtyDTO newSpecialtyDTO)
        {
            if (newSpecialtyDTO == null || string.IsNullOrEmpty(newSpecialtyDTO.Title))
            {
                return BadRequest("Invalid specialty data.");
            }

            clsSpecialty Specialty = new clsSpecialty(new SpecialtyDTO(newSpecialtyDTO.Id, newSpecialtyDTO.Title));
            Specialty.Save();

            newSpecialtyDTO.Id = Specialty.Id;

            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetSpecialtyById", new { id = newSpecialtyDTO.Id }, newSpecialtyDTO);
        }

        [HttpPut("{Id}", Name = "UpdateSpecialty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SpecialtyDTO> UpdateSpecialty(int Id, SpecialtyDTO updatedSpecialtyDTO)
        {
            if (Id < 1 || updatedSpecialtyDTO == null || string.IsNullOrEmpty(updatedSpecialtyDTO.Title))
            {
                return BadRequest("Invalid specialty data.");
            }

            clsSpecialty Specialty = clsSpecialty.Find(Id);

            if (Specialty == null)
            {
                return NotFound($"Specialty with ID {Id} not found.");
            }

            Specialty.Title = updatedSpecialtyDTO.Title;
            Specialty.Save();

            return Ok(Specialty.SpecialtyDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteSpecialty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteSpecialty(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsSpecialty.DeleteSpecialty(Id))
            {
                return Ok($"Specialty with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Specialty with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
