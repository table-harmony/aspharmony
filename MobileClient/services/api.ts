import axios from "axios";
import { API_BASE_URL } from "@/config/api";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Add request interceptor to handle auth tokens if needed
api.interceptors.request.use(
  (config) => {
    // Add auth token from secure storage if available
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle specific error cases (401, 403, etc.)
    return Promise.reject(error);
  }
);

export default api;
