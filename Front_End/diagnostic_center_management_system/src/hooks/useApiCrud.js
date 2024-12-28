import { useState, useEffect } from "react";

const useApiCrud = (initialUrl) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // GET

  const fetchData = async (url) => {
    setLoading(true);
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`Network response was not ok: ${response.statusText}`);
      }
      const data = await response.json();
      setData(data);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData(initialUrl);
  }, [initialUrl]);
  // POST
  const createData = async (url, data) => {
    setLoading(true);
    try {
      const response = await fetch(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });
      if (!response.ok) {
        throw new Error(`Network response was not ok: ${response.statusText}`);
      }
      const newData = await response.json();
      setData([...data, newData]);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  };
  // PUT
  const updateData = async (url, id, data) => {
    setLoading(true);
    try {
      const response = await fetch(`${url}/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });
      if (!response.ok) {
        throw new Error(`Network response was not ok: ${response.statusText}`);
      }
      const updatedData = await response.json();
      const updatedDataIndex = data.findIndex((item) => item.id === id);
      const updatedDataArr = [...data];
      updatedDataArr[updatedDataIndex] = updatedData;
      setData(updatedDataArr);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  };

  // DELETE
  const deleteData = async (url, id) => {
    setLoading(true);
    try {
      const response = await fetch(`${url}/${id}`, {
        method: "DELETE",
      });
      if (!response.ok) {
        throw new Error(`Network response was not ok: ${response.statusText}`);
      }
      const filteredData = data.filter((item) => item.id !== id);
      setData(filteredData);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  };

  return {
    data,
    loading,
    error,
    fetchData,
    createData,
    updateData,
    deleteData,
  };
};

export default useApiCrud;
