using Diagnostic_Center_Management_System_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_BusinessLayer
{

    public class clsDoctor
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int SpecialityId { get; set; }
        public DateTime JoinDate { get; set; }

        public DoctorDTO DoctorDTO
        {
            get
            {
                return new DoctorDTO(this.Id, this.Name, this.DateOfBirth, this.Phone, this.Address, this.SpecialityId, this.JoinDate);
            }
        }

        public clsDoctor(DoctorDTO DoctorDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = DoctorDTO.Id;
            this.Name = DoctorDTO.Name;
            this.DateOfBirth = DoctorDTO.DateOfBirth;
            this.Phone = DoctorDTO.Phone;
            this.Address = DoctorDTO.Address;
            this.SpecialityId = DoctorDTO.SpecialityId;
            this.JoinDate = DoctorDTO.JoinDate;

            this.Mode = Mode;
        }

        private bool _AddNewDoctor()
        {
            this.Id = clsDoctorData.AddDoctor(DoctorDTO);

            return (this.Id != -1);
        }

        private bool _UpdateDoctor()
        {
            return clsDoctorData.UpdateDoctor(DoctorDTO);
        }

        public static List<DoctorDTO> GetAllDoctors()
        {
            return clsDoctorData.GetAllDoctors();
        }

        public static int GetNumberOfDoctors()
        {
            return clsDoctorData.GetNumberOfDoctors();
        }

        public static clsDoctor Find(int Id)
        {
            DoctorDTO DoctorDTO = clsDoctorData.GetDoctorById(Id);

            if (DoctorDTO != null)
            {
                return new clsDoctor(DoctorDTO, enMode.Update);
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
                    if (_AddNewDoctor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDoctor();

            }

            return false;
        }

        public static bool DeleteDoctor(int Id)
        {
            return clsDoctorData.DeleteDoctor(Id);
        }
    }

}
