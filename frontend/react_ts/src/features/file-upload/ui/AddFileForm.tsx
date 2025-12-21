import { useState } from "react";
import { ChevronDown, ChevronLeft, FilePlus } from "lucide-react";

const AddFileForm = () => {
  const [formData, setFormData] = useState({
    subject: "",
    year: "",
    semester: "",
    contentType: "",
    description: "",
  });
  const [dragActive, setDragActive] = useState(false);
  const [files, setFiles] = useState<File[]>([]);

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
      setFiles(Array.from(e.dataTransfer.files));
    }
  };

  const handleFileInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFiles(Array.from(e.target.files));
    }
  };

  const handleSubmit = () => {
    console.log("Form submitted:", { formData, files });
  };

  return (
    <div className="min-h-screen w-full bg-gradient-to-br from-purple-50 via-pink-50 to-blue-50 flex items-center justify-center p-4 md:p-6 lg:p-8">
      <div className="w-full max-w-[1169px] bg-white/95 rounded-[20px] shadow-2xl backdrop-blur-sm p-4 md:p-6 lg:p-8">
        <div className="flex flex-col lg:flex-row gap-6 lg:gap-8">
          {/* Left Section - Form */}
          <div className="flex-1 flex flex-col gap-12 lg:gap-20">
            {/* Back Button */}
            <button 
              className="w-[55px] h-[55px] flex items-center justify-center hover:bg-purple-50 rounded-full transition-colors"
              onClick={() => window.history.back()}
            >
              <ChevronLeft className="w-[55px] h-[55px] text-[#1E265A]" strokeWidth={2} />
            </button>

            {/* Form Fields */}
            <div className="flex flex-col gap-[15px]">
              {/* Subject */}
              <div className="flex flex-col gap-[2px]">
                <label className="text-[#1E265A] text-[20px] font-normal leading-normal">
                  Subject
                </label>
                <div className="relative">
                  <select
                    value={formData.subject}
                    onChange={(e) => setFormData({ ...formData, subject: e.target.value })}
                    className="w-full h-[45px] px-[9px] py-[13px] rounded-[10px] bg-[#E4B7F5]/36 text-[#1E265A]/49 text-[20px] font-normal appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-[#1E265A]/20"
                  >
                    <option value="">e.g. теория вероятности</option>
                    <option value="theory">Теория вероятности</option>
                    <option value="algebra">Алгебра</option>
                    <option value="calculus">Математический анализ</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-[#1E265A] pointer-events-none" strokeWidth={2} />
                </div>
              </div>

              {/* Year */}
              <div className="flex flex-col gap-[2px]">
                <label className="text-[#1E265A] text-[20px] font-normal leading-normal">
                  Year
                </label>
                <div className="relative">
                  <select
                    value={formData.year}
                    onChange={(e) => setFormData({ ...formData, year: e.target.value })}
                    className="w-full h-[45px] px-[9px] py-[13px] rounded-[10px] bg-[#E4B7F5]/36 text-[#1E265A]/49 text-[20px] font-normal appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-[#1E265A]/20"
                  >
                    <option value="">e.g. 2024</option>
                    <option value="2024">2024</option>
                    <option value="2023">2023</option>
                    <option value="2022">2022</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-[#1E265A] pointer-events-none" strokeWidth={2} />
                </div>
              </div>

              {/* Semester */}
              <div className="flex flex-col gap-[2px]">
                <label className="text-[#1E265A] text-[20px] font-normal leading-normal">
                  Semester
                </label>
                <div className="relative">
                  <select
                    value={formData.semester}
                    onChange={(e) => setFormData({ ...formData, semester: e.target.value })}
                    className="w-full h-[45px] px-[9px] py-[13px] rounded-[10px] bg-[#E4B7F5]/36 text-[#1E265A]/49 text-[20px] font-normal appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-[#1E265A]/20"
                  >
                    <option value="">e.g. 3 семестр</option>
                    <option value="1">1 семестр</option>
                    <option value="2">2 семестр</option>
                    <option value="3">3 семестр</option>
                    <option value="4">4 семестр</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-[#1E265A] pointer-events-none" strokeWidth={2} />
                </div>
              </div>

              {/* Type of content */}
              <div className="flex flex-col gap-[2px]">
                <label className="text-[#1E265A] text-[20px] font-normal leading-normal">
                  Type of content
                </label>
                <div className="relative">
                  <select
                    value={formData.contentType}
                    onChange={(e) => setFormData({ ...formData, contentType: e.target.value })}
                    className="w-full h-[45px] px-[9px] py-[13px] rounded-[10px] bg-[#E4B7F5]/36 text-[#1E265A]/49 text-[20px] font-normal appearance-none cursor-pointer focus:outline-none focus:ring-2 focus:ring-[#1E265A]/20"
                  >
                    <option value="">e.g. коллоквиум</option>
                    <option value="colloquium">Коллоквиум</option>
                    <option value="exam">Экзамен</option>
                    <option value="lecture">Лекция</option>
                  </select>
                  <ChevronDown className="absolute right-3 top-1/2 -translate-y-1/2 w-8 h-8 text-[#1E265A] pointer-events-none" strokeWidth={2} />
                </div>
              </div>

              {/* Description */}
              <div className="flex flex-col gap-[2px]">
                <label className="text-[#1E265A] text-[20px] font-normal leading-normal">
                  Description
                </label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  placeholder="type down additional information about file"
                  className="w-full h-[128px] px-[9px] py-[13px] rounded-[10px] bg-[#E4B7F5]/36 text-[#1E265A] placeholder:text-[#1E265A]/49 text-[20px] font-normal resize-none focus:outline-none focus:ring-2 focus:ring-[#1E265A]/20"
                />
              </div>
            </div>

            {/* Upload Button */}
            <button
              onClick={handleSubmit}
              className="w-fit px-[22px] py-2 rounded-[20px] border-2 border-[#E4B7F5]/36 bg-white/[0.02] shadow-[inset_-2px_-2px_4px_rgba(255,255,255,0.3),inset_5px_5px_25px_rgba(255,255,255,0.4),0_10px_20px_rgba(42,42,42,0.36),inset_2px_2px_4px_#FFF] hover:shadow-[inset_-2px_-2px_4px_rgba(255,255,255,0.4),inset_5px_5px_25px_rgba(255,255,255,0.5),0_12px_24px_rgba(42,42,42,0.4),inset_2px_2px_4px_#FFF] transition-all active:scale-95"
            >
              <span className="text-[#1E265A] text-[25px] font-medium tracking-[0.25px]">
                Upload
              </span>
            </button>
          </div>

          {/* Right Section - File Drop Zone */}
          <div className="flex-1 lg:max-w-[512px]">
            <div
              onDragEnter={handleDrag}
              onDragLeave={handleDrag}
              onDragOver={handleDrag}
              onDrop={handleDrop}
              className={`relative w-full h-full min-h-[400px] lg:min-h-[699px] flex flex-col items-center justify-center px-8 md:px-12 lg:px-[100px] py-12 rounded-[15px] border border-dashed border-[#1E265A] bg-[#E4B7F5]/36 shadow-[inset_-2px_-2px_4px_rgba(0,0,0,0.25),inset_5px_5px_25px_rgba(228,183,245,0.6),inset_2px_2px_4px_#E4B7F5,0_4px_20px_#E4B7F5] transition-all ${
                dragActive ? "border-[#1E265A] bg-[#E4B7F5]/50 scale-[1.02]" : ""
              }`}
            >
              <div className="flex flex-col items-center gap-[30px]">
                <FilePlus className="w-20 h-20 text-[#1E265A]" strokeWidth={2} />
                
                <div className="flex flex-col items-center gap-[7px] px-[5px]">
                  <p className="text-[#1E265A] text-center text-[20px] md:text-[25px] font-medium tracking-[0.25px]">
                    Drop your files here or
                  </p>
                  <label className="cursor-pointer">
                    <input
                      type="file"
                      multiple
                      onChange={handleFileInput}
                      className="hidden"
                    />
                    <div className="px-[10px] py-[5px] rounded-[3px] bg-[#1E265A] hover:bg-[#2A3470] transition-colors">
                      <span className="text-white text-center text-[20px] md:text-[25px] font-medium tracking-[0.25px]">
                        choose files
                      </span>
                    </div>
                  </label>
                </div>

                {files.length > 0 && (
                  <div className="mt-4 w-full">
                    <p className="text-[#1E265A] text-[18px] font-medium mb-2">
                      Selected files:
                    </p>
                    <ul className="text-[#1E265A]/70 text-[16px] space-y-1">
                      {files.map((file, index) => (
                        <li key={index} className="truncate">
                          {file.name}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddFileForm;
