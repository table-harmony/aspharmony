import axios from "axios";

export const BASE_URL = "http://localhost:7137";

export const api = axios.create({
  baseURL: `${BASE_URL}/api`,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});
