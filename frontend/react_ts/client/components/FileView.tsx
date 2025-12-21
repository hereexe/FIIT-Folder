import {
  ChevronLeft,
  Heart,
  Download,
  ThumbsUp,
  ThumbsDown,
} from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

interface FileViewerProps {
  title: string;
  author: string;
  description: string;
  likes: number;
  dislikes: number;
  pdfUrl?: string;
}

export default function FileViewer({
  title,
  author,
  description,
  likes: initialLikes,
  dislikes: initialDislikes,
  pdfUrl,
}: FileViewerProps) {
  const navigate = useNavigate();
  const handleBackClick = (e: React.MouseEvent) => {
    e.stopPropagation(); // Важно: останавливаем всплытие
    navigate("/doc_page")
  };
  const [isLiked, setIsLiked] = useState(false);
  const [isDisliked, setIsDisliked] = useState(false);
  const [isFavorited, setIsFavorited] = useState(false);
  const [likes, setLikes] = useState(initialLikes);
  const [dislikes, setDislikes] = useState(initialDislikes);

  const handleLike = () => {
    
    if (isLiked) {
      // Already liked, remove like
      setIsLiked(false);
      setLikes(likes - 1);
    } else {
      // New like
      setIsLiked(true);
      setLikes(likes + 1);
      // Remove dislike if present
      if (isDisliked) {
        setIsDisliked(false);
        setDislikes(dislikes - 1);
      }
    }
  };

  const handleDislike = () => {
    if (isDisliked) {
      // Already disliked, remove dislike
      setIsDisliked(false);
      setDislikes(dislikes - 1);
    } else {
      // New dislike
      setIsDisliked(true);
      setDislikes(dislikes + 1);
      // Remove like if present
      if (isLiked) {
        setIsLiked(false);
        setLikes(likes - 1);
      }
    }
  };

  const handleFavorite = () => {
    setIsFavorited(!isFavorited);
  };
  return (
    <div className="flex flex-col items-center gap-4 w-full max-w-80p mx-auto px-4 md:px-6 py-6">
      {/* Header section */}
      <div className="flex items-center justify-between w-full py-3">
        <button
          onClick={handleBackClick}
          className="p-2 hover:bg-white/20 rounded-lg transition-colors">
          <ChevronLeft
            className="w-10 h-10 md:w-11 md:h-11 text-folder-navy"
            strokeWidth={2}
          />
        </button>

        <div className="flex flex-col items-center justify-center gap-2 flex-1 px-4">
          <h2 className="text-folder-navy whitespace-normal break-words text-xl md:text-2xl lg:text-[30px] font-medium tracking-wide text-center truncate max-w-full">
            {title}
          </h2>
          <p className="text-folder-navy text-lg md:text-xl lg:text-[25px] font-medium tracking-wide text-center truncate max-w-full">
            {author}
          </p>
        </div>

        <div className="flex items-center gap-2 md:gap-3">
          <button
            onClick={handleFavorite}
            className="p-2 hover:bg-white/20 rounded-lg transition-colors">
            <Heart
              className={`w-9 h-9 md:w-10 md:h-10 stroke-folder-navy transition-all ${isFavorited ? "fill-red-500" : ""
                }`}
              strokeWidth={2}
            />
          </button>
        </div>
      </div>

      {/* Description */}
      <div className="flex justify-center text-center w-full max-w-2xl">
        <p className="text-folder-navy text-base md:text-lg lg:text-xl leading-relaxed">
          {description}
        </p>
      </div>

      {/* Likes/Dislikes */}
      <div className="flex items-center justify-center gap-6 md:gap-8 w-full">
        <button
          onClick={handleLike}
          className="flex items-center gap-2 md:gap-3 hover:opacity-70 transition-opacity"
        >
          <ThumbsUp
            className={`w-5 h-5 md:w-6 md:h-6 transition-all ${isLiked ? "fill-blue-500 text-folder-navy" : "text-folder-navy"
              }`}
            strokeWidth={2}
          />
          <span className={`text-lg md:text-xl font-normal transition-colors ${isLiked ? "text-folder-navy font-semibold" : "text-folder-navy"
            }`}>
            {likes}
          </span>
        </button>
        <button
          onClick={handleDislike}
          className="flex items-center gap-2 md:gap-3 hover:opacity-70 transition-opacity"
        >
          <ThumbsDown
            className={`w-5 h-5 md:w-6 md:h-6 transition-all ${isDisliked ? "fill-blue-500 text-folder-navy" : "text-folder-navy"
              }`}
            strokeWidth={2}
          />
          <span className={`text-lg md:text-xl font-normal transition-colors ${isDisliked ? "text-folder-navy font-semibold" : "text-folder-navy"
            }`}>
            {dislikes}
          </span>
        </button>
      </div>

      {/* File content area */}
      <div className="relative w-full mt-4 flex gap-3">
        <div className="flex-1 aspect-[16/10] md:aspect-[16/9] bg-[#C4C4C4] rounded-2xl shadow-lg" />
        <iframe
          src={pdfUrl ? `${pdfUrl}#view=FitH` : ""}
          title="PDF Viewer"
          className="w-full h-[1000px] rounded border"
          frameBorder="0"
        />
      </div>
    </div>
  );
}
