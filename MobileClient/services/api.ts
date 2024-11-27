import axios from "axios";

export const api = axios.create({
  baseURL: "https://localhost:7137/api",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

export const authService = {
  async login(email: string, password: string) {
    const response = await api.post("/auth/login", { email, password });
    return response.data;
  },

  async register(email: string, password: string, confirmPassword: string) {
    const response = await api.post("/auth/register", {
      email,
      password,
      confirmPassword,
    });
    return response.data;
  },

  async logout() {
    const response = await api.post("/auth/logout");
    return response.data;
  },

  async checkAuth() {
    try {
      const response = await api.get("/auth/check");
      return response.data;
    } catch {
      return false;
    }
  },

  async getNotifications(unreadOnly = false) {
    const response = await api.get("/auth/notifications", {
      params: { unreadOnly },
    });
    return response.data;
  },

  async markNotificationAsRead(id: number) {
    const response = await api.post(`/auth/notifications/${id}/read`);
    return response.data;
  },

  async markAllNotificationsAsRead() {
    const response = await api.post("/auth/notifications/read-all");
    return response.data;
  },
};
