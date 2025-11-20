import api from "./api";

export interface SubjectDto {
  id: string;
  name: string;
}

export const SubjectsService = {
  async getAll(): Promise<SubjectDto[]> {
    const response = await api.get<SubjectDto[]>("/subjects");
    return response.data;
  },
};
