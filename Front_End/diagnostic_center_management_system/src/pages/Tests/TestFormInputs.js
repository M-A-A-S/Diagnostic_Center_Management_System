import FormInput from "../../components/FormInput";

const TestFormInputs = (props) => {
  const { testData, onChange, errors } = props;

  return (
    <>
      <FormInput
        label="Title"
        name="title"
        value={testData.title}
        onChange={onChange}
        errorMessage={errors.title}
      />
      <FormInput
        label="Cost"
        name="cost"
        value={testData.cost}
        onChange={onChange}
        errorMessage={errors.cost}
      />
    </>
  );
};
export default TestFormInputs;
