import { ChevronRight } from "lucide-react";
import { MaterialDto } from "../../../api/types";
import { useNavigate } from "react-router-dom";
import { useGetSubjectsQuery } from "../../../api/api";

interface ListItemProps {
  title: string;
  author: string;
  onClick?: () => void;
}

function ListItem({ title, author, onClick }: ListItemProps) {
  return (
    <button
      onClick={onClick}
      className="w-full flex items-center bg-purple-medium justify-between px-4 py-6 md:px-6 md:py-7 rounded-[20px] bg-item-bg transition-all hover:bg-item-bg/80 active:scale-[0.99]"
    >
      <div className="flex items-center gap-3 min-w-0 flex-1">
        <div className="w-20 h-20 md:w-24 md:h-24 flex-shrink-0" />
        <div className="flex flex-col items-start gap-1.5 min-w-0">
          <h3 className="text-item-text font-medium text-lg md:text-[23px] leading-tight tracking-[0.23px] truncate w-full text-left">
            {title}
          </h3>
          <p className="text-item-text text-base md:text-xl leading-normal truncate w-full text-left">
            {author}
          </p>
        </div>
      </div>
      <ChevronRight className="w-12 h-12 md:w-14 md:h-14 text-item-text flex-shrink-0 ml-4" strokeWidth={2} />
    </button>
  );
}

interface SearchResultProps {
  items: MaterialDto[];
}

const materialTypeLabels: Record<string, string> = {
  "Exam": "Экзамен",
  "Colloquium": "Коллоквиум",
  "Pass": "Зачёт",
  "ControlWork": "Контрольная работа",
  "ComputerPractice": "Компьютерный практикум"
};

export default function SearchResult({ items }: SearchResultProps) {
  const navigate = useNavigate();
  const { data: subjects = [] } = useGetSubjectsQuery();

  const handleItemClick = (material: MaterialDto) => {
    const subject = subjects.find(s => s.id === material.subjectId);
    const subjectName = subject?.name || "Предмет";
    const typeLabel = materialTypeLabels[material.materialType] || material.materialType;
    const examName = `${typeLabel}, ${material.semester} семестр`;

    // Сохраняем в sessionStorage для корректного отображения на странице Docs_page
    sessionStorage.setItem("selectedSubjectId", material.subjectId);
    sessionStorage.setItem("selectedSubject", subjectName);
    sessionStorage.setItem("examName", examName);

    navigate("/doc_page", {
      state: {
        examType: material.materialType,
        examName: examName,
        semester: material.semester,
        subjectId: material.subjectId
      }
    });
  }

  return (
    <div className="max-w-6xl mx-auto ">
      <div className="flex flex-col gap-5 h-[calc(100vh-120px)] md:h-[calc(100vh-140px)] overflow-y-auto pr-2 md:pr-3">
        {items.map((item) => (
          <ListItem
            key={item.id}
            title={item.name}
            author={item.authorName || "Unknown"}
            onClick={() => handleItemClick(item)}
          />
        ))}
        {items.length === 0 && (
          <div className="text-center text-gray-500 py-10">
            No results found
          </div>
        )}
      </div>
    </div>
  );
}
