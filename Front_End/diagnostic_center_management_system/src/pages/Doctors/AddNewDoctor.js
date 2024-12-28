import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import DoctorFormInputs from "./DoctorFormInputs";
import useFetch from "../../hooks/useFetch";
import { handleCreate } from "../../utils/util";
import Alert from "../../components/Alert";

function AddNewDoctor() {
  const navigate = useNavigate();

  const { data: specialties } = useFetch("/api/Specialties/All");

  const [specialty, setSpecialty] = useState("");

  const [doctorData, setDoctorData] = useState({
    name: "",
    dateOfBirth: "",
    phone: "",
    address: "",
    joinDate: "",
    specialityId: 0,
  });

  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const getSpecialtyId = (specialty) => {
    let id = 0;
    specialties &&
      specialties.forEach((s) => {
        if (s.title == specialty) {
          id = s.id;
        }
      });

    return id;
  };

  const handleSpecialtyChange = (event) => {
    setSpecialty(event.target.value);
    setDoctorData({
      ...doctorData,
      specialityId: getSpecialtyId(event.target.value),
    });
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/doctors");
  };

  const AddNewDoctor = async () => {
    let result = await handleCreate("/api/Doctors/", doctorData);

    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const onChange = (event) => {
    const { name, value } = event.target;
    setDoctorData({ ...doctorData, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    const validationErrors = {};

    if (!doctorData.name.trim()) {
      validationErrors.name = "Name is required";
    }

    if (!doctorData.dateOfBirth) {
      validationErrors.dateOfBirth = "Date of birth is required";
    }

    if (!doctorData.phone.trim()) {
      validationErrors.phone = "Phone is required";
    } else if (isNaN(doctorData.phone.trim())) {
      validationErrors.phone = "Phone is not valid";
    } else if (doctorData.phone.trim().length !== 10) {
      validationErrors.phone = "Phone should be 10 digits";
    }

    if (!doctorData.address.trim()) {
      validationErrors.address = "Address is required";
    }

    if (!doctorData.joinDate) {
      validationErrors.joinDate = "Date of birth is required";
    }

    if (!specialty) {
      validationErrors.specialty = "Specialty is required";
    }

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      AddNewDoctor();
    }
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <h4
        style={{
          marginBottom: "1rem",
          textAlign: "center",
          color: "var(--primary-500)",
          fontWeight: "bold",
        }}
      >
        Add New Doctor
      </h4>

      <DoctorFormInputs
        doctorData={doctorData}
        onChange={onChange}
        errors={errors}
        handleSpecialtyChange={handleSpecialtyChange}
        specialties={specialties}
      />

      <button
        type="submit"
        onClick={handleSubmit}
        className="btn btn-block"
        style={{ marginBottom: "0.5rem" }}
      >
        Save
      </button>
      <Link
        style={{ textAlign: "center" }}
        className="btn btn-block"
        to="/doctors"
      >
        Cancel
      </Link>
      <Alert
        isOpen={alertConfig.isOpen}
        message={alertConfig.message}
        type={alertConfig.type}
        duration={alertConfig.duration}
        onClose={closeAlert}
      />
    </form>
  );
}
export default AddNewDoctor;
