import { useNavigate } from 'react-router-dom';
export default function Index() {
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
  function MyComponent() {
    const navigate = useNavigate();
  
    const handleClick = () => {
      navigate('/target-page');
    };
  }

  return (
    <div className="min-h-screen bg-app-background flex flex-col">
      {/* Header Bar */}
      <header className="w-full h-[81px] bg-app-purple flex items-center justify-between px-6 md:px-[25px] flex-shrink-0">
        {/* User Icon */}
        <button
          className="flex-shrink-0 hover:opacity-30 hover:scale-110 transition-all"
          aria-label="User profile"
        >
          <svg
            width="55"
            height="55"
            viewBox="0 0 55 55"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            className="w-10 h-10 md:w-[55px] md:h-[55px]"
          >
            <path
              d="M12.1832 44.5463C13.5773 41.2619 16.8322 38.9583 20.625 38.9583H34.375C38.1679 38.9583 41.4228 41.2619 42.8169 44.5464M36.6667 21.7708C36.6667 26.8334 32.5627 30.9375 27.5 30.9375C22.4374 30.9375 18.3334 26.8334 18.3334 21.7708C18.3334 16.7082 22.4374 12.6042 27.5 12.6042C32.5627 12.6042 36.6667 16.7082 36.6667 21.7708ZM50.4167 27.5C50.4167 40.1565 40.1566 50.4167 27.5 50.4167C14.8435 50.4167 4.58337 40.1565 4.58337 27.5C4.58337 14.8435 14.8435 4.58333 27.5 4.58333C40.1566 4.58333 50.4167 14.8435 50.4167 27.5Z"
              stroke="#19024F"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        </button>

        {/* Title */}
        <h1 className="text-app-text text-2xl md:text-[45px] font-semibold tracking-[0.9px] leading-normal">
          FIIT Folder
        </h1>

        {/* Search Icon */}
        <button
          className="flex-shrink-0 hover:opacity-30 hover:scale-110 transition-all"
          aria-label="Search"
        >
          <svg
            width="55"
            height="55"
            viewBox="0 0 55 55"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            className="w-10 h-10 md:w-[55px] md:h-[55px]"
          >
            <path
              d="M25.2083 43.5417C35.3336 43.5417 43.5417 35.3336 43.5417 25.2083C43.5417 15.0831 35.3336 6.875 25.2083 6.875C15.0831 6.875 6.875 15.0831 6.875 25.2083C6.875 35.3336 15.0831 43.5417 25.2083 43.5417Z"
              fill="#E8E7F9"
            />
            <path
              d="M48.125 48.125L38.1563 38.1563M25.2083 13.75C31.5366 13.75 36.6667 18.8801 36.6667 25.2083M43.5417 25.2083C43.5417 35.3336 35.3336 43.5417 25.2083 43.5417C15.0831 43.5417 6.875 35.3336 6.875 25.2083C6.875 15.0831 15.0831 6.875 25.2083 6.875C35.3336 6.875 43.5417 15.0831 43.5417 25.2083Z"
              stroke="#19024F"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        </button>
      </header>

      {/* Main Content */}
      <main className="flex-1 w-full px-4 md:px-5 lg:px-6 py-8 md:py-[45px]">
        <div className="max-w-[1280px] mx-auto">
          {/* Grid of Subject Cards */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-x-[66px] md:gap-y-[50px]">
            {subjects.map((subject, index) => (
              <button
                
                key={index}
                className="h-[105px] bg-app-purple rounded-[10px] flex items-center justify-center px-4 text-app-text text-lg md:text-[25px] font-medium tracking-[0.25px] leading-normal text-center hover:opacity-30 hover:scale-110 transition-all whitespace-pre-line"
              >
                {subject}
              </button>
            ))}
          </div>
        </div>
      </main>
    </div>
  );
}
