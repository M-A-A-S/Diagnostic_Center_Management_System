import { Link, useNavigate, useParams } from "react-router-dom";
import PatientFormInputs from "./PatientFormInputs";
import { useEffect, useState } from "react";
import { handleGetById, handleUpdate } from "../../utils/util";
import Alert from "../../components/Alert";

const UpdatePatient = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [patientData, setPatientData] = useState({
    id: 0,
    name: "",
    dateOfBirth: "",
    phone: "",
    sex: false,
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const fetchPatientData = async () => {
    const result = await handleGetById(`/api/Patients/${id}`);
    setPatientData(result);
  };

  useEffect(() => {
    fetchPatientData();
  }, []);

  const onChange = (e) => {
    const { name, value } = e.target;
    setPatientData({ ...patientData, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const validationErrors = {};

    if (!patientData.name.trim()) {
      validationErrors.name = "Name is required";
    }

    if (!patientData.dateOfBirth) {
      validationErrors.dateOfBirth = "Date of birth is required";
    }

    if (!patientData.phone.trim()) {
      validationErrors.phone = "Phone is required";
    } else if (isNaN(patientData.phone.trim())) {
      validationErrors.phone = "Phone is not valid";
    } else if (patientData.phone.trim().length !== 10) {
      validationErrors.phone = "Phone number should be 10 digits";
    }

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      updatePatient();
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/patients");
  };

  const updatePatient = async () => {
    let result = await handleUpdate(`/api/Patients/${id}`, {
      id: id,
      ...patientData,
      sex: patientData.sex === "true" ? true : false,
    });

    if (result) {
      showAlert("Data Updated Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="form">
      <h4
        style={{
          marginBottom: "1rem",
          textAlign: "center",
          color: "var(--primary-500)",
          fontWeight: "bold",
        }}
      >
        Update Patient
      </h4>

      <PatientFormInputs
        patientData={patientData}
        onChange={onChange}
        errors={errors}
      />

      <button
        onClick={handleSubmit}
        type="submit"
        className="btn btn-block"
        style={{ marginBottom: "0.5rem" }}
      >
        Save
      </button>
      <Link
        style={{ textAlign: "center" }}
        className="btn btn-block"
        to="/patients/"
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
export default UpdatePatient;
