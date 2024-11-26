export const API_BASE_URL = "https://localhost:7137";

export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: "/api/auth/login",
    REGISTER: "/api/auth/register",
    LOGOUT: "/api/auth/logout",
  },
  BOOKS: {
    LIST: "/api/books",
    CREATE: "/api/books",
    DETAILS: (id: string) => `/api/books/${id}`,
    UPDATE: (id: string) => `/api/books/${id}`,
    DELETE: (id: string) => `/api/books/${id}`,
    GENERATE_CHAPTERS: "/api/books/generate-chapters",
  },
  // Add other endpoints as needed
};
