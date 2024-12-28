using Diagnostic_Center_Management_System_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_BusinessLayer
{

    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }

        public TestDTO TestDTO
        {
            get
            {
                return new TestDTO(this.Id, this.Title, this.Cost);
            }
        }

        public clsTest(TestDTO TestDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = TestDTO.Id;
            this.Title = TestDTO.Title;
            this.Cost = TestDTO.Cost;

            this.Mode = Mode;
        }

        private bool _AddNewTest()
        {
            this.Id = clsTestData.AddTest(TestDTO);

            return (this.Id != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(TestDTO);
        }

        public static List<TestDTO> GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public static clsTest Find(int Id)
        {
            TestDTO TestDTO = clsTestData.GetTestById(Id);

            if (TestDTO != null)
            {
                return new clsTest(TestDTO, enMode.Update);
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
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTest();

            }

            return false;
        }

        public static bool DeleteTest(int Id)
        {
            return clsTestData.DeleteTest(Id);
        }
    }
}
