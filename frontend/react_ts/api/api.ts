import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  RegisterResponse,
  SubjectResponse,
  SubjectWithMaterialsResponse,
  MaterialDto,
  UploadMaterialRequest,
  GetMaterialsParams,
  RatingResponse,
  RateRequest,
  CreateSubjectRequest,
  SubjectDto,
  AddFavoriteRequest,
} from "./types";

const BASE_URL = "http://158.160.99.237:8080/api";

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

  tagTypes: [
    "Materials",
    "MaterialDetail",
    "Subjects",
    "SubjectMaterials",
    "Favorites",
    "Rating",
  ],

  endpoints: (builder) => ({
    login: builder.mutation<AuthResponse, LoginRequest>({
      query: (data) => ({
        url: "Auth/login",
        method: "POST",
        body: data,
      }),
      async onQueryStarted(arg, { queryFulfilled }) {
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

    getMaterials: builder.query<MaterialDto[], GetMaterialsParams>({
      query: (params) => ({
        url: "Materials",
        params: {
          SubjectId: params.subjectId,
          MaterialType: params.materialType,
          Semester: params.semester,
          Year: params.year,
          SearchText: params.searchQuery,
        },
      }),
      providesTags: (result) =>
        result
          ? [
            ...result.map(({ id }) => ({ type: "Materials" as const, id })),
            { type: "Materials", id: "LIST" },
          ]
          : [{ type: "Materials", id: "LIST" }],
    }),

    getMaterialById: builder.query<MaterialDto, string>({
      query: (id) => `Materials/${id}`,
      providesTags: (result, error, id) => [{ type: "MaterialDetail", id }],
      transformResponse: (response: MaterialDto) => {
        return {
          ...response,
          downloadUrl: `${BASE_URL}/Materials/${response.id}/download`,
        };
      },
    }),

    uploadMaterial: builder.mutation<MaterialDto, UploadMaterialRequest>({
      query: ({
        file,
        subjectId,
        year,
        materialType,
        description,
        semester,
      }) => {
        const formData = new FormData();
        formData.append("File", file);
        formData.append("SubjectId", subjectId);
        formData.append("Year", year.toString());
        formData.append("MaterialType", materialType);

        if (description) formData.append("Description", description);
        if (semester) formData.append("Semester", semester.toString());

        return {
          url: "Materials",
          method: "POST",
          body: formData,
        };
      },
      invalidatesTags: [
        { type: "Materials", id: "LIST" },
        { type: "SubjectMaterials" },
      ],
    }),

    deleteMaterial: builder.mutation<void, string>({
      query: (id) => ({
        url: `Materials/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["Materials", "SubjectMaterials", "Favorites"],
    }),

    rateMaterial: builder.mutation<
      RatingResponse,
      { id: string; rating: RateRequest }
    >({
      query: ({ id, rating }) => ({
        url: `materials/${id}/rate`,
        method: "POST",
        body: rating,
      }),
      invalidatesTags: (result, error, { id }) => [
        { type: "Materials", id: "LIST" },
        { type: "MaterialDetail", id },
        { type: "Favorites", id: "LIST" },
      ],
    }),

    getMaterialRating: builder.query<RatingResponse, string>({
      query: (id) => `materials/${id}/rating`,
      providesTags: (result, error, id) => [{ type: "Rating", id }],
    }),

    addFavoriteMaterial: builder.mutation<string, AddFavoriteRequest>({
      query: (body) => ({
        url: "Favorites/materials",
        method: "POST",
        body,
      }),
      invalidatesTags: (result, error, { materialId }) => [
        { type: "Favorites" },
        { type: "Materials", id: "LIST" },
        { type: "MaterialDetail", id: materialId },
      ],
    }),

    removeFavoriteMaterial: builder.mutation<void, string>({
      query: (materialId) => ({
        url: `Favorites/${materialId}`,
        method: "DELETE",
      }),
      invalidatesTags: (result, error, materialId) => [
        { type: "Favorites" },
        { type: "Materials", id: "LIST" },
        { type: "MaterialDetail", id: materialId },
      ],
    }),

    getFavorites: builder.query<MaterialDto[], void>({
      query: () => "Favorites",
      providesTags: (result) =>
        result
          ? [
            ...result.map(({ id }) => ({ type: "Favorites" as const, id })),
            { type: "Favorites", id: "LIST" },
          ]
          : [{ type: "Favorites", id: "LIST" }],
      transformResponse: (response: any[]) => {
        return response.map(fav => ({
          ...fav,
          id: fav.materialId, // Use MaterialId as the ID for consistency in UI
          downloadUrl: fav.downloadUrl.startsWith('http')
            ? fav.downloadUrl
            : `${BASE_URL.replace('/api', '')}${fav.downloadUrl}`
        }));
      }
    }),

    getSubjects: builder.query<SubjectDto[], void>({
      query: () => "Subjects",
      providesTags: ["Subjects"],
    }),

    getSubjectWithMaterials: builder.query<
      SubjectWithMaterialsResponse,
      string
    >({
      query: (subjectId) => `Subjects/${subjectId}/materials`,
      providesTags: (result, error, subjectId) => [
        { type: "SubjectMaterials", id: subjectId },
      ],
    }),

    createSubject: builder.mutation<SubjectResponse, CreateSubjectRequest>({
      query: (data) => ({
        url: "Subjects",
        method: "POST",
        body: data,
      }),
      invalidatesTags: ["Subjects"],
    }),
  }),
});

export const {
  useLoginMutation,
  useRegisterMutation,
  useLogoutMutation,
  useGetMaterialsQuery,
  useGetMaterialByIdQuery,
  useUploadMaterialMutation,
  useDeleteMaterialMutation,
  useRateMaterialMutation,
  useGetMaterialRatingQuery,
  useGetSubjectsQuery,
  useGetSubjectWithMaterialsQuery,
  useCreateSubjectMutation,
  useAddFavoriteMaterialMutation,
  useRemoveFavoriteMaterialMutation,
  useGetFavoritesQuery,
} = appApi;
