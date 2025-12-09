import { useNavigate } from "react-router-dom";

export interface Material {
  id: string;
  title: string;
  author: string;
  likes: number;
  isliked: boolean;
}

interface MaterialCardProps {
  material: Material;
}

const MaterialCard = ({ material }: MaterialCardProps) => {
  const navigate = useNavigate();

  const handleClickFile = () => {
    navigate("/fileview_page");
  };

  return (
    <button
      type="button"
      onClick={handleClickFile}
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
            type="button"
            className="text-fiit-text hover:opacity-80 transition-opacity"
            aria-label="Like"
          >
            <svg
              className={`w-[25px] h-[25px] ${material.isliked ? "fill-red-500" : ""}`}
              viewBox="0 0 25 25"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fillRule="evenodd"
                clipRule="evenodd"
                d="M12.4929 5.34981C10.4102 2.915 6.93728 2.26004 4.32785 4.4896C1.71842 6.71915 1.35105 10.4469 3.40025 13.0838C5.10402 15.2762 10.2602 19.9001 11.9501 21.3967C12.1392 21.5641 12.2337 21.6479 12.344 21.6807C12.4403 21.7095 12.5456 21.7095 12.6418 21.6807C12.7521 21.6479 12.8466 21.5641 13.0357 21.3967C14.7256 19.9001 19.8818 15.2762 21.5856 13.0838C23.6348 10.4469 23.3122 6.6957 20.658 4.4896C18.0037 2.2835 14.5756 2.915 12.4929 5.34981Z"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          </button>
          <span className="text-xl text-fiit-text">{material.likes}</span>
        </div>
      </div>
    </button>
  );
};

export default MaterialCard;
