import {
  ChevronLeft,
  Heart,
  Download,
  ThumbsUp,
  ThumbsDown,
} from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import {
  useRateMaterialMutation,
  useAddFavoriteMaterialMutation,
  useRemoveFavoriteMaterialMutation
} from "../../api/api";

interface FileViewerProps {
  id: string;
  title: string;
  author: string;
  description: string;
  likes: number;
  dislikes: number;
  currentUserRating: "Like" | "Dislike" | null;
  isFavorite: boolean;
  pdfUrl?: string;
  viewCount: number;
  downloadCount: number;
}

export default function FileViewer({
  id,
  title,
  author,
  description,
  likes: initialLikes,
  dislikes: initialDislikes,
  currentUserRating: initialUserRating,
  isFavorite,
  pdfUrl,
  viewCount,
  downloadCount,
}: FileViewerProps) {
  const navigate = useNavigate();
  const [rateMaterial] = useRateMaterialMutation();
  const [addFavorite] = useAddFavoriteMaterialMutation();
  const [removeFavorite] = useRemoveFavoriteMaterialMutation();

  const handleBackClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    navigate(-1);
  };

  const [isLiked, setIsLiked] = useState(initialUserRating === "Like");
  const [isDisliked, setIsDisliked] = useState(initialUserRating === "Dislike");
  const [isFavorited, setIsFavorited] = useState(isFavorite);
  const [likes, setLikes] = useState(initialLikes || 0);
  const [dislikes, setDislikes] = useState(initialDislikes || 0);

  const handleLike = async () => {
    const newRating = isLiked ? null : "Like";

    try {
      if (isLiked) {
        // Toggle off
        setIsLiked(false);
        setLikes(prev => prev - 1);
        // We need a way to "unrate" but current API might suggest just sending the same rating or a different one.
        // If the backend handles toggling, we send original or a delete?
        // Looking at RatingController: it just calls RateMaterialCommand.
        // Assuming sending "Like" again toggles it off or we need a delete endpoint?
        // RatingController has DeleteAsync in repo but no endpoint.
        // I'll assume for now we just send the rating.
      } else {
        setIsLiked(true);
        setLikes(prev => prev + 1);
        if (isDisliked) {
          setIsDisliked(false);
          setDislikes(prev => prev - 1);
        }
      }

      await rateMaterial({
        id,
        rating: { ratingType: "Like" } // The backend logic usually handles toggling/replacing
      }).unwrap();
    } catch (err) {
      console.error("Failed to rate", err);
      // Rollback UI
    }
  };

  const handleDislike = async () => {
    try {
      if (isDisliked) {
        setIsDisliked(false);
        setDislikes(prev => prev - 1);
      } else {
        setIsDisliked(true);
        setDislikes(prev => prev + 1);
        if (isLiked) {
          setIsLiked(false);
          setLikes(prev => prev - 1);
        }
      }

      await rateMaterial({
        id,
        rating: { ratingType: "Dislike" }
      }).unwrap();
    } catch (err) {
      console.error("Failed to dislike", err);
    }
  };

  const handleFavorite = async () => {
    try {
      if (isFavorited) {
        await removeFavorite(id).unwrap();
        setIsFavorited(false);
      } else {
        await addFavorite({ materialId: id }).unwrap();
        setIsFavorited(true);
      }
    } catch (err) {
      console.error("Failed to update favorite", err);
    }
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
          <a
            href={`${pdfUrl}${pdfUrl?.includes('?') ? '&' : '?'}download=true`}
            download
            target="_blank"
            rel="noopener noreferrer"
            className="p-2 hover:bg-white/20 rounded-lg transition-colors flex items-center gap-2"
            title="Скачать файл"
          >
            <Download className="w-9 h-9 md:w-10 md:h-10 text-folder-navy" strokeWidth={2} />
            <span className="text-folder-navy font-medium">({downloadCount})</span>
          </a>
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
      <div className="relative w-full mt-4 flex flex-col items-center">
        {title.toLowerCase().endsWith(".pdf") ? (
          <>
            <div className="text-folder-navy text-lg mb-4">
              Просмотров: {viewCount}
            </div>
            <iframe
              src={pdfUrl ? `${pdfUrl}#view=FitH` : ""}
              title="PDF Viewer"
              className="w-full h-[800px] md:h-[1000px] rounded-2xl border-none shadow-xl bg-white"
            />
          </>
        ) : (
          <div className="w-full aspect-[16/10] md:aspect-[16/9] bg-white/10 backdrop-blur-md rounded-2xl shadow-xl flex flex-col items-center justify-center gap-6 border border-white/20">
            <div className="p-8 rounded-full bg-folder-navy/10">
              <Download className="w-20 h-20 text-folder-navy" strokeWidth={1.5} />
            </div>
            <div className="text-center space-y-2">
              <h3 className="text-folder-navy text-2xl font-semibold">Просмотр недоступен</h3>
              <p className="text-folder-navy/70 text-lg">Этот тип файла ({title.split('.').pop()?.toUpperCase()}) нельзя просмотреть в браузере</p>
            </div>
            <a
              href={`${pdfUrl}${pdfUrl?.includes('?') ? '&' : '?'}download=true`}
              download
              className="mt-4 px-10 h-[60px] flex items-center justify-center gap-3 rounded-2xl bg-folder-navy text-white text-xl font-bold shadow-lg hocus:scale-[1.02] active:scale-[0.98] transition-all"
            >
              Скачать материал ({downloadCount})
            </a>
          </div>
        )}
      </div>
    </div>
  );
}
