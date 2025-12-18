import React, { useState } from "react";
import { configureStore } from '@reduxjs/toolkit';
import { useNavigate } from "react-router-dom";
import {
  ChevronUp,
  ChevronDown,
  
} from "lucide-react";


export interface ExamTypeProps {
  examType: string;
  examNames: string[];
}

const ExamType: React.FC<ExamTypeProps> = ({ examType, examNames }) => {
  const [examsExpanded, setExamsExpanded] = useState(true);
  const navigate = useNavigate();
  
  const handleHeaderClick = (e: React.MouseEvent) => {
    e.stopPropagation(); // Важно: останавливаем всплытие
    setExamsExpanded(!examsExpanded);
  };
  
  const handleExamClick = (examName: string, e: React.MouseEvent) => {
    e.stopPropagation(); // Останавливаем всплытие
    console.log("Клик по экзамену:", examType, examName);
    sessionStorage.setItem('examName', examName);
    navigate("/doc_page", {
      state: { examType, examName }
    });
  };
  
  return (
    <div 
      className="flex flex-col gap-[15px]"
      
    >
      <button
        onClick={handleHeaderClick}
        className="flex items-center justify-between w-full md:w-[500px] hover:opacity-80 transition-opacity"
      >
        <div className="flex items-center gap-3">
          <div className="w-5 h-5 rounded-full bg-app-text flex-shrink-0" />
          <span className="text-app-text text-[25px] font-medium tracking-[0.25px]">
            {examType}
          </span>
        </div>
        {examsExpanded ? (
          <ChevronUp className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
        ) : (
          <ChevronDown className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
        )}
      </button>

      {examsExpanded && (
        <div className="flex flex-col gap-[15px]">
          {examNames.map((examName, index) => (
            <div 
              key={`${examType}-${index}`}
              className="bg-app-item rounded-[10px] px-9 py-[11px] hover:opacity-80 transition-opacity"
              onClick={(e) => handleExamClick(examName, e)}
            >
              <div className="text-black text-[23px] font-medium tracking-[0.23px] cursor-pointer">
                {examName}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ExamType;