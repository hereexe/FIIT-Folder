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

export interface ExamTypeProps {
  examType: string;
  examNames: string[];
}

export interface SubjectMaterialsResponse {
  id: string;
  name: string;
  semester: number;
  content: ExamTypeProps[];
}

export interface SubjectDto {
  id: string;
  name: string;
  semester?: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}
