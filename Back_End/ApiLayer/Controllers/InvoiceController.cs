using Diagnostic_Center_Management_System_BusinessLayer;
using Diagnostic_Center_Management_System_DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diagnostic_Center_Management_System_ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllInvoices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DetailedInvoiceDTO>> GetAllInvoices()
        {
            List<DetailedInvoiceDTO> InvoicesList = clsInvoice.GetAllInvoices();
            if (InvoicesList.Count == 0)
            {
                return NotFound("No Invoices Found!");
            }
            return Ok(InvoicesList);
        }

        [HttpGet("Income", Name = "GetIncome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetIncome()
        {
            return Ok(clsInvoice.GetIncome());
        }

        [HttpGet("{Id}", Name = "GetInvoiceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DetailedInvoiceDTO> GetInvoiceById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsInvoice Invoice = clsInvoice.Find(Id);

            if (Invoice == null)
            {
                return NotFound($"Invoice with ID {Id} not found");
            }

            DetailedInvoiceDTO InvoiceDTO = Invoice.DetailedInvoiceDTO;

            return Ok(InvoiceDTO);
        }

        [HttpPost(Name = "AddInvoice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<InvoiceDTO> AddInvoice(InvoiceDTO newInvoiceDTO)
        {
            if (newInvoiceDTO == null || newInvoiceDTO.PatientId < 0 || newInvoiceDTO.DoctorId < 0 || newInvoiceDTO.TestId < 0)
            {
                return BadRequest("Invalid invoice data.");
            }

            clsInvoice Invoice = new clsInvoice(new InvoiceDTO(newInvoiceDTO.Id, newInvoiceDTO.PatientId, newInvoiceDTO.DoctorId, newInvoiceDTO.DeliveryDate, newInvoiceDTO.TestId));

            Invoice.Save();

            newInvoiceDTO.Id = Invoice.Id;

            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetInvoiceById", new { id = newInvoiceDTO.Id }, newInvoiceDTO);
        }

        [HttpPut("{Id}", Name = "UpdateInvoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<InvoiceDTO> UpdateInvoice(int Id, InvoiceDTO updatedInvoiceDTO)
        {
            if (Id < 1 || updatedInvoiceDTO == null || updatedInvoiceDTO.PatientId < 0 || updatedInvoiceDTO.DoctorId < 0 || updatedInvoiceDTO.TestId < 0)
            {
                return BadRequest("Invalid invoice data.");
            }

            clsInvoice Invoice = clsInvoice.Find(Id);

            if (Invoice == null)
            {
                return NotFound($"Invoice with ID {Id} not found.");
            }

            Invoice.PatientId = updatedInvoiceDTO.PatientId;
            Invoice.DoctorId = updatedInvoiceDTO.DoctorId;
            Invoice.DeliveryDate = updatedInvoiceDTO.DeliveryDate;
            Invoice.TestId = updatedInvoiceDTO.TestId;

            Invoice.Save();

            return Ok(Invoice.InvoiceDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteInvoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteInvoice(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsInvoice.DeleteInvoice(Id))
            {
                return Ok($"Invoice with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Invoice with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
