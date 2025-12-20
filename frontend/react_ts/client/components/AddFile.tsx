import { ChevronDown, FilePlus2, ChevronLeft } from "lucide-react";
import { useState } from "react";
import { useUploadMaterialMutation, useGetSubjectsQuery } from "../../api/api";
import { UploadMaterialRequest } from "../../api/types";
import { useNavigate } from "react-router-dom";

export default function Index() {
  const navigate = useNavigate();
  const [dragActive, setDragActive] = useState(false);
  const [subjectId, setSubjectId] = useState("");
  const [year, setYear] = useState(new Date().getFullYear().toString());
  const [semester, setSemester] = useState("");
  const [contentType, setContentType] = useState("");
  const [description, setDescription] = useState("");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const { data: subjects, isLoading: isLoadingSubjects } = useGetSubjectsQuery();
  const [uploadMaterial, { isLoading: isUploading }] = useUploadMaterialMutation();

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
      setSelectedFile(e.dataTransfer.files[0]);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setSelectedFile(e.target.files[0]);
    }
  };

  const handleUpload = async () => {
    if (!selectedFile || !subjectId || !year || !contentType) {
      alert("Пожалуйста, заполните все обязательные поля и выберите файл.");
      return;
    }

    try {
      const request: UploadMaterialRequest = {
        file: selectedFile,
        subjectId: subjectId,
        year: parseInt(year, 10),
        materialType: contentType,
        description: description,
        semester: semester ? parseInt(semester, 10) : undefined,
      };
      await uploadMaterial(request).unwrap();
      alert("Файл успешно загружен!");
      navigate(-1);
    } catch (err) {
      console.error("Upload failed:", err);
      alert("Ошибка при загрузке файла.");
    }
  };

  return (
    <div className="flex items-center justify-center p-4 bg-app-bg min-h-screen">
      <div className="w-full bg-white rounded-[20px] shadow-2xl p-6 md:p-8 lg:p-10 max-w-6xl">
        <button
          onClick={() => navigate(-1)}
          className="mb-6 flex items-center text-primary-dark hover:opacity-70"
        >
          <ChevronLeft className="w-6 h-6" />
          Назад
        </button>

        <div className="flex flex-col lg:flex-row gap-8 lg:gap-12">
          {/* Left Section - Form */}
          <div className="flex-1 space-y-6">
            <div className="space-y-4">
              {/* Subject */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-lg font-medium">Предмет</label>
                <div className="relative">
                  <select
                    value={subjectId}
                    onChange={(e) => setSubjectId(e.target.value)}
                    disabled={isLoadingSubjects}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-lg appearance-none cursor-pointer focus:outline-none focus:ring-2"
                  >
                    <option value="">Выберите предмет</option>
                    {subjects?.map(s => (
                      <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-6 h-6 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Year & Semester Row */}
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <label className="block text-primary-dark text-lg font-medium">Год</label>
                  <input
                    type="number"
                    value={year}
                    onChange={(e) => setYear(e.target.value)}
                    className="w-full h-[45px] px-4 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-lg focus:outline-none focus:ring-2"
                  />
                </div>
                <div className="space-y-2">
                  <label className="block text-primary-dark text-lg font-medium">Семестр</label>
                  <select
                    value={semester}
                    onChange={(e) => setSemester(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-lg appearance-none focus:outline-none focus:ring-2"
                  >
                    <option value="">Не указан</option>
                    <option value="1">1</option>
                    <option value="2">2</option>
                  </select>
                </div>
              </div>

              {/* Material Type */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-lg font-medium">Тип контента</label>
                <div className="relative">
                  <select
                    value={contentType}
                    onChange={(e) => setContentType(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-lg appearance-none cursor-pointer focus:outline-none focus:ring-2"
                  >
                    <option value="">Выберите тип</option>
                    <option value="Exam">Экзамен</option>
                    <option value="Colloquium">Коллоквиум</option>
                    <option value="ControlWork">Контрольная работа</option>
                    <option value="Pass">Зачёт</option>
                    <option value="ComputerPractice">Практикум</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-6 h-6 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Description */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-lg font-medium">Описание</label>
                <textarea
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  placeholder="Дополнительная информация (опционально)"
                  className="w-full h-24 px-4 py-3 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-lg placeholder:text-primary-dark/50 resize-none focus:outline-none focus:ring-2"
                />
              </div>
            </div>

            <button
              onClick={handleUpload}
              disabled={isUploading}
              className="w-full px-8 h-[50px] rounded-[20px] bg-primary-dark text-white text-xl font-medium hover:bg-primary-dark/90 transition-colors disabled:opacity-50"
            >
              {isUploading ? "Загрузка..." : "Загрузить"}
            </button>
          </div>

          {/* Right Section - File Drop Zone */}
          <div className="flex-1 flex flex-col gap-4">
            <label className="block text-primary-dark text-lg font-medium">Файл</label>
            <div
              onDragEnter={handleDrag}
              onDragLeave={handleDrag}
              onDragOver={handleDrag}
              onDrop={handleDrop}
              className={`flex-1 min-h-[300px] border-2 border-dashed border-primary-dark rounded-[15px] bg-[rgba(228,183,245,0.1)] flex flex-col items-center justify-center gap-4 transition-all ${dragActive ? "bg-[rgba(228,183,245,0.2)]" : ""
                }`}
            >
              <FilePlus2 className="w-16 h-16 text-primary-dark" />
              <div className="text-center">
                <p className="text-primary-dark text-lg font-medium">Перетащите файл сюда или</p>
                <label className="mt-2 inline-block px-4 py-2 bg-primary-dark text-white rounded cursor-pointer hover:bg-opacity-90">
                  выберите на устройстве
                  <input type="file" onChange={handleFileChange} className="hidden" />
                </label>
              </div>
              {selectedFile && (
                <div className="mt-4 px-4 py-2 bg-white rounded-lg shadow-sm border border-primary-dark/20 flex items-center gap-2">
                  <span className="text-primary-dark font-medium truncate max-w-[200px]">{selectedFile.name}</span>
                  <span className="text-sm text-gray-500">({(selectedFile.size / 1024 / 1024).toFixed(2)} MB)</span>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
