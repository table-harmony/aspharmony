import { useUserStore } from "./userStore";
import { api } from "./api";

export const authService = {
  async login(email: string, password: string) {
    try {
      const response = await api.post("/auth/login", { email, password });
      useUserStore.getState().setUser(response.data);
      return response.data;
    } catch (error: any) {
      throw new Error(error.response?.data || "Login failed");
    }
  },

  async register(email: string, password: string, confirmPassword: string) {
    try {
      const response = await api.post("/auth/register", {
        email,
        password,
        confirmPassword,
      });
      useUserStore.getState().setUser(response.data);
      return response.data;
    } catch (error: any) {
      throw new Error(error.response?.data || "Registration failed");
    }
  },

  async logout() {
    try {
      await api.post("/auth/logout");
      useUserStore.getState().logout();
    } catch (error) {
      console.error("Logout failed:", error);
      throw error;
    }
  },

  async checkAuth() {
    try {
      const response = await api.get("/auth/check");
      if (!response.data) {
        useUserStore.getState().logout();
      }
      return response.data;
    } catch {
      useUserStore.getState().logout();
      return false;
    }
  },
};
