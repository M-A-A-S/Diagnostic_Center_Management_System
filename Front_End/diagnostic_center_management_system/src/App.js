import { BrowserRouter, Route, Routes } from "react-router-dom";
import Doctors from "./pages/Doctors";
import Patients from "./pages/Patients";
import Dashboard from "./pages/Dashboard";
import Tests from "./pages/Tests";
import Invoices from "./pages/Invoices";
import AddNewDoctor from "./pages/Doctors/AddNewDoctor";
import UpdateDoctor from "./pages/Doctors/UpdateDoctor";
import AddNewPatient from "./pages/Patients/AddNewPatient";
import UpdatePatient from "./pages/Patients/UpdatePatient";
import AddNewTest from "./pages/Tests/AddNewTest";
import UpdateTest from "./pages/Tests/UpdateTest";
import AddNewInvoice from "./pages/Invoices/AddNewInvoice";
import UpdateInvoice from "./pages/Invoices/UpdateInvoice";
import Login from "./pages/Login";
import PrivateRoute from "./auth/PrivateRoute";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <Doctors />
            </PrivateRoute>
          }
        />
        <Route
          path="/doctors"
          element={
            <PrivateRoute>
              <Doctors />
            </PrivateRoute>
          }
        />
        <Route
          path="/doctors/create"
          element={
            <PrivateRoute>
              <AddNewDoctor />
            </PrivateRoute>
          }
        />
        <Route
          path="/doctors/update/:id"
          element={
            <PrivateRoute>
              <UpdateDoctor />
            </PrivateRoute>
          }
        />
        <Route
          path="/patients"
          element={
            <PrivateRoute>
              <Patients />
            </PrivateRoute>
          }
        />
        <Route
          path="/patients/create"
          element={
            <PrivateRoute>
              <AddNewPatient />
            </PrivateRoute>
          }
        />
        <Route
          path="/patients/update/:id"
          element={
            <PrivateRoute>
              <UpdatePatient />
            </PrivateRoute>
          }
        />
        <Route
          path="/tests"
          element={
            <PrivateRoute>
              <Tests />
            </PrivateRoute>
          }
        />
        <Route
          path="/tests/create"
          element={
            <PrivateRoute>
              <AddNewTest />
            </PrivateRoute>
          }
        />
        <Route
          path="/tests/update/:id"
          element={
            <PrivateRoute>
              <UpdateTest />
            </PrivateRoute>
          }
        />
        <Route
          path="/invoices"
          element={
            <PrivateRoute>
              <Invoices />
            </PrivateRoute>
          }
        />
        <Route
          path="/invoices/create"
          element={
            <PrivateRoute>
              <AddNewInvoice />
            </PrivateRoute>
          }
        />
        <Route
          path="/invoices/update/:id"
          element={
            <PrivateRoute>
              <UpdateInvoice />
            </PrivateRoute>
          }
        />
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <Dashboard />
            </PrivateRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}
export default App;
