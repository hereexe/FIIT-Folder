export interface MaterialDto {
  id: string;
  name: string;
  type: string;
}

export interface UploadResponse {
  id: string;
  subject: string;
}

export interface DownloadLinkResponse {
  downloadUrl: string;
}

export interface AuthResponse {
  token: string;
}

export interface RegisterResponse {
  message: string;
}
