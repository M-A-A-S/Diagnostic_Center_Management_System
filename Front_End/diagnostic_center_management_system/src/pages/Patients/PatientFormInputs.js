import { formatDate } from "../../utils/util";
import FormInput from "../../components/FormInput";

const PatientFormInputs = ({ patientData, onChange, errors }) => {
  return (
    <>
      <FormInput
        label="Name"
        name="name"
        errorMessage={errors.name}
        value={patientData.name}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Date Of Birth"
        name="dateOfBirth"
        type="date"
        min="1950-01-01"
        max={formatDate(new Date().setFullYear(new Date().getFullYear() - 18))}
        errorMessage={errors.dateOfBirth}
        value={formatDate(patientData.dateOfBirth)}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Phone"
        name="phone"
        errorMessage={errors.phone}
        value={patientData.phone}
        onChange={onChange}
        required={true}
      />
      <div className="form-row">
        <label className="form-label">Sex</label>
        <select
          name="sex"
          value={Boolean(patientData.sex)}
          onChange={onChange}
          className="form-input"
        >
          <option value={false}>Male</option>
          <option value={true}>Female</option>
        </select>
      </div>
    </>
  );
};
export default PatientFormInputs;
