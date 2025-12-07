export interface AuthResponse {
  token: string;
}

export interface RegisterResponse {
  message: string;
}

export interface MaterialDto {
  id: string;
  name: string;
  type: string;
}

export interface UploadResponse {
  id: string;
}

export interface DownloadLinkResponse {
  downloadUrl: string;
}

export interface SubjectDto {
  id: string;
  name: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}
