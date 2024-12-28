import { useEffect } from "react";
import "./Alert.css";

const Alert = ({
  message,
  type = "info",
  isOpen,
  duration = 3000,
  onClose,
}) => {
  useEffect(() => {
    if (isOpen) {
      const timer = setTimeout(() => {
        onClose();
      }, duration);

      return () => clearTimeout(timer);
    }
  }, [isOpen, duration, onClose]);

  if (!isOpen) {
    return null;
  }

  return (
    <div className={`alert alert-${type}`}>
      {message}
      <button className="alert-close" onClick={onClose}>
        &times;
      </button>
    </div>
  );
};
export default Alert;
