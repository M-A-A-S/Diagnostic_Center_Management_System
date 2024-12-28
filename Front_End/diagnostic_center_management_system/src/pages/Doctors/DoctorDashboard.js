import { useNavigate } from "react-router-dom";
import { formatDate, handleDelete, handleGetAll } from "../../utils/util";
import { useEffect, useState } from "react";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";

const DoctorDashboard = () => {
  const navigate = useNavigate();
  const [doctors, setDoctors] = useState([]);
  const [specialties, setSpecialties] = useState([]);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [recordIdToDelete, setRecordIdToDelete] = useState(null);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getAllSecialties();
    getAllDoctors();
  }, []);

  const getAllSecialties = async () => {
    let result = await handleGetAll("/api/Specialties/All");

    if (result) {
      setSpecialties(result);
    }
  };

  const getAllDoctors = async () => {
    let result = await handleGetAll("/api/Doctors/All");

    if (result) {
      setDoctors(result);
    }
  };

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

  const deleteDoctor = async () => {
    if (recordIdToDelete) {
      let result = await handleDelete(`/api/Doctors/${recordIdToDelete}`);

      if (result) {
        showAlert("Data Deleted Successfuly", "success");
        setDoctors(doctors.filter((doctor) => doctor.id !== recordIdToDelete));
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }

    handleCloseDialog();
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Doctors Dashboard</h2>
        <div className="title-underline"></div>
      </div>
      <button onClick={() => navigate("/doctors/create")} className="btn">
        Add New Doctor
      </button>
      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Doctor ID</th>
              <th>Doctor Name</th>
              <th>Date Of Birth</th>
              <th>Doctor Phone</th>
              <th>Doctor Address</th>
              <th>Designation</th>
              <th>Join Date</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {doctors &&
              doctors.map((doctor) => {
                return (
                  <tr key={doctor.id}>
                    <td>{doctor.id}</td>
                    <td>{doctor.name}</td>
                    <td>{formatDate(doctor.dateOfBirth)}</td>
                    <td>{doctor.phone}</td>
                    <td>{doctor.address}</td>
                    <td>{getSpecialtyName(doctor.specialityId)}</td>
                    <td>{formatDate(doctor.joinDate)}</td>
                    <td>
                      <button
                        onClick={() => navigate(`/doctors/update/${doctor.id}`)}
                        className="btn"
                      >
                        Edit
                      </button>
                    </td>
                    <td>
                      <button
                        onClick={() => handleOpenDialog(doctor.id)}
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
        onConfirm={deleteDoctor}
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
export default DoctorDashboard;
