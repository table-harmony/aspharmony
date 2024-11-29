import { api } from "./api";

export type User = {
  id: string;
  username: string;
  email: string;
  phone_number?: string;
};

export async function login(email: string, password: string) {
  const response = await api.post("/auth/login", { email, password });
  return response.data as User;
}

export async function register(email: string, password: string) {
  const response = await api.post("/auth/register", { email, password });
  return response.data as User;
}

export async function logout() {
  const response = await api.post("/auth/logout");
  return response.data;
}
