using Diagnostic_Center_Management_System_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_BusinessLayer
{
    public class clsInvoice
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TestId { get; set; }

        public InvoiceDTO InvoiceDTO
        {
            get
            {
                return new InvoiceDTO(this.Id, this.PatientId, this.DoctorId, this.DeliveryDate, this.TestId);
            }
        }

        public DetailedInvoiceDTO DetailedInvoiceDTO
        {
            get
            {
                return new DetailedInvoiceDTO(this.Id, this.PatientId, this.DoctorId, this.DeliveryDate, this.TestId);
            }
        }

        public clsInvoice(InvoiceDTO InvoiceDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = InvoiceDTO.Id;
            this.PatientId = InvoiceDTO.PatientId;
            this.DoctorId = InvoiceDTO.DoctorId;
            this.DeliveryDate = InvoiceDTO.DeliveryDate;
            this.TestId = InvoiceDTO.TestId;

            this.Mode = Mode;
        }

        public clsInvoice(DetailedInvoiceDTO InvoiceDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = InvoiceDTO.Id;
            this.PatientId = InvoiceDTO.PatientId;
            this.DoctorId = InvoiceDTO.DoctorId;
            this.DeliveryDate = InvoiceDTO.DeliveryDate;
            this.TestId = InvoiceDTO.TestId;

            this.Mode = Mode;
        }

        private bool _AddNewInvoice()
        {
            this.Id = clsInvoiceData.AddInvoice(InvoiceDTO);

            return (this.Id != -1);
        }

        private bool _UpdateInvoice()
        {
            return clsInvoiceData.UpdateInvoice(InvoiceDTO);
        }

        public static List<DetailedInvoiceDTO> GetAllInvoices()
        {
            return clsInvoiceData.GetAllInvoices();
        }

        public static int GetIncome()
        {
            return clsInvoiceData.GetIncome();
        }

        public static clsInvoice Find(int Id)
        {
            DetailedInvoiceDTO InvoiceDTO = clsInvoiceData.GetInvoiceById(Id);

            if (InvoiceDTO != null)
            {
                return new clsInvoice(InvoiceDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInvoice())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateInvoice();

            }

            return false;
        }

        public static bool DeleteInvoice(int Id)
        {
            return clsInvoiceData.DeleteInvoice(Id);
        }
    }

}
