import { useEffect, useState } from "react";
import { formatDate, handleGetAll } from "../../utils/util";
import FormInput from "../../components/FormInput";

const InvoiceFormFields = (props) => {
  const { invoiceData, onChange, errors } = props;

  const [doctors, setDoctors] = useState([]);
  const [patients, setPatients] = useState([]);
  const [tests, setTests] = useState([]);

  useEffect(() => {
    getAllPatients();
    getAllDoctors();
    getAllTests();
  }, []);

  const getAllDoctors = async () => {
    let result = await handleGetAll("/api/Doctors/All");
    if (result) {
      setDoctors(result);
    }
  };
  const getAllPatients = async () => {
    const result = await handleGetAll("/api/Patients/All");
    if (result) {
      setPatients(result);
    }
  };
  const getAllTests = async () => {
    let result = await handleGetAll("/api/Tests/All");
    if (result) {
      setTests(result);
    }
  };

  return (
    <>
      <div className="form-row">
        <label className="form-label">Patient</label>
        <select
          className="form-input"
          name="patientId"
          value={invoiceData.patientId}
          onChange={onChange}
        >
          {patients &&
            patients.map((patient) => {
              return (
                <option key={patient.id} value={patient.id}>
                  {patient.name}
                </option>
              );
            })}
        </select>
        {errors.patientId && (
          <small className="form-alert">{errors.patientId}</small>
        )}
      </div>

      <div className="form-row">
        <label className="form-label">Refered By</label>
        <select
          className="form-input"
          name="doctorId"
          value={invoiceData.doctorId}
          onChange={onChange}
        >
          {doctors &&
            doctors.map((doctor) => {
              return (
                <option key={doctor.id} value={doctor.id}>
                  {doctor.name}
                </option>
              );
            })}
        </select>
        {errors.doctorId && (
          <small className="form-alert">{errors.doctorId}</small>
        )}
      </div>

      <div className="form-row">
        <label className="form-label">Test</label>
        <select
          className="form-input"
          name="testId"
          value={invoiceData.testId}
          onChange={onChange}
        >
          {tests &&
            tests.map((test) => {
              return (
                <option key={test.id} value={test.id}>
                  {test.title}
                </option>
              );
            })}
        </select>
        {errors.testId && <small className="form-alert">{errors.testId}</small>}
      </div>

      <FormInput
        label="Delivery Date"
        type="date"
        name="deliveryDate"
        value={invoiceData.deliveryDate && formatDate(invoiceData.deliveryDate)}
        onChange={onChange}
        min="2000-01-01"
        max={formatDate(new Date())}
        errorMessage={errors.deliveryDate}
      />
    </>
  );
};
export default InvoiceFormFields;
