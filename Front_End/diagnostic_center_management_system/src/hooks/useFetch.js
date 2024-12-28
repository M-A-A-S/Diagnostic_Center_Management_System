import { useEffect, useState } from "react";
import { fetchWithToken } from "../api/apiClient";

const DEFAULT_OPTIONS = {
  headers: { "Content-Type": "application/json" },
};

const useFetch = (url, options = {}, dependencies = []) => {
  const [isLoading, setIsLoading] = useState(true);
  const [isError, setIsError] = useState(false);
  const [data, setData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        //const resp = await fetch(url, { ...DEFAULT_OPTIONS, ...options });
        const resp = await fetchWithToken(url, {
          ...DEFAULT_OPTIONS,
          ...options,
        });
        if (!resp.ok) {
          setIsError(true);
          setIsLoading(false);
          return;
        }

        const response = await resp.json();
        setData(response);
      } catch (error) {
        setIsError(true);
      }

      // hide loading
      setIsLoading(false);
    };
    fetchData();
  }, dependencies);

  return { isLoading, isError, data };
};

export default useFetch;
