import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import {
  AuthResponse,
  DownloadLinkResponse,
  LoginRequest,
  MaterialDto,
  RegisterRequest,
  RegisterResponse,
  SubjectDto,
  UploadResponse,
} from "./types";

const BASE_URL = "http://localhost:5179/api/";

export const appApi = createApi({
  reducerPath: "appApi",
  baseQuery: fetchBaseQuery({
    baseUrl: BASE_URL,
    prepareHeaders: (headers) => {
      const token = localStorage.getItem("token");
      if (token) {
        headers.set("Authorization", `Bearer ${token}`);
      }
      return headers;
    },
  }),

  tagTypes: ["Materials", "Subjects"],
  endpoints: (builder) => ({
    login: builder.mutation<AuthResponse, LoginRequest>({
      query: (data) => ({
        url: "Auth/login",
        method: "POST",
        body: data,
      }),
      async QueryStarted(arg, { queryFulfilled }) {
        try {
          const { data } = await queryFulfilled;
          localStorage.setItem("token", data.token);
        } catch (err) {
          console.error("Login failed", err);
        }
      },
    }),

    register: builder.mutation<RegisterResponse, RegisterRequest>({
      query: (userData) => ({
        url: "Auth/register",
        method: "POST",
        body: userData,
      }),
    }),

    logout: builder.mutation<void, void>({
      queryFn: () => {
        localStorage.removeItem("token");
        return { data: undefined };
      },
    }),

    getMaterials: builder.query<MaterialDto[], string | undefined>({
      query: (subjectId) => ({
        url: "Materials",
        params: subjectId ? { subjectId } : undefined,
      }),
      providesTags: (result) =>
        result
          ? [
              ...result.map(({ id }) => ({ type: "Materials" as const, id })),
              { type: "Materials", id: "LIST" },
            ]
          : [{ type: "Materials", id: "LIST" }],
    }),

    uploadMaterial: builder.mutation<
      UploadResponse,
      { file: File; subject: string }
    >({
      query: ({ file, subject }) => {
        const formData = new FormData();
        formData.append("file", file);
        formData.append("subject", subject);

        return {
          url: "Materials",
          method: "POST",
          body: formData,
        };
      },
      invalidatesTags: [{ type: "Materials", id: "LIST" }],
    }),

    getDownloadLink: builder.query<string, string>({
      query: (id) => `Materials/${id}/download-link`,
      transformResponse: (response: DownloadLinkResponse) =>
        response.downloadUrl,
    }),

    getSubjects: builder.query<SubjectDto[], void>({
      query: () => "Subjects",
      providesTags: ["Subjects"],
      transformResponse: (response: string[]) => {
        return response.map((name, index) => ({
          id: name,
          name: name,
        }));
      },
    }),
  }),
});

export const {
  useLoginMutation,
  useRegisterMutation,
  useLogoutMutation,
  useGetMaterialsQuery,
  useUploadMaterialMutation,
  useGetDownloadLinkQuery,
  useGetSubjectsQuery,
} = appApi;
