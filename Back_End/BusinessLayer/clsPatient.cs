using Diagnostic_Center_Management_System_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_BusinessLayer
{
    public class clsPatient
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public bool Sex { get; set; } // 0 - Male, 1 - Female 

        public PatientDTO PatientDTO
        {
            get
            {
                return new PatientDTO(this.Id, this.Name, this.DateOfBirth, this.Phone, this.Sex);
            }
        }

        public clsPatient(PatientDTO PatientDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = PatientDTO.Id;
            this.Name = PatientDTO.Name;
            this.DateOfBirth = PatientDTO.DateOfBirth;
            this.Phone = PatientDTO.Phone;
            this.Sex = PatientDTO.Sex;

            this.Mode = Mode;
        }

        private bool _AddNewPatient()
        {
            this.Id = clsPatientData.AddPatient(PatientDTO);

            return (this.Id != -1);
        }

        private bool _UpdatePatient()
        {
            return clsPatientData.UpdatePatient(PatientDTO);
        }

        public static List<PatientDTO> GetAllPatients()
        {
            return clsPatientData.GetAllPatients();
        }

        public static int GetNumberOfPatients()
        {
            return clsPatientData.GetNumberOfPatients();
        }

        public static clsPatient Find(int Id)
        {
            PatientDTO PatientDTO = clsPatientData.GetPatientById(Id);

            if (PatientDTO != null)
            {
                return new clsPatient(PatientDTO, enMode.Update);
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
                    if (_AddNewPatient())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePatient();

            }

            return false;
        }

        public static bool DeletePatient(int Id)
        {
            return clsPatientData.DeletePatient(Id);
        }
    }
}
