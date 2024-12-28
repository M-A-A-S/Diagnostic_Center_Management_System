import { useState } from "react";
import { handleCreate } from "../../utils/util";
import { Link, useNavigate } from "react-router-dom";
import InvoiceFormFields from "./InvoiceFormFields";
import Alert from "../../components/Alert";

const AddNewInvoice = () => {
  const navigate = useNavigate();

  const [invoiceData, setInvoiceData] = useState({
    id: 0,
    patientId: 0,
    doctorId: 0,
    deliveryDate: "",
    testId: 0,
  });

  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/invoices");
  };

  const addNewInvoice = async () => {
    let result = await handleCreate("/api/Invoices", invoiceData);
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const onChange = (event) => {
    const { name, value } = event.target;
    setInvoiceData({ ...invoiceData, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    const validationErrors = {};

    if (!invoiceData.patientId) {
      validationErrors.patientId = "Patient is required";
    }
    if (!invoiceData.doctorId) {
      validationErrors.doctorId = "Doctor is required";
    }
    if (!invoiceData.testId) {
      validationErrors.testId = "Test is required";
    }
    if (!invoiceData.deliveryDate) {
      validationErrors.deliveryDate = "Delivery Date is required";
    }

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      addNewInvoice();
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
        Add New Invoice
      </h4>

      <InvoiceFormFields
        invoiceData={invoiceData}
        onChange={onChange}
        errors={errors}
      />

      <button
        onClick={handleSubmit}
        className="btn btn-block"
        style={{ marginBottom: "0.5rem" }}
      >
        Save
      </button>
      <Link
        to="/invoices"
        className="btn btn-block"
        style={{ textAlign: "center" }}
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
export default AddNewInvoice;
