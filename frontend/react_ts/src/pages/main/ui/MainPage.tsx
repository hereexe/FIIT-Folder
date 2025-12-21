import { Link, useNavigate } from "react-router-dom";

const MainPage = () => {
  const subjects = [
    "Математический\nанализ",
    "Алгебра и геометрия",
    "Дискретная\nматематика",
    "Теория вероятности",
    "Философия",
    "Языки и технологии\nпрограммирования",
    "Nand to Tetris",
    "Сети и протоколы\nинтернета",
    "Основы российской\nгосударственности",
    "Введение в математику",
    "Английский язык",
    "Основы проектной\nдеятельности",
  ];

  const navigate = useNavigate();

  const handleClick = (subject: string) => {
    sessionStorage.setItem("selectedSubject", subject);
    navigate("/exam_type", { state: { subject } });
  };

  return (
    <div className="min-h-screen bg-app-background flex flex-col">
      <main className="flex-1 w-full px-4 md:px-5 lg:px-6 py-8 md:py-[45px]">
        <div className="max-w-[1280px] mx-auto">
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-x-[66px] md:gap-y-[50px]">
            {subjects.map((subject) => (
              <Link
                to="/exam_type"
                onClick={() => handleClick(subject)}
                key={subject}
                className="h-[105px] bg-app-purple rounded-[10px] flex items-center justify-center px-4 text-app-text text-lg md:text-[25px] font-medium tracking-[0.25px] leading-normal text-center hover:opacity-30 hover:scale-110 transition-all whitespace-pre-line"
              >
                {subject}
              </Link>
            ))}
          </div>
        </div>
      </main>
    </div>
  );
};

export default MainPage;
