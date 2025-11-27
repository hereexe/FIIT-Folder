import { useState } from "react";
import { MaterialsService } from "./materials.service";
import { UploadResponse } from "./types";
import { isAxiosError } from "axios";

interface UseUploadMaterialReturn {
  upload: (file: File, subject: string) => Promise<UploadResponse | undefined>;
  isLoading: boolean;
  error: string | null;
  data: UploadResponse | null;
}

export const useUploadMaterial = (): UseUploadMaterialReturn => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<UploadResponse | null>(null);

  const upload = async (file: File, subject: string) => {
    setIsLoading(true);
    setError(null);
    setData(null);

    try {
      const response = await MaterialsService.upload(file, subject);
      setData(response);
      return response;
    } catch (err) {
      let message = "Произошла ошибка при загрузке файла";
      if (isAxiosError(err)) {
        message = err.response?.data?.message || err.message;
      } else if (err instanceof Error) {
        message = err.message;
      }
      setError(message);
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return { upload, isLoading, error, data };
};
