import {
  ChevronLeft,
  Heart,
  Download,
  ThumbsUp,
  ThumbsDown,
} from "lucide-react";
import { useState } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { useGetMaterialsQuery } from '../../api/api';
import { MaterialDto } from "../../api/types";

export default function Index() {
  const location = useLocation();
  const navigate = useNavigate();

  const { examType, examName, semester, subjectId: stateSubjectId } = location.state || {};
  const sessionSubjectId = sessionStorage.getItem("selectedSubjectId");
  const subjectId = stateSubjectId || sessionSubjectId;
  const subjectName = sessionStorage.getItem("selectedSubject");

  const { data: materialsData, isLoading, error } = useGetMaterialsQuery({
    subjectId: subjectId || undefined,
    materialType: examType,
    semester: semester,
  });

  const handleClickBack = () => {
    navigate("/exam_type");
  };

  if (isLoading) return <div className="min-h-screen bg-fiit-bg flex items-center justify-center text-fiit-text">Загрузка материалов...</div>;
  if (error) return <div className="min-h-screen bg-fiit-bg flex items-center justify-center text-fiit-text">Ошибка загрузки материалов</div>;

  const materials = materialsData ? [...materialsData].sort((a, b) => b.likesCount - a.likesCount) : [];

  return (
    <div className="min-h-screen bg-fiit-bg font-inter">
      <div className="px-6 pt-8 pb-4 md:px-10 lg:px-16">
        <div className="flex items-center gap-4 mb-12">
          <button
            onClick={handleClickBack}
            className="flex-shrink-0 text-fiit-text hover:opacity-80 transition-opacity"
            aria-label="Go back"
          >
            <ChevronLeft className="w-14 h-14" strokeWidth={1.5} />
          </button>
          <h2 className="text-4xl md:text-5xl font-semibold text-fiit-text tracking-wide leading-tight">
            {subjectName} / {examName}
          </h2>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8 lg:gap-10 pb-20">
          {materials.map((material) => (
            <MaterialCard key={material.id} material={material} />
          ))}
          {materials.length === 0 && (
            <div className="col-span-full text-center text-fiit-text text-xl">
              Материалы не найдены
            </div>
          )}
        </div>
      </div>

      <button
        className="fixed bottom-8 right-8 text-fiit-text hover:opacity-80 transition-opacity"
        aria-label="Add new material"
      >
        <svg className="w-[60px] h-[60px]" viewBox="0 0 60 60" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M30 55C43.8071 55 55 43.8071 55 30C55 16.1929 43.8071 5 30 5C16.1929 5 5 16.1929 5 30C5 43.8071 16.1929 55 30 55Z" fill="#E8E7F9" fillOpacity="0.77" />
          <path d="M30 20V40M20 30H40M55 30C55 43.8071 43.8071 55 30 55C16.1929 55 5 43.8071 5 30C5 16.1929 16.1929 5 30 5C43.8071 5 55 16.1929 55 30Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
        </svg>
      </button>
    </div>
  );
}

function MaterialCard({ material }: { material: MaterialDto }) {
  const navigate = useNavigate();
  const handleClickFile = () => {
    console.log(material)
    navigate("/fileview_page", {
      state: {
        material: material  // Передаем весь объект material
      }
    });
  }
  return (
    <div
      onClick={() => handleClickFile()}
      className="relative w-full aspect-[5/4] rounded-[10px] bg-fiit-card overflow-hidden group cursor-pointer block transition-all duration-300 hover:shadow-lg hover:scale-105 active:scale-100"
    >
      <div className="absolute top-0 left-0 right-0 p-[10px_10px_10px_7px] flex flex-col gap-[7px] z-10">
        <h3 className="text-[23px] font-medium text-fiit-text tracking-[0.23px] leading-tight truncate group-hover:text-opacity-80 transition-colors">
          {material.name}
        </h3>
        <p className="text-xl text-fiit-text truncate group-hover:text-opacity-80 transition-colors">
          {material.authorName || "Аноним"}
        </p>
      </div>

      <div className="absolute bottom-0 left-0 right-0 h-[35px] bg-fiit-bg/35 group-hover:bg-fiit-bg/50 transition-colors" />

      <div className="absolute bottom-0 left-0 right-0 h-[35px] flex items-center justify-between px-[15px] z-10">
        <div className="flex items-center gap-4">
          <div className="flex items-center gap-1">
            <Heart className={`w-[20px] h-[20px] ${material.isFavorite ? "fill-red-500 text-red-500" : "text-fiit-text"}`} />
          </div>
          <div className="flex items-center gap-1">
            <ThumbsUp className={`w-[20px] h-[20px] ${material.currentUserRating === "Like" ? "fill-blue-500 text-blue-500" : "text-fiit-text"}`} />
            <span className="text-lg text-fiit-text">{material.likesCount || 0}</span>
          </div>
        </div>
        <span className="text-lg text-fiit-text">{material.sizeFormatted}</span>
      </div>
    </div>
  );
}
