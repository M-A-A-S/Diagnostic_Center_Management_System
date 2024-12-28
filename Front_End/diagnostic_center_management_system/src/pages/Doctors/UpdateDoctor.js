import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { formatDate, handleGetById, handleUpdate } from "../../utils/util";
import DoctorFormInputs from "./DoctorFormInputs";
import useFetch from "../../hooks/useFetch";
import Alert from "../../components/Alert";

const UpdateDoctor = () => {
  const navigate = useNavigate();
  const { id } = useParams();

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
  const { data: specialties } = useFetch("/api/Specialties/All");

  const fetchDoctorData = async () => {
    let result = await handleGetById(`/api/Doctors/${id}`);

    if (result) {
      setDoctorData({
        name: result.name,
        dateOfBirth: formatDate(result.dateOfBirth),
        phone: result.phone,
        address: result.address,
        joinDate: formatDate(result.joinDate),
        specialityId: result.specialityId,
      });
    }
  };

  useEffect(() => {
    fetchDoctorData();
  }, []);

  const getSpecialtyName = (id) => {
    let name = "";
    specialties &&
      specialties.forEach((s) => {
        if (s.id == id) {
          name = s.title;
        }
      });

    return name;
  };

  const getSpecialtyId = (specialty) => {
    let id = 0;
    specialties &&
      specialties.forEach((s) => {
        if (s.title == specialty) {
          id = s.id;
          //return s.id;
        }
      });

    return id;
    //return -1;
  };

  const handleSpecialtyChange = (event) => {
    setSpecialty(event.target.value);
    setDoctorData({
      ...doctorData,
      specialityId: getSpecialtyId(event.target.value),
    });
  };

  const updateDoctor = async () => {
    let result = await handleUpdate(`/api/Doctors/${id}`, {
      id: id,
      ...doctorData,
    });

    if (result) {
      showAlert("Data Updated Successfuly", "success");
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

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      //alert("Form submitted successfully");
      updateDoctor();
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/doctors");
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
        Update Doctor
      </h4>

      <DoctorFormInputs
        doctorData={doctorData}
        onChange={onChange}
        errors={errors}
        handleSpecialtyChange={handleSpecialtyChange}
        specialty={getSpecialtyName(doctorData.specialityId)}
        specialties={specialties}
      />

      <button
        onClick={handleSubmit}
        className="btn btn-block"
        style={{ marginBottom: "0.5rem" }}
      >
        Save
      </button>
      <Link
        style={{ textAlign: "center" }}
        className="btn btn-block"
        to="/doctors/"
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
};
export default UpdateDoctor;
