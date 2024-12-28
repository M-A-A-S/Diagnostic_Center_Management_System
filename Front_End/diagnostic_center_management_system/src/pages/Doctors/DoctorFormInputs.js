import { formatDate } from "../../utils/util";
import FormInput from "../../components/FormInput";

const DoctorFormInputs = ({
  doctorData,
  onChange,
  specialty,
  handleSpecialtyChange,
  specialties,
  errors,
}) => {
  return (
    <>
      <FormInput
        label="Name"
        type="text"
        name="name"
        errorMessage={errors.name}
        value={doctorData.name}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Date Of Brith"
        type="date"
        name="dateOfBirth"
        errorMessage={errors.dateOfBirth}
        value={doctorData.dateOfBirth}
        min={`1950-01-01`}
        max={formatDate(new Date().setFullYear(new Date().getFullYear() - 18))}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Phone"
        type="text"
        name="phone"
        errorMessage={errors.phone}
        value={doctorData.phone}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Address"
        type="textarea"
        name="address"
        errorMessage={errors.address}
        value={doctorData.address}
        onChange={onChange}
        required={true}
      />
      <FormInput
        label="Join Date"
        type="date"
        name="joinDate"
        errorMessage={errors.joinDate}
        min={`1950-01-01`}
        max={formatDate(new Date())}
        value={doctorData.joinDate}
        onChange={onChange}
        required={true}
      />
      <div className="form-row">
        <label className="form-label">Designation</label>
        <select
          className="form-input"
          name=""
          id=""
          value={specialty}
          onChange={handleSpecialtyChange}
        >
          {specialties &&
            specialties.map((specialty) => {
              return (
                <option key={specialty.id} value={specialty.title}>
                  {specialty.title}
                </option>
              );
            })}
        </select>
        {errors.specialty && (
          <small className="form-alert">{errors.specialty}</small>
        )}
      </div>
    </>
  );
};
export default DoctorFormInputs;
