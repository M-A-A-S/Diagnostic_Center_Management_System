import { useNavigate } from "react-router-dom";
import { formatDate, handleDelete, handleGetAll } from "../../utils/util";
import { useEffect, useState } from "react";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";

const PatientDashboard = () => {
  const navigate = useNavigate();

  const [patients, setPatients] = useState([]);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [recordIdToDelete, setRecordIdToDelete] = useState(null);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getAllPatients();
  }, []);

  const getAllPatients = async () => {
    const result = await handleGetAll("/api/Patients/All");

    if (result) {
      setPatients(result);
    }
  };

  const handleOpenDialog = (recordId) => {
    setRecordIdToDelete(recordId);
    setIsDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false);
    setRecordIdToDelete(0);
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
  };

  const handleDeletePatient = async () => {
    let result = await handleDelete(`/api/Patients/${recordIdToDelete}`);

    if (result) {
      setPatients(
        patients.filter((patient) => patient.id !== recordIdToDelete)
      );
      showAlert("Data Deleted Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }

    handleCloseDialog();
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Patients Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <button onClick={() => navigate("/patients/create")} className="btn">
        Add New Patient
      </button>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Patient ID</th>
              <th>Name</th>
              <th>Date Of Birth</th>
              <th>Sex</th>
              <th>Phone</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {patients &&
              patients.map((patient) => {
                return (
                  <tr key={patient.id}>
                    <td>{patient.id}</td>
                    <td>{patient.name}</td>
                    <td>{formatDate(patient.dateOfBirth)}</td>
                    <td>{patient.sex ? "Female" : "Male"}</td>
                    <td>{patient.phone}</td>
                    <td>
                      <button
                        onClick={() =>
                          navigate(`/patients/update/${patient.id}`)
                        }
                        className="btn"
                      >
                        Edit
                      </button>
                    </td>
                    <td>
                      <button
                        onClick={() => handleOpenDialog(patient.id)}
                        className="btn"
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                );
              })}
          </tbody>
        </table>
      </div>
      <Dialog
        isOpen={isDialogOpen}
        title="Confirm Your Action"
        message="Are you sure you want to delete this record?"
        onConfirm={handleDeletePatient}
        onCancel={handleCloseDialog}
      />
      <Alert
        isOpen={alertConfig.isOpen}
        message={alertConfig.message}
        type={alertConfig.type}
        duration={alertConfig.duration}
        onClose={closeAlert}
      />
    </section>
  );
};
export default PatientDashboard;
