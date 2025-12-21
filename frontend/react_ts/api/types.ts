export interface AuthResponse {
  token: string;
}

export interface RegisterResponse {
  userId: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
}
export type RatingType = "Like" | "Dislike";
  
export interface RateRequest {
  ratingType: RatingType;
}

export interface RatingResponse {
  likes: number;
  dislikes: number;
  userRating: RatingType | null;
}
export interface AddFavoriteRequest {
  materialId: string;
}
export interface MaterialDto {
  id: string;
  subjectId: string;
  name: string;
  year: number;
  semester?: number;
  description?: string;
  authorName?: string;
  isFavorite: boolean;
  materialType: string;
  size: string;
  uploadedAt: string;
  downloadUrl?: string;
}
export interface UploadMaterialRequest {
  file: File;
  subjectId: string;
  year: number;
  materialType: string;
  semester?: number;
  description?: string;
}
export interface GetMaterialsParams {
  subjectId?: string;
  materialType?: string;
  semester?: number;
  year?: number;
  searchQuery?: string;
}

export interface MaterialTypeResponse {
  value: string;
  displayName: string;
}

export interface SubjectDto {
  id: string;
  name: string;
  semester: number;
  materialTypes: MaterialTypeResponse[];
}

export interface SubjectResponse {
  id: string;
  name: string;
  semester: number;
  materialTypes: MaterialTypeResponse[];
}
export interface ExamTypeProps {
  examType: string;
  examNames: string[];
}

export interface SubjectWithMaterialsResponse {
  id: string;
  name: string;
  content: ExamTypeProps[];
}

export interface CreateSubjectRequest {
  name: string;
  semester: number;
  materialTypes: number[];
}
