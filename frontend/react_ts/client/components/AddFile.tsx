import { ChevronDown, FilePlus2, X } from "lucide-react";
import { useState, useMemo } from "react";
import { useUploadMaterialMutation, useGetSubjectsQuery } from "../../api/api";
import { UploadMaterialRequest } from "../../api/types";
import { useNavigate } from "react-router-dom";

interface SubjectDto {
  id: string;
  name: string;
  semester: number;
  materialTypes: { value: string; displayName: string }[];
}

export default function Index() {
  const navigate = useNavigate();
  const [dragActive, setDragActive] = useState(false);
  const [selectedSubjectName, setSelectedSubjectName] = useState("");
  const [subject, setSubject] = useState(""); // This will be the actual subjectId
  const [year, setYear] = useState("");
  const [semester, setSemester] = useState("");
  const [contentType, setContentType] = useState("");
  const [description, setDescription] = useState("");
  const [files, setFiles] = useState<File | null>(null);
  const token = localStorage.getItem("token");

  if (!token) {
    return (
      <div className="flex flex-col items-center justify-center h-full min-h-[50vh] gap-6 text-center">
        <p className="text-2xl font-medium text-primary-dark">
          Пожалуйста, войдите в аккаунт, чтобы добавить файл
        </p>
        <button
          onClick={() => navigate("/login")}
          className="px-8 py-3 rounded-[15px] bg-primary-dark text-white text-xl font-bold hover:opacity-90 transition-opacity"
        >
          Войти
        </button>
      </div>
    );
  }

  const { data: subjects = [] } = useGetSubjectsQuery() as { data: SubjectDto[] };

  // Get unique subject names (no duplicates)
  const uniqueSubjectNames = useMemo(() => {
    const names = subjects.map((s) => s.name);
    return [...new Set(names)];
  }, [subjects]);

  // Get all subjects with the selected name (for material types)
  const selectedSubjects = useMemo(() => {
    return subjects.filter((s) => s.name === selectedSubjectName);
  }, [subjects, selectedSubjectName]);

  // Get unique material types for the selected subject name (merged from all semesters)
  const availableMaterialTypes = useMemo(() => {
    const typeMap = new Map<string, string>();
    selectedSubjects.forEach((s) => {
      s.materialTypes.forEach((t) => {
        if (!typeMap.has(t.value)) {
          typeMap.set(t.value, t.displayName);
        }
      });
    });
    return Array.from(typeMap.entries()).map(([value, displayName]) => ({
      value,
      displayName,
    }));
  }, [selectedSubjects]);

  // Get available semesters for the selected subject name
  const availableSemesters = useMemo(() => {
    return selectedSubjects.map((s) => s.semester).sort((a, b) => a - b);
  }, [selectedSubjects]);

  // Handle subject name selection
  const handleSubjectNameChange = (name: string) => {
    setSelectedSubjectName(name);
    setContentType(""); // Reset content type
    setSemester(""); // Reset semester
    setSubject(""); // Reset subject ID
  };

  // Handle semester selection - set the actual subject ID
  const handleSemesterChange = (sem: string) => {
    setSemester(sem);
    const matchingSubject = subjects.find(
      (s) => s.name === selectedSubjectName && s.semester === parseInt(sem, 10)
    );
    if (matchingSubject) {
      setSubject(matchingSubject.id);
    }
  };

  const handleDrag = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (e.type === "dragenter" || e.type === "dragover") {
      setDragActive(true);
    } else if (e.type === "dragleave") {
      setDragActive(false);
    }
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
    if (e.dataTransfer.files && e.dataTransfer.files[0]) {
      setFiles(e.dataTransfer.files[0]);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFiles(e.target.files[0]);
    }
  };

  const [uploadMaterial, { isLoading }] = useUploadMaterialMutation();
  const handleUpload = async () => {
    if (!files || !subject || !year || !contentType) {
      alert("Заполните все поля и выберите файл");
      return;
    }
    try {
      const request: UploadMaterialRequest = {
        file: files,
        subjectId: subject,
        year: parseInt(year, 10),
        materialType: contentType,
        description: description,
        semester: semester ? parseInt(semester, 10) : 1, // Providing default 1 if not selected, as backend might require it
      };
      await uploadMaterial(request).unwrap();
      alert("Успешно загружено!");
      navigate(-1);
    } catch (err: any) {
      console.error("Upload failed:", err);
      let errorMessage = "Ошибка при загрузке. Проверьте заполнение всех полей.";

      if (err.data && Array.isArray(err.data)) {
        errorMessage = err.data.map((e: any) => e.errorMessage).join("\n");
      } else if (err.data?.message) {
        errorMessage = err.data.message;
      }

      alert(errorMessage);
    }
  };

  return (
    <div className="flex items-center justify-center p-2 md:p-4 ">
      <div className="w-full rounded-[20px] p-4 md:p-8 lg:p-10 relative">
        <button
          onClick={() => navigate(-1)}
          className="absolute top-2 right-2 md:top-4 md:right-4 p-2 hover:bg-primary-dark/10 rounded-full transition-colors z-10"
          aria-label="Close"
        >
          <X className="w-8 h-8 text-primary-dark" />
        </button>
        <div className="flex flex-col lg:flex-row gap-8 lg:gap-12">
          {/* Left Section - Form */}
          <div className="flex-1 space-y-12">
            {/* Form Fields */}
            <div className="space-y-6">
              {/* Subject */}
              <label className="block text-primary-dark text-lg md:text-xl font-normal">
                Предмет
              </label>
              <div className="relative">
                <select
                  value={selectedSubjectName}
                  onChange={(e) => handleSubjectNameChange(e.target.value)}
                  className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-base md:text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                >
                  <option value="" className="text-primary-dark/50">
                    Выберите предмет
                  </option>
                  {uniqueSubjectNames.map((name) => (
                    <option key={name} value={name}>
                      {name}
                    </option>
                  ))}
                </select>
                <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
              </div>
            </div>

            {/* Year */}
            <div className="space-y-2">
              <label className="block text-primary-dark text-lg md:text-xl font-normal">
                Год
              </label>
              <div className="relative">
                <select
                  value={year}
                  onChange={(e) => setYear(e.target.value)}
                  className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-base md:text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                >
                  <option value="" className="text-primary-dark/50">
                    Выберите год
                  </option>
                  <option value="2025">2025</option>
                  <option value="2024">2024</option>
                  <option value="2023">2023</option>
                  <option value="2022">2022</option>
                </select>
                <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
              </div>
            </div>

            {/* Semester */}
            <div className="space-y-2">
              <label className="block text-primary-dark text-lg md:text-xl font-normal">
                Семестр
              </label>
              <div className="relative">
                <select
                  value={semester}
                  onChange={(e) => handleSemesterChange(e.target.value)}
                  disabled={!selectedSubjectName}
                  className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-base md:text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <option value="" className="text-primary-dark/50">
                    {selectedSubjectName ? "Выберите семестр" : "Сначала выберите предмет"}
                  </option>
                  {availableSemesters.map((sem) => (
                    <option key={sem} value={sem.toString()}>
                      {sem} семестр
                    </option>
                  ))}
                </select>
                <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
              </div>
            </div>

            {/* Type of content */}
            <div className="space-y-2">
              <label className="block text-primary-dark text-lg md:text-xl font-normal">
                Тип контента
              </label>
              <div className="relative">
                <select
                  value={contentType}
                  onChange={(e) => setContentType(e.target.value)}
                  disabled={!selectedSubjectName}
                  className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-base md:text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <option value="" className="text-primary-dark/50">
                    {selectedSubjectName ? "Выберите тип" : "Сначала выберите предмет"}
                  </option>
                  {availableMaterialTypes.map((type) => (
                    <option key={type.value} value={type.value}>
                      {type.displayName}
                    </option>
                  ))}
                </select>
                <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
              </div>
            </div>

            {/* Description */}
            <div className="space-y-2">
              <label className="block text-primary-dark text-lg md:text-xl font-normal">
                Описание
              </label>
              <textarea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Дополнительная информация"
                className="w-full h-32 px-4 py-3 rounded-[10px] bg-[rgba(228,183,245,0.3)] text-primary-dark text-base md:text-xl placeholder:text-primary-dark/50 resize-none focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
              />
            </div>

            {/* Upload Button */}
            <div className="flex justify-center">
              <button
                onClick={handleUpload}
                disabled={isLoading}
                className="px-8 h-[55px] min-w-[200px] rounded-[20px] bg-primary-dark text-white text-xl md:text-2xl font-bold shadow-lg hover:scale-[1.02] active:scale-[0.98] transition-all disabled:opacity-50"
              >
                {isLoading ? "Загрузка..." : "Загрузить"}
              </button>
            </div>
          </div>

          {/* Right Section - File Drop Zone */}
          <div className="flex-1 flex items-center justify-center lg:min-h-[700px]">
            <div
              onDragEnter={handleDrag}
              onDragLeave={handleDrag}
              onDragOver={handleDrag}
              onDrop={handleDrop}
              className={`w-full max-w-xl h-full min-h-[400px] lg:min-h-[700px] flex flex-col items-center justify-center gap-8 px-8 md:px-24 rounded-[25px] border-2 border-dashed border-primary-dark bg-primary-dark/[0.04] transition-all ${dragActive ? "scale-[1.02] border-primary-dark/80 bg-primary-dark/[0.08]" : ""
                }`}
            >
              <FilePlus2 className="w-20 h-20 text-primary-dark stroke-[1.5]" />

              <div className="flex flex-col items-center gap-7">
                <p className="text-primary-dark text-center text-xl md:text-2xl font-medium tracking-[0.25px]">
                  Кидай сюда или
                </p>
                <label className="px-6 py-3 rounded-[15px] bg-primary-dark text-white text-xl md:text-2xl font-bold tracking-[0.25px] cursor-pointer hover:bg-primary-dark/90 transition-all active:scale-[0.95]">
                  выбрать файл
                  <input
                    type="file"
                    onChange={handleFileChange}
                    accept=".pdf,.doc,.docx,.xls,.xlsx,.png,.jpg,.jpeg,.ppt,.pptx,.txt,.md"
                    className="hidden"
                  />
                </label>
              </div>

              {files && (
                <div className="mt-4 text-primary-dark text-lg font-medium bg-primary-dark/10 px-6 py-3 rounded-[12px] animate-in fade-in zoom-in duration-300">
                  {files.name}
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>

  );
}
