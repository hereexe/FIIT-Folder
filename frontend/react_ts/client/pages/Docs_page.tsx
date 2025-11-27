import { ChevronLeft } from "lucide-react";
import { useState } from "react";

interface Material {
  id: string;
  title: string;
  author: string;
  likes: number;
}
{/* TODO Автоматическое определение количества объектов*/}
export default function Index() {
  const [materials] = useState<Material[]>([
    { id: "1", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "2", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "3", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "4", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "5", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "6", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "7", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "8", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "9", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "10", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "11", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
    { id: "12", title: "2022. Расписанные билеты", author: "Artem Scheglevatov", likes: 202 },
  ]);

  return (
    <div className="min-h-screen bg-fiit-bg font-inter">
      <header className="w-full h-20 bg-fiit-header flex items-center justify-between px-6 rounded-sm">
        <button
          className="flex-shrink-0 text-fiit-text hover:opacity-80 transition-opacity"
          aria-label="User profile"
        >
          <svg className="w-14 h-14" viewBox="0 0 55 55" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M12.1832 44.5464C13.5773 41.262 16.8322 38.9584 20.625 38.9584H34.375C38.1679 38.9584 41.4228 41.262 42.8169 44.5464M36.6667 21.7709C36.6667 26.8335 32.5627 30.9375 27.5 30.9375C22.4374 30.9375 18.3334 26.8335 18.3334 21.7709C18.3334 16.7083 22.4374 12.6042 27.5 12.6042C32.5627 12.6042 36.6667 16.7083 36.6667 21.7709ZM50.4167 27.5C50.4167 40.1566 40.1566 50.4167 27.5 50.4167C14.8435 50.4167 4.58337 40.1566 4.58337 27.5C4.58337 14.8435 14.8435 4.58337 27.5 4.58337C40.1566 4.58337 50.4167 14.8435 50.4167 27.5Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
        </button>
        
        <h1 className="text-[45px] font-semibold text-fiit-text tracking-[0.9px] leading-none">
          FIIT Folder
        </h1>
        
        <button
          className="flex-shrink-0 text-fiit-text hover:opacity-80 transition-opacity"
          aria-label="Search"
        >
          <svg className="w-14 h-14" viewBox="0 0 55 55" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M25.2083 43.5417C35.3336 43.5417 43.5417 35.3336 43.5417 25.2083C43.5417 15.0831 35.3336 6.875 25.2083 6.875C15.0831 6.875 6.875 15.0831 6.875 25.2083C6.875 35.3336 15.0831 43.5417 25.2083 43.5417Z" fill="#E8E7F9"/>
            <path d="M48.125 48.125L38.1563 38.1563M25.2083 13.75C31.5366 13.75 36.6667 18.8801 36.6667 25.2083M43.5417 25.2083C43.5417 35.3336 35.3336 43.5417 25.2083 43.5417C15.0831 43.5417 6.875 35.3336 6.875 25.2083C6.875 15.0831 15.0831 6.875 25.2083 6.875C35.3336 6.875 43.5417 15.0831 43.5417 25.2083Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
        </button>
      </header>

      <div className="px-6 pt-8 pb-4 md:px-10 lg:px-16">
        <div className="flex items-center gap-4 mb-12">
          <button
            className="flex-shrink-0 text-fiit-text hover:opacity-80 transition-opacity"
            aria-label="Go back"
          >
            <ChevronLeft className="w-14 h-14" strokeWidth={1.5} />
          </button>
          <h2 className="text-4xl md:text-5xl font-semibold text-fiit-text tracking-wide leading-tight">
            Математический анализ
          </h2>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8 lg:gap-10 pb-20">
          {materials.map((material) => (
            <MaterialCard key={material.id} material={material} />
          ))}
        </div>
      </div>

      <button
        className="fixed bottom-8 right-8 text-fiit-text hover:opacity-80 transition-opacity"
        aria-label="Add new material"
      >
        <svg className="w-[60px] h-[60px]" viewBox="0 0 60 60" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M30 55C43.8071 55 55 43.8071 55 30C55 16.1929 43.8071 5 30 5C16.1929 5 5 16.1929 5 30C5 43.8071 16.1929 55 30 55Z" fill="#E8E7F9" fillOpacity="0.77"/>
          <path d="M30 20V40M20 30H40M55 30C55 43.8071 43.8071 55 30 55C16.1929 55 5 43.8071 5 30C5 16.1929 16.1929 5 30 5C43.8071 5 55 16.1929 55 30Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
        </svg>
      </button>
    </div>
  );
}

function MaterialCard({ material }: { material: Material }) {
  return (
    <a
      href={`#material-${material.id}`}
      className="relative w-full aspect-[5/4] rounded-[10px] bg-fiit-card overflow-hidden group cursor-pointer block transition-all duration-300 hover:shadow-lg hover:scale-105 active:scale-100"
    >
      <div className="absolute top-0 left-0 right-0 p-[10px_10px_10px_7px] flex flex-col gap-[7px] z-10">
        <h3 className="text-[23px] font-medium text-fiit-text tracking-[0.23px] leading-tight truncate group-hover:text-opacity-80 transition-colors">
          {material.title}
        </h3>
        <p className="text-xl text-fiit-text truncate group-hover:text-opacity-80 transition-colors">
          {material.author}
        </p>
      </div>

      <div className="absolute bottom-0 left-0 right-0 h-[35px] bg-fiit-bg/35 group-hover:bg-fiit-bg/50 transition-colors" />

      <div className="absolute bottom-0 left-0 right-0 h-[35px] flex items-center justify-between px-[15px] z-10">
        <div className="flex items-center gap-2">
          <button
            onClick={(e) => e.preventDefault()}
            className="text-fiit-text hover:opacity-80 transition-opacity"
            aria-label="Like"
          >
            <svg className="w-[25px] h-[25px]" viewBox="0 0 25 25" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path fillRule="evenodd" clipRule="evenodd" d="M12.4929 5.34981C10.4102 2.915 6.93728 2.26004 4.32785 4.4896C1.71842 6.71915 1.35105 10.4469 3.40025 13.0838C5.10402 15.2762 10.2602 19.9001 11.9501 21.3967C12.1392 21.5641 12.2337 21.6479 12.344 21.6807C12.4403 21.7095 12.5456 21.7095 12.6418 21.6807C12.7521 21.6479 12.8466 21.5641 13.0357 21.3967C14.7256 19.9001 19.8818 15.2762 21.5856 13.0838C23.6348 10.4469 23.3122 6.6957 20.658 4.4896C18.0037 2.2835 14.5756 2.915 12.4929 5.34981Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            </svg>
          </button>
          <span className="text-xl text-fiit-text">
            {material.likes}
          </span>
        </div>

        <button
          onClick={(e) => e.preventDefault()}
          className="text-fiit-text hover:opacity-80 transition-opacity"
          aria-label="Download"
        >
          <svg className="w-[25px] h-[25px]" viewBox="0 0 25 25" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M21.875 15.625V16.875C21.875 18.6252 21.875 19.5002 21.5344 20.1687C21.2348 20.7567 20.7567 21.2348 20.1687 21.5344C19.5002 21.875 18.6252 21.875 16.875 21.875H8.125C6.37484 21.875 5.49975 21.875 4.83128 21.5344C4.24327 21.2348 3.76521 20.7567 3.4656 20.1687C3.125 19.5002 3.125 18.6252 3.125 16.875V15.625M17.7083 10.4167L12.5 15.625M12.5 15.625L7.29167 10.4167M12.5 15.625V3.125" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
        </button>
      </div>
    </a>
  );
}
