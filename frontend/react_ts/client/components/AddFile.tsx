import { ChevronLeft, ChevronDown, FilePlus2 } from "lucide-react";
import { useState } from "react";
import { useUploadMaterialMutation } from "../../api/api";
import { UploadMaterialRequest } from "../../api/types";
import { useNavigate } from "react-router-dom";

export default function Index() {
  const navigate = useNavigate();
  const [dragActive, setDragActive] = useState(false);
  const [subject, setSubject] = useState("");
  const [year, setYear] = useState("");
  const [semester, setSemester] = useState("");
  const [contentType, setContentType] = useState("");
  const [description, setDescription] = useState("");
  const [files, setFiles] = useState<File | null>(null);

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
        subjectId: subject, // assuming subject state now holds the ID from a real list if we implement it, or we use the old logic for now
        year: parseInt(year, 10),
        materialType: contentType,
        description: description,
        semester: semester ? parseInt(semester, 10) : undefined,
      };
      await uploadMaterial(request).unwrap();
      alert("Успешно загружено!");
      navigate(-1);
    } catch (err) {
      console.error("Upload failed:", err);
      alert("Ошибка при загрузке. Проверьте авторизацию.");
    }
  };

  return (
    <div className="flex items-center justify-center p-4 min-h-screen bg-app-bg">
      <div className="w-full bg-white/100 rounded-[20px] shadow-2xl p-6 md:p-8 lg:p-10 max-w-7xl">
        <button
          onClick={() => navigate(-1)}
          className="mb-6 flex items-center text-primary-dark hover:opacity-70"
        >
          <ChevronLeft className="w-6 h-6" />
          Назад
        </button>
        <div className="flex flex-col lg:flex-row gap-8 lg:gap-12">
          {/* Left Section - Form */}
          <div className="flex-1 space-y-12">
            {/* Form Fields */}
            <div className="space-y-6">
              {/* Subject */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-xl font-normal">
                  Предмет
                </label>
                <div className="relative">
                  <select
                    value={subject}
                    onChange={(e) => setSubject(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                  >
                    <option value="" className="text-primary-dark/50">
                      Выберите предмет
                    </option>
                    <option value="96924d67-6cd8-4720-942f-87034c29cf46">теория вероятности</option>
                    <option value="algebra">алгебра</option>
                    <option value="calculus">математический анализ</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Year */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-xl font-normal">
                  Год
                </label>
                <div className="relative">
                  <select
                    value={year}
                    onChange={(e) => setYear(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                  >
                    <option value="" className="text-primary-dark/50">
                      Выберите год
                    </option>
                    <option value="2024">2024</option>
                    <option value="2023">2023</option>
                    <option value="2022">2022</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Semester */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-xl font-normal">
                  Семестр
                </label>
                <div className="relative">
                  <select
                    value={semester}
                    onChange={(e) => setSemester(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                  >
                    <option value="" className="text-primary-dark/50">
                      Выберите семестр
                    </option>
                    <option value="1">1 семестр</option>
                    <option value="2">2 семестр</option>
                    <option value="3">3 семестр</option>
                    <option value="4">4 семестр</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Type of content */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-xl font-normal">
                  Тип контента
                </label>
                <div className="relative">
                  <select
                    value={contentType}
                    onChange={(e) => setContentType(e.target.value)}
                    className="w-full h-[45px] px-4 pr-12 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-xl appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                  >
                    <option value="" className="text-primary-dark/50">
                      Выберите тип
                    </option>
                    <option value="Colloquium">коллоквиум</option>
                    <option value="Exam">экзамен</option>
                    <option value="ControlWork">контрольная</option>
                    <option value="ComputerPractice">практикум</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-primary-dark pointer-events-none" />
                </div>
              </div>

              {/* Description */}
              <div className="space-y-2">
                <label className="block text-primary-dark text-xl font-normal">
                  Описание
                </label>
                <textarea
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  placeholder="Дополнительная информация"
                  className="w-full h-32 px-4 py-3 rounded-[10px] bg-[rgba(228,183,245,0.36)] text-primary-dark text-xl placeholder:text-primary-dark/50 resize-none focus:outline-none focus:ring-2 focus:ring-primary-dark/20"
                />
              </div>
            </div>

            {/* Upload Button */}
            <div className="flex justify-center">
              <button
                onClick={handleUpload}
                disabled={isLoading}
                className="px-8 h-[45px] rounded-[20px] border-2 border-primary-light/36 bg-white/5 text-primary-dark text-2xl font-medium shadow-[inset_-2px_-2px_4px_rgba(255,255,255,0.3),inset_5px_5px_25px_rgba(255,255,255,0.4),0_10px_20px_rgba(42,42,42,0.36),inset_2px_2px_4px_#fff] hover:bg-white/10 transition-colors disabled:opacity-50"
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
              className={`w-full max-w-xl h-full min-h-[400px] lg:min-h-[700px] flex flex-col items-center justify-center gap-8 px-8 md:px-24 rounded-[15px] border-2 border-dashed border-primary-dark bg-primary-light/36 shadow-[inset_-2px_-2px_4px_rgba(0,0,0,0.25),inset_5px_5px_25px_rgba(228,183,245,0.6),inset_2px_2px_4px_#E4B7F5,0_4px_20px_#E4B7F5] transition-all ${dragActive ? "scale-[1.02] border-primary-dark/80" : ""
                }`}
            >
              <FilePlus2 className="w-20 h-20 text-primary-dark stroke-[2]" />

              <div className="flex flex-col items-center gap-7">
                <p className="text-primary-dark text-center text-2xl font-medium tracking-[0.25px]">
                  Кидай сюда или
                </p>
                <label className="px-4 py-2 rounded-[3px] bg-primary-dark text-white text-2xl font-medium tracking-[0.25px] cursor-pointer hover:bg-primary-dark/90 transition-colors">
                  выбрать файл
                  <input
                    type="file"
                    onChange={handleFileChange}
                    className="hidden"
                  />
                </label>
              </div>

              {files && (
                <div className="mt-4 text-primary-dark text-lg font-medium bg-white/50 px-4 py-2 rounded">
                  {files.name} selected
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
