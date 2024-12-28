import { useState } from "react";
import FormInput from "../../components/FormInput";
import { useAuthContext } from "../../auth/AuthContext";
import { useNavigate } from "react-router-dom";
import Alert from "../../components/Alert";

const Login = () => {
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    username: "",
    password: "",
  });
  const { login } = useAuthContext();
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
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const validationErrors = {};

    if (!credentials.username.trim()) {
      validationErrors.username = "Username is required";
    }

    if (!credentials.password.trim()) {
      validationErrors.password = "Password is required";
    }

    setErrors(validationErrors);

    if (Object.keys(validationErrors).length === 0) {
      handleLogin();
    }
  };

  const handleLogin = async () => {
    try {
      await login(credentials);
      navigate("/");
    } catch (error) {
      showAlert("Something Went Wrong", "error");
    }
  };

  const onChange = (e) => {
    const { name, value } = e.target;
    setCredentials({ ...credentials, [name]: value });
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Login</h2>
        <div className="title-underline"></div>
      </div>

      <form className="form" onSubmit={handleSubmit}>
        <FormInput
          name="username"
          value={credentials.username}
          onChange={onChange}
          label="Username"
          errorMessage={errors.username}
        />
        <FormInput
          name="password"
          type="password"
          value={credentials.password}
          onChange={onChange}
          label="Password"
          errorMessage={errors.password}
        />
        <button
          onClick={handleSubmit}
          className="btn btn-block"
          style={{ marginBottom: "0.5rem" }}
        >
          Login
        </button>
      </form>

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
export default Login;
