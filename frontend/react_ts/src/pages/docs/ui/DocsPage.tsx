import { ChevronLeft } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { Material, MaterialCard } from "@/entities/material";

const DocsPage = () => {
  const [materials] = useState<Material[]>([
    { id: "1", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: true },
    { id: "2", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: true },
    { id: "3", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: false },
    { id: "4", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: false },
    { id: "5", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: false },
    { id: "6", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: false },
    { id: "7", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: false },
    { id: "8", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: true },
    { id: "9", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202, isliked: true },
  ]);

  const navigate = useNavigate();

  const handleClickBack = () => {
    navigate("/exam_type");
  };

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
            {sessionStorage.getItem("selectedSubject")} / {sessionStorage.getItem("examName")}
          </h2>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8 lg:gap-10 pb-20">
          {materials.map((material) => (
            <MaterialCard key={material.id} material={material} />
          ))}
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
};

export default DocsPage;
