import { useState } from "react";
import { useLocation } from "react-router-dom";
import ExamType from "@/components/ExamType";
import { ExamTypeProps } from "@/components/ExamType";
import { useGetSubjectWithMaterialsQuery } from "../../api/api";

import {
  Heart,
  Download,
  PlusCircle,
  Settings,
  ChevronLeft,
  Search,
  ChevronUp,
  ChevronDown,
  UserCircle,
  LayoutGrid,
  Menu,
  X,
} from "lucide-react";
import { useNavigate } from "react-router-dom";
import { Console } from "console";

export default function Index() {
  const content: ExamTypeProps[] = [
    { examType: "Экзамены", examNames: ["Экзамен 1", "Экзамен 2"] },
    { examType: "Коллоквиумы", examNames: ["Коллоквиум 1", "Коллоквиум 2"] },
    { examType: "Контрольные работы", examNames: ["КР 1", "КР 2"] },
    { examType: "Лекции", examNames: ["Лекция 1", "Лекция 2"] },
  ];
  const subjectName = sessionStorage.getItem("selectedSubject"); // Здесь должен быть реальный ID предмета
  const { 
    data: subjectMaterials, 
    isLoading: isLoadingMaterials, 
    error: materialsError 
  } = useGetSubjectWithMaterialsQuery(subjectName);
  const serverContent = subjectMaterials;
  console.log("serverContent was gone")

  const navigate = useNavigate();
  const location = useLocation();
  const { subject } = location.state || {};
  const handleClickBack = () => {
    navigate("/main_page");
  };

  return (
    <div className="flex min-h-screen bg-app-bg font-[Inter]">
      {/* Main Content */}
      <main className="flex-1">
        {/* Top Bar */}
        <div className="flex items-center justify-between px-5 md:px-[24px] py-[14px]">
          {/* Левая часть - кнопки */}
          <div className="flex items-center gap-2 flex-shrink-0">
            <button
              onClick={handleClickBack}
              className="hover:opacity-80 transition-opacity"
            >
              <ChevronLeft className="w-10 h-10 text-app-text stroke-[2]" />
            </button>
          </div>
        </div>

        {/* Content Sections */}
        <div className="px-5 md:px-[24px] py-0 flex flex-col gap-[15px] max-w-[780px] md:mr-auto md:ml-20 md:top-20">
          {/* Центральная часть - заголовок */}
          <div className="flex-1 flex justify-left">
            <h1 className="text-app-text text-[28px] md:text-[45px] font-semibold tracking-[0.9px] text-center">
              {sessionStorage.getItem("selectedSubject")}
            </h1>
          </div>
          {/* Экзамены Section */}
          {content.map((examProps) => (
            <div>
              <ExamType
                examType={examProps.examType}
                examNames={examProps.examNames}
              ></ExamType>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}
