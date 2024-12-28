import { useEffect, useState } from "react";
import Navbar from "../../components/Navbar";
import { handleGetAll } from "../../utils/util";

function Dashboard() {
  const [numberOfDoctors, setNumberOfDoctors] = useState(0);
  const [numberOfPatients, setNumberOfPatients] = useState(0);
  const [income, setIncome] = useState(0);

  useEffect(() => {
    getNumberOfDoctors();
    getNumberOfPatients();
    getIncome();
  }, []);

  const getNumberOfDoctors = async () => {
    const result = await handleGetAll("/api/Doctors/NumberOfDoctors");
    if (result) {
      setNumberOfDoctors(result);
    } else {
      setNumberOfDoctors(0);
    }
  };

  const getNumberOfPatients = async () => {
    const result = await handleGetAll("/api/Patients/NumberOfPatients");
    if (result) {
      setNumberOfPatients(result);
    } else {
      setNumberOfPatients(0);
    }
  };

  const getIncome = async () => {
    const result = await handleGetAll("/api/Invoices/Income");
    if (result) {
      setIncome(result);
    } else {
      setIncome(0);
    }
  };

  return (
    <>
      <Navbar />

      <section className="section section-center">
        <div className="title" style={{ marginBottom: "2rem" }}>
          <h2>Dashboard</h2>
          <div className="title-underline"></div>
        </div>

        <div className="dashboard-container">
          <div className="dashboard-card">
            <h4>Doctors</h4>
            <p>{numberOfDoctors}</p>
          </div>
          <div className="dashboard-card">
            <h4>Patients</h4>
            <p>{numberOfPatients}</p>
          </div>
          <div className="dashboard-card">
            <h4>Income</h4>
            <p>{income}</p>
          </div>
        </div>
      </section>
    </>
  );
}

export default Dashboard;
