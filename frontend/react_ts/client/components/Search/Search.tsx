import { useState, useMemo, useEffect } from "react";
import { Search, Filter, CheckIcon, RefreshCcw } from "lucide-react";
import SearchResult from "./SearchResult";
import { useGetMaterialsQuery, useGetSubjectsQuery } from "../../../api/api";
import { GetMaterialsParams, SubjectDto } from "../../../api/types";

interface FilterState {
  subjects: string[]; // Subject IDs
  contentTypes: string[];
  years: string[];
  semesters: string[];
}

export default function SearchMenu() {
  const [searchQuery, setSearchQuery] = useState("");
  const [debouncedSearchQuery, setDebouncedSearchQuery] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedSearchQuery(searchQuery);
    }, 500);

    return () => {
      clearTimeout(handler);
    };
  }, [searchQuery]);

  // Auto-switch to results when search query is present
  useEffect(() => {
    if (debouncedSearchQuery.trim().length > 0) {
      setShowFilters(false);
    }
  }, [debouncedSearchQuery]);

  const [filters, setFilters] = useState<FilterState>({
    subjects: [],
    contentTypes: [],
    years: [],
    semesters: [],
  });

  const [showFilters, setShowFilters] = useState(true);

  // Fetch subjects
  const { data: subjectsData = [] } = useGetSubjectsQuery();

  // Prepare query params
  // Strategy: If single value selected, pass to backend for optimization.
  // Otherwise pass null and filter on client.
  const queryParams: GetMaterialsParams = {
    searchQuery: debouncedSearchQuery || undefined,
    subjectId: filters.subjects.length === 1 ? filters.subjects[0] : undefined,
    year: filters.years.length === 1 ? parseInt(filters.years[0]) : undefined,
    semester: filters.semesters.length === 1 ? parseInt(filters.semesters[0]) : undefined,
    materialType: filters.contentTypes.length === 1 ? filters.contentTypes[0] : undefined
  };

  const { data: materials = [], isLoading } = useGetMaterialsQuery(queryParams);

  // Client-side filtering for multiple selections case or mixed scenarios
  const filteredMaterials = useMemo(() => {
    return materials.filter(material => {
      // Subject Filter
      if (filters.subjects.length > 0 && !filters.subjects.includes(material.subjectId)) {
        return false;
      }
      // Content Type Filter (MaterialType is string in DTO)
      if (filters.contentTypes.length > 0 && !filters.contentTypes.some(t => material.materialType.toLowerCase() === t.toLowerCase())) {
        // Backend might return "Exam", "Lection". Filter UI has Russian names? 
        // Need to map UI names to backend enum string values.
        // Let's check logic below.
        // Actually, let's look at what we are putting in `filters.contentTypes`.
        return false;
      }
      // Year Filter
      if (filters.years.length > 0 && !filters.years.includes(material.year.toString())) {
        return false;
      }
      // Semester Filter
      if (filters.semesters.length > 0 && !filters.semesters.includes(material.semester.toString())) {
        return false;
      }
      return true;
    });
  }, [materials, filters]);

  // Mapping for Content Types (UI Display -> Backend Enum)
  // Backend types: Exam, Colloquium, Pass, ControlWork, ComputerPractice
  // UI types: "Экзамены" -> "Exam", etc.

  const contentTypeMap: Record<string, string> = {
    "Exam": "Экзамены",
    "Colloquium": "Коллоквиумы",
    "Pass": "Зачёты",
    "ControlWork": "Контрольные",
    "ComputerPractice": "Практики"
    // What about "Лекции"? Not in backend enum MaterialType.cs?
    // Enum: Exam, Colloquium, Pass, ControlWork, ComputerPractice.
    // So "Лекции" might not exist on backend yet! I will omit it or map strictly.
  };

  const invertedContentTypeMap: Record<string, string> = Object.fromEntries(
    Object.entries(contentTypeMap).map(([k, v]) => [v, k])
  );

  const uiContentTypes = Object.values(contentTypeMap);
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
    setShowFilters(false);
  };

  const toggleFilters = () => {
    setShowFilters(!showFilters);
  };

  const getButtonStyle = (
    category: keyof FilterState,
    value: string,
    isHovered?: boolean
  ) => {
    const isSelected = filters[category].includes(value);

    if (isSelected) {
      return "bg-purple-dark text-purple-light";
    }

    // Hover handling in React is separate, so just return base style
    return "bg-white/50 text-purple-dark";
  };

  // Helper to get Backend Value for ContentType filter
  const handleContentTypeToggle = (uiValue: string) => {
    // Convert "Экзамены" -> "Exam"
    const backendValue = invertedContentTypeMap[uiValue];
    if (backendValue) toggleFilter("contentTypes", backendValue);
  };

  return (
    <div className="h-full max-h-8xl bg-purple-light p-4 sm:p-6 md:p-8 lg:p-10 flex items-start justify-center overflow-hidden">
      <div className="w-full max-w-[1200px]">
        <div className="flex flex-col lg:flex-row gap-4 mb-6">
          <div className="flex-1 flex items-center gap-2.5 px-4 py-2.5 rounded-[30px] border-2 border-purple-dark bg-white/50">
            <Search className="w-10 h-10 text-purple-dark flex-shrink-0" strokeWidth={2} />
            <input
              type="text"
              placeholder="Search"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="flex-1 bg-transparent text-xl text-purple-dark/75 placeholder:text-purple-dark/75 outline-none font-normal"
            />
          </div>

          <div className="lg:w-40 flex items-center justify-between px-2.5 py-2.5 rounded-[30px] bg-purple-medium gap-2">
            <button
              onClick={toggleFilters}
              className="flex items-center gap-2.5 hover:opacity-70 transition-opacity"
            >
              <Filter className="w-10 h-10 text-purple-dark flex-shrink-0" strokeWidth={2} />
              <div className="relative flex-shrink-0">
                <div className="w-10 h-10 rounded-full bg-white/50 flex items-center justify-center">
                  <span className="text-purple-dark/75 text-2xl font-medium">
                    {getFilterCount()}
                  </span>
                </div>
              </div>
            </button>
            <button
              onClick={saveFilters}
              className="flex-shrink-0 hover:opacity-70 transition-opacity"
            >
              <CheckIcon className="w-10 h-10 text-purple-dark" strokeWidth={2} />
            </button>
          </div>
        </div>

        {/* Условный рендеринг окна фильтров */}
        {showFilters && (
          <div className="relative bg-purple-medium rounded-[20px] p-6 sm:p-8 md:p-10">
            <div className="space-y-8">
              <div className="space-y-4">
                <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                  Предмет
                </h2>
                <div className="h-px w-full bg-purple-dark"></div>
                <div className="space-y-1.5">
                  <div className="flex flex-wrap gap-2">
                    {subjectsData.map((subject: SubjectDto) => (
                      <button
                        key={subject.id}
                        onClick={() => toggleFilter("subjects", subject.id)}
                        className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                          "subjects",
                          subject.id
                        )}`}
                      >
                        {subject.name}
                      </button>
                    ))}
                  </div>
                </div>
              </div>

              <div className="space-y-4">
                <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                  Тип материала
                </h2>
                <div className="h-px w-full bg-purple-dark"></div>
                <div className="flex flex-wrap gap-2">
                  {uiContentTypes.map((type) => (
                    <button
                      key={type}
                      onClick={() => handleContentTypeToggle(type)}
                      className={`px-2 py-1.5 rounded-[20px] text-xl font-normal transition-all hover:bg-purple-light/77 ${getButtonStyle(
                        "contentTypes",
                        invertedContentTypeMap[type]
                      )}`}
                    >
                      {type}
                    </button>
                  ))}
                </div>
              </div>

              <div className="space-y-4">
                <h2 className="text-2xl font-medium text-purple-dark tracking-wide">
                  Год
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
                  Семестр
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
        )}

        {!showFilters && (<SearchResult items={filteredMaterials} />)}
      </div>
    </div>
  );
}