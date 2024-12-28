import { fetchWithToken } from "../api/apiClient";

export const formatDate = (dateString) => {
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = date.getMonth() + 1;
  const day = date.getDate();

  return `${year}-${month < 10 ? `0${month}` : month}-${
    day < 10 ? `0${day}` : day
  }`;
};

export const handleCreate = async (url, object) => {
  let response = await fetchWithToken(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(object),
  });

  return response.ok ? true : false;
};

export const handleGetAll = async (url) => {
  let response = await fetchWithToken(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  const result = await response.json();

  return response.ok ? result : false;
};

export const handleGetById = async (url) => {
  let response = await fetchWithToken(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  const result = await response.json();

  return response.ok ? result : false;
};

export const handleUpdate = async (url, object) => {
  let response = await fetchWithToken(url, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(object),
  });

  return response.ok ? true : false;
};

export const handleDelete = async (url) => {
  let response = await fetchWithToken(url, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return response.ok ? true : false;
};

// export const handleCreate = async (url, object) => {
//   let response = await fetch(url, {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     body: JSON.stringify(object),
//   });

//   return response.ok ? true : false;
// };

// export const handleGetAll = async (url) => {
//   let response = await fetch(url, {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//     },
//   });

//   const result = await response.json();

//   return response.ok ? result : false;
// };

// export const handleGetById = async (url) => {
//   let response = await fetch(url, {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//     },
//   });

//   const result = await response.json();

//   return response.ok ? result : false;
// };

// export const handleUpdate = async (url, object) => {
//   let response = await fetch(url, {
//     method: "PUT",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     body: JSON.stringify(object),
//   });

//   return response.ok ? true : false;
// };

// export const handleDelete = async (url) => {
//   let response = await fetch(url, {
//     method: "DELETE",
//     headers: {
//       "Content-Type": "application/json",
//     },
//   });

//   return response.ok ? true : false;
// };
