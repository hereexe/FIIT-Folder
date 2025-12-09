import { ChevronLeft } from "lucide-react";
import { useNavigate } from "react-router-dom";

import { ExamType, ExamTypeProps } from "@/entities/exam";

const content: ExamTypeProps[] = [
  { examType: "Экзамены", examNames: ["Экзамен 1", "Экзамен 2"] },
  { examType: "Коллоквиумы", examNames: ["Коллоквиум 1", "Коллоквиум 2"] },
  { examType: "Контрольные работы", examNames: ["КР 1", "КР 2"] },
  { examType: "Лекции", examNames: ["Лекция 1", "Лекция 2"] },
];

const ExamTypePage = () => {
  const navigate = useNavigate();

  const handleClickBack = () => {
    navigate("/main_page");
  };

  return (
    <div className="flex min-h-screen bg-app-bg font-[Inter]">
      <main className="flex-1">
        <div className="flex items-center justify-between px-5 md:px-[24px] py-[14px]">
          <div className="flex items-center gap-2 flex-shrink-0">
            <button onClick={handleClickBack} className="hover:opacity-80 transition-opacity">
              <ChevronLeft className="w-10 h-10 text-app-text stroke-[2]" />
            </button>
          </div>
        </div>

        <div className="px-5 md:px-[24px] py-0 flex flex-col gap-[15px] max-w-[780px] md:mr-auto md:ml-20 md:top-20">
          <div className="flex-1 flex justify-left">
            <h1 className="text-app-text text-[28px] md:text-[45px] font-semibold tracking-[0.9px] text-center">
              {sessionStorage.getItem("selectedSubject")}
            </h1>
          </div>
          {content.map((examProps) => (
            <ExamType key={examProps.examType} {...examProps} />
          ))}
        </div>
      </main>
    </div>
  );
};

export default ExamTypePage;
