import { useEffect, useState } from "react";
import { formatDate, handleDelete, handleGetAll } from "../../utils/util";
import { Link } from "react-router-dom";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";

const InvoicesDashboard = () => {
  const [invoices, setInvoices] = useState([]);
  const [recordIdToDelete, setRecordIdToDelete] = useState(null);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getAllInvoices();
  }, []);

  const getAllInvoices = async () => {
    let result = await handleGetAll("/api/Invoices/All");
    if (result) {
      setInvoices(result);
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

  const deleteInvoice = async () => {
    let result = await handleDelete(`/api/Invoices/${recordIdToDelete}`);
    if (result) {
      showAlert("Data Deleted Successfuly", "success");
      setInvoices(
        invoices.filter((invoice) => invoice.id !== recordIdToDelete)
      );
    } else {
      showAlert("Something Went Wrong", "error");
    }

    handleCloseDialog();
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Invoices Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link to="/invoices/create" className="btn">
        Add New Invoice
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Invoice ID</th>
              <th>Patient Id</th>
              <th>Patient Name</th>
              <th>Patient Phone</th>
              <th>Refered By</th>
              <th>Delivery Date</th>
              <th>Test ID</th>
              <th>Test Name</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {invoices &&
              invoices.map((invoice) => {
                return (
                  <tr key={invoice.id}>
                    <td>{invoice.id}</td>
                    <td>{invoice.patientId}</td>
                    <td>{invoice.patientInfo.name}</td>
                    <td>{invoice.patientInfo.phone}</td>
                    <td>{invoice.doctorInfo.name}</td>
                    <td>{formatDate(invoice.deliveryDate)}</td>
                    <td>{invoice.testId}</td>
                    <td>{invoice.testInfo.title}</td>
                    <td>
                      <Link
                        to={`/invoices/update/${invoice.id}`}
                        className="btn"
                      >
                        Edit
                      </Link>
                    </td>
                    <td>
                      <button
                        onClick={() => handleOpenDialog(invoice.id)}
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
        onConfirm={deleteInvoice}
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
export default InvoicesDashboard;
