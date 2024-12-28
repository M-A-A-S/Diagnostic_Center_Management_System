import { useEffect, useState } from "react";
import { handleDelete, handleGetAll } from "../../utils/util";
import { Link } from "react-router-dom";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";

const TestsDashboard = () => {
  const [tests, setTests] = useState([]);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [recordIdToDelete, setRecordIdToDelete] = useState(null);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getAllTests();
  }, []);

  const getAllTests = async () => {
    let result = await handleGetAll("/api/Tests/All");

    if (result) {
      setTests(result);
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

  const deleteTest = async () => {
    let result = await handleDelete(`/api/Tests/${recordIdToDelete}`);

    if (result) {
      setTests(tests.filter((test) => test.id !== recordIdToDelete));
      showAlert("Data Deleted Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }

    handleCloseDialog();
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Tests Dashboard</h2>
        <div className="title-underline"></div>
      </div>

      <Link className="btn" to="/tests/create">
        Add New Test
      </Link>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Title</th>
              <th>Cost</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {tests &&
              tests.map((test) => {
                return (
                  <tr key={test.id}>
                    <td>{test.id}</td>
                    <td>{test.title}</td>
                    <td>{test.cost}</td>
                    <td>
                      {" "}
                      <Link className="btn" to={`/tests/update/${test.id}`}>
                        Edit
                      </Link>
                    </td>
                    <td>
                      <button
                        onClick={() => handleOpenDialog(test.id)}
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
        onConfirm={deleteTest}
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
export default TestsDashboard;
