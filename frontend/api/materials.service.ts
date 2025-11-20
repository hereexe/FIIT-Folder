import api from "./api";
import { MaterialDto, UploadResponse, DownloadLinkResponse } from "./types";

export const MaterialsService = {
  async getBySubject(subjectId?: string): Promise<MaterialDto[]> {
    const response = await api.get<MaterialDto[]>("/materials", {
      params: { subjectId },
    });
    return response.data;
  },

  async upload(file: File, subject: string): Promise<UploadResponse> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("subject", subject);

    const response = await api.post<UploadResponse>("/materials", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data;
  },

  async getDownloadLink(id: string): Promise<string> {
    const response = await api.get<DownloadLinkResponse>(
      `/materials/${id}/download-link`
    );
    return response.data.downloadUrl;
  },
};
