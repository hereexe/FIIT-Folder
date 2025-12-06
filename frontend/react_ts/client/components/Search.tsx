import { useState } from "react";
import { Search, Filter, X, CheckIcon, RefreshCcw } from "lucide-react";

interface FilterState {
  subjects: string[];
  contentTypes: string[];
  years: string[];
  semesters: string[];
}

export default function SearchMenu() {
  const [filters, setFilters] = useState<FilterState>({
    subjects: [],
    contentTypes: [],
    years: [],
    semesters: [],
  });

  const subjects = [
    "Математический анализ",
    "Алгебра и геометрия",
    "ОРГ",
    "Философия",
    "Теория вероятности",
    "Дискретная математика",
    "Nand To Tetris",
    "ОПД",
    "Сети и протоколы интернета",
    "Английский язык",
  ];

  const contentTypes = [
    "Лекции",
    "Экзамены",
    "Контрольные",
    "Практики",
    "Коллоквиумы",
  ];

  const years = ["2025", "2024", "2023", "2022", "2021"];

  const semesters = ["1", "2", "3", "4", "5", "6", "7", "8"];

  const toggleFilter = (
    category: keyof FilterState,
    value: string
  ) => {
    setFilters((prev) => ({
      ...prev,
      [category]: prev[category].includes(value)
        ? prev[category].filter((item) => item !== value)
        : [...prev[category], value],
    }));
  };

  const getFilterCount = () => {
    return (
      filters.subjects.length +
      filters.contentTypes.length +
      filters.years.length +
      filters.semesters.length
    );
  };

  const resetFilters = () => {
    setFilters({
      subjects: [],
      contentTypes: [],
      years: [],
      semesters: [],
    });
  };

  const saveFilters = () => {
    
  }

  const getButtonStyle = (
    category: keyof FilterState,
    value: string,
    isHovered?: boolean
  ) => {
    const isSelected = filters[category].includes(value);
    
    if (isSelected) {
      return "bg-purple-dark text-purple-light";
    }
    
    if (isHovered) {
      return "bg-purple-light/77 text-purple-dark";
    }
    
    return "bg-white/50 text-purple-dark";
  };

  return (
    <div className="h-full bg-purple-light p-4 sm:p-6 md:p-8 lg:p-10 flex items-start justify-center">
      <div className="w-full max-w-[1200px]">
        <div className="flex flex-col lg:flex-row gap-4 mb-6">
          <div className="flex-1 flex items-center gap-2.5 px-4 py-2.5 rounded-[30px] border-2 border-purple-dark bg-white/50">
            <Search className="w-10 h-10 text-purple-dark flex-shrink-0" strokeWidth={2} />
            <input
              type="text"
              placeholder="Search"
              className="flex-1 bg-transparent text-xl text-purple-dark/75 placeholder:text-purple-dark/75 outline-none font-normal"
            />
          </div>

          <div className="lg:w-40 flex items-center justify-between px-2.5 py-2.5 rounded-[30px] bg-purple-medium gap-2">
            <Filter className="w-10 h-10 text-purple-dark flex-shrink-0" strokeWidth={2} />
            <div className="relative flex-shrink-0">
              <div className="w-10 h-10 rounded-full bg-white/50 flex items-center justify-center">
                <span className="text-purple-dark/75 text-2xl font-medium">
                  {getFilterCount()}
                </span>
              </div>
            </div>
            <button 
              onClick={saveFilters}
              className="flex-shrink-0 hover:opacity-70 transition-opacity"
            >
              <CheckIcon className="w-10 h-10 text-purple-dark" strokeWidth={2} />
            </button>
          </div>
        </div>

        <div className="relative bg-purple-medium rounded-[20px] p-6 sm:p-8 md:p-10">
          <div className="space-y-8">
            <div className="space-y-4">
              <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                Subject
              </h2>
              <div className="h-px w-full bg-purple-dark"></div>
              <div className="space-y-1.5">
                <div className="flex flex-wrap gap-2">
                  {subjects.slice(0, 5).map((subject) => (
                    <button
                      key={subject}
                      onClick={() => toggleFilter("subjects", subject)}
                      className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                        "subjects",
                        subject
                      )}`}
                    >
                      {subject}
                    </button>
                  ))}
                </div>
                <div className="flex flex-wrap gap-2">
                  {subjects.slice(5).map((subject) => (
                    <button
                      key={subject}
                      onClick={() => toggleFilter("subjects", subject)}
                      className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                        "subjects",
                        subject
                      )}`}
                    >
                      {subject}
                    </button>
                  ))}
                </div>
              </div>
            </div>

            <div className="space-y-4">
              <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                Type of content
              </h2>
              <div className="h-px w-full bg-purple-dark"></div>
              <div className="flex flex-wrap gap-2">
                {contentTypes.map((type) => (
                  <button
                    key={type}
                    onClick={() => toggleFilter("contentTypes", type)}
                    className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                      "contentTypes",
                      type
                    )}`}
                  >
                    {type}
                  </button>
                ))}
              </div>
            </div>

            <div className="space-y-4">
              <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                Year
              </h2>
              <div className="h-px w-full bg-purple-dark"></div>
              <div className="flex flex-wrap gap-2">
                {years.map((year) => (
                  <button
                    key={year}
                    onClick={() => toggleFilter("years", year)}
                    className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                      "years",
                      year
                    )}`}
                  >
                    {year}
                  </button>
                ))}
              </div>
            </div>

            <div className="space-y-4">
              <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                Semester
              </h2>
              <div className="h-px w-full bg-purple-dark"></div>
              <div className="flex flex-wrap gap-2">
                {semesters.map((semester) => (
                  <button
                    key={semester}
                    onClick={() => toggleFilter("semesters", semester)}
                    className={`px-2.5 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                      "semesters",
                      semester
                    )}`}
                  >
                    {semester}
                  </button>
                ))}
              </div>
            </div>
          </div>

          <button
            onClick={resetFilters}
            className="absolute bottom-6 right-6 hover:opacity-70 transition-opacity"
            aria-label="Reset filters"
          >
            <RefreshCcw className="w-12 h-12 text-purple-dark" strokeWidth={2} />
          </button>
        </div>
      </div>
    </div>
  );
}
