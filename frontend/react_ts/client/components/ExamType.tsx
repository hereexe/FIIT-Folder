import React, { useState } from "react";
import { configureStore } from "@reduxjs/toolkit";
import { useNavigate } from "react-router-dom";
import { ChevronUp, ChevronDown } from "lucide-react";

export interface ExamTypeItemProps {
  displayName: string;
  semester: number;
  subjectId: string;
}

export interface ExamTypeProps {
  examType: string;
  rawType: string;
  items: ExamTypeItemProps[];
}

const ExamType: React.FC<ExamTypeProps> = ({ examType, rawType, items }) => {
  const [examsExpanded, setExamsExpanded] = useState(true);
  const navigate = useNavigate();

  const handleHeaderClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    setExamsExpanded(!examsExpanded);
  };

  const handleItemClick = (item: ExamTypeItemProps, e: React.MouseEvent) => {
    e.stopPropagation();
    sessionStorage.setItem("examName", item.displayName);
    sessionStorage.setItem("selectedSubjectId", item.subjectId);
    navigate("/doc_page", {
      state: { examType: rawType, examName: item.displayName, semester: item.semester, subjectId: item.subjectId },
    });
  };

  return (
    <div className="flex flex-col gap-[15px]">
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
          {items.map((item, index) => (
            <div
              key={`${examType}-${index}`}
              className="bg-app-item rounded-[10px] px-9 py-[11px] hover:opacity-80 transition-opacity cursor-pointer"
              onClick={(e) => handleItemClick(item, e)}
            >
              <div className="text-black text-[23px] line-clamp-1 font-medium tracking-[0.23px]">
                {item.displayName}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ExamType;
