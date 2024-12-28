using Diagnostic_Center_Management_System_BusinessLayer;
using Diagnostic_Center_Management_System_DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diagnostic_Center_Management_System_ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DoctorDTO>> GetAllDoctors()
        {
            List<DoctorDTO> DoctorsList = clsDoctor.GetAllDoctors();
            if (DoctorsList.Count == 0)
            {
                return NotFound("No Doctors Found!");
            }
            return Ok(DoctorsList);
        }

        [HttpGet("NumberOfDoctors", Name = "GetNumberOfDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetNumberOfDoctors()
        {
            return Ok(clsDoctor.GetNumberOfDoctors());
        }

        [HttpGet("{Id}", Name = "GetDoctorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DoctorDTO> GetDoctorById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsDoctor Doctor = clsDoctor.Find(Id);

            if (Doctor == null)
            {
                return NotFound($"Doctor with ID {Id} not found");
            }

            DoctorDTO DoctorDTO = Doctor.DoctorDTO;

            return Ok(DoctorDTO);
        }

        [HttpPost(Name = "AddDoctor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DoctorDTO> AddDoctor(DoctorDTO newDoctorDTO)
        {
            if (newDoctorDTO == null || string.IsNullOrEmpty(newDoctorDTO.Name) || string.IsNullOrEmpty(newDoctorDTO.Phone) || string.IsNullOrEmpty(newDoctorDTO.Address) || newDoctorDTO.SpecialityId < 1)
            {
                return BadRequest("Invalid doctor data.");
            }

            clsDoctor Doctor = new clsDoctor(new DoctorDTO(newDoctorDTO.Id, newDoctorDTO.Name, newDoctorDTO.DateOfBirth, newDoctorDTO.Phone, newDoctorDTO.Address, newDoctorDTO.SpecialityId, newDoctorDTO.JoinDate));
            Doctor.Save();

            newDoctorDTO.Id = Doctor.Id;

            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetDoctorById", new { id = newDoctorDTO.Id }, newDoctorDTO);
        }

        [HttpPut("{Id}", Name = "UpdateDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DoctorDTO> UpdateDoctor(int Id, DoctorDTO updatedDoctorDTO)
        {
            if (updatedDoctorDTO.Id < 1 || updatedDoctorDTO == null || string.IsNullOrEmpty(updatedDoctorDTO.Name) || string.IsNullOrEmpty(updatedDoctorDTO.Phone) || string.IsNullOrEmpty(updatedDoctorDTO.Address) || updatedDoctorDTO.SpecialityId < 1)
            {
                return BadRequest("Invalid doctor data.");
            }

            clsDoctor Doctor = clsDoctor.Find(Id);

            if (Doctor == null)
            {
                return NotFound($"Doctor with ID {Id} not found.");
            }

            Doctor.Name = updatedDoctorDTO.Name;
            Doctor.DateOfBirth = updatedDoctorDTO.DateOfBirth;
            Doctor.Phone = updatedDoctorDTO.Phone;
            Doctor.Address = updatedDoctorDTO.Address;
            Doctor.SpecialityId = updatedDoctorDTO.SpecialityId;
            Doctor.JoinDate = updatedDoctorDTO.JoinDate;

            Doctor.Save();

            return Ok(Doctor.DoctorDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteDoctor(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsDoctor.DeleteDoctor(Id))
            {
                return Ok($"Doctor with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Doctor with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
