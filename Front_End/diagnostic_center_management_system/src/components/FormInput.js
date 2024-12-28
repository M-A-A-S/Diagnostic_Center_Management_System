const FormInput = (props) => {
  const {
    label,
    errorMessage,
    value,
    onChange,
    type,
    name,
    placeholder,
    ...inputProps
  } = props;
  return (
    <div className="form-row">
      <label className="form-label">{label}</label>
      {type === "textarea" ? (
        <textarea
          type="textarea"
          name={name}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          {...inputProps}
          className="form-textarea"
        ></textarea>
      ) : (
        <input
          type={!type ? "text" : type}
          name={name}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          {...inputProps}
          className="form-input"
        />
      )}

      {errorMessage && <small className="form-alert">{errorMessage}</small>}
    </div>
  );
};
export default FormInput;
