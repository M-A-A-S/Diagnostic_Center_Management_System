import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import TestFormInputs from "./TestFormInputs";
import { handleCreate } from "../../utils/util";
import Alert from "../../components/Alert";

const AddNewTest = () => {
  const navigate = useNavigate();

  const [testData, setTestData] = useState({
    id: 0,
    title: "",
    cost: "",
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  const onChange = (event) => {
    const { name, value } = event.target;
    setTestData({ ...testData, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    const validationErrors = {};

    if (testData.title.trim().length === 0) {
      validationErrors.title = "Title is required";
    }

    if (testData.cost.trim().length === 0) {
      validationErrors.cost = "Cost is required";
    } else if (isNaN(testData.cost.trim())) {
      validationErrors.cost = "Invalid data";
    }

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      addNewTest();
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/tests");
  };

  const addNewTest = async () => {
    let result = await handleCreate("/api/tests", testData);

    if (result) {
      showAlert("Data Added Successfuly", "success");
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
        Add New Test
      </h4>

      <TestFormInputs testData={testData} errors={errors} onChange={onChange} />

      <button
        onClick={handleSubmit}
        className="btn btn-block"
        style={{ marginBottom: "0.5rem" }}
      >
        Save
      </button>
      <Link
        className="btn btn-block"
        to="/tests"
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
export default AddNewTest;
