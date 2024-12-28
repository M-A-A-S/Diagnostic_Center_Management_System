using Diagnostic_Center_Management_System_DataAccessLayer;

namespace Diagnostic_Center_Management_System_BusinessLayer
{
    public class clsSpecialty
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Title { get; set; }

        public SpecialtyDTO SpecialtyDTO
        {
            get
            {
                return new SpecialtyDTO(this.Id, this.Title);
            }
        }

        public clsSpecialty(SpecialtyDTO SpecialtyDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = SpecialtyDTO.Id;
            this.Title = SpecialtyDTO.Title;

            this.Mode = Mode;
        }

        private bool _AddNewSpecialty()
        {
            this.Id = clsSpecialtyData.AddSpecialty(SpecialtyDTO);

            return (this.Id != -1);
        }

        private bool _UpdateSpecialty()
        {
            return clsSpecialtyData.UpdateSpecialty(SpecialtyDTO);
        }

        public static List<SpecialtyDTO> GetAllSpecialties()
        {
            return clsSpecialtyData.GetAllSpecialties();
        }

        public static clsSpecialty Find(int Id)
        {
            SpecialtyDTO SpecialtyDTO = clsSpecialtyData.GetSpecialtyById(Id);

            if (SpecialtyDTO != null)
            {
                return new clsSpecialty(SpecialtyDTO, enMode.Update);
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
                    if (_AddNewSpecialty())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateSpecialty();

            }

            return false;
        }

        public static bool DeleteSpecialty(int Id)
        {
            return clsSpecialtyData.DeleteSpecialty(Id);
        }
    }
}
