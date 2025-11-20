import api from "./api";
import { AuthResponse, RegisterResponse } from "./types";

export const AuthService = {
  async login(): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>("/auth/login");
    if (response.data.token) {
      localStorage.setItem("token", response.data.token);
    }
    return response.data;
  },

  async register(): Promise<RegisterResponse> {
    const response = await api.post<RegisterResponse>("/auth/register");
    return response.data;
  },

  logout() {
    localStorage.removeItem("token");
  },
};
