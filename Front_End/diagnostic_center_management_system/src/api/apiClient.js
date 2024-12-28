import CryptoJS from "crypto-js";

const secretKey = "bR3+mI1+hDPqVw1vEmwhfTCA/z+52vVIFFa022Wt2lk=";

export const encrypt = (token) => {
  const ciphertext = CryptoJS.AES.encrypt(token, secretKey).toString();
  return ciphertext;
};

export const decrypt = (ciphertext) => {
  const bytes = CryptoJS.AES.decrypt(ciphertext, secretKey);
  const originalText = bytes.toString(CryptoJS.enc.Utf8);
  return originalText;
};

const getHeaders = () => {
  const accessToken = localStorage.getItem("accessToken");
  const headers = { "Content-Type": "application/json" };
  if (accessToken.length !== 0) {
    headers.Authorization = `Bearer ${decrypt(accessToken)}`;
  }
  return headers;
};

const fetchWithToken = async (url, options = {}) => {
  const response = await fetch(url, {
    ...options,
    headers: {
      ...getHeaders(),
      ...options.headers,
    },
  });

  // Check for 401 to refresh token
  if (response.status === 401 && !options._retry) {
    const refreshResponse = await refreshToken();
    if (refreshResponse.ok) {
      const data = await refreshToken.json();
      localStorage.setItem("accessToken", encrypt(data.accessToken));

      // Retry the original request
      return fetchWithToken(url, { ...options, _retry: true });
    } else {
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      window.location.href = "/login";
      throw new Error("Unauthorized");
    }
  }

  return response;
};

const refreshToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  return fetch("/api/auth/refresh", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ refreshToken }),
  });
};

export { fetchWithToken };
