import {
    ChevronLeft,
    Heart,
    ThumbsUp,
} from "lucide-react";
import { useNavigate } from 'react-router-dom';
import { useGetFavoritesQuery } from '../../api/api';
import { MaterialDto } from "../../api/types";

export default function Index() {
    const navigate = useNavigate();

    const { data: materialsData, isLoading, error } = useGetFavoritesQuery();

    const handleClickBack = () => {
        navigate("/main_page");
    };

    if (isLoading) return <div className="min-h-screen bg-fiit-bg flex items-center justify-center text-fiit-text">Загрузка избранного...</div>;
    if (error) return <div className="min-h-screen bg-fiit-bg flex items-center justify-center text-fiit-text">Ошибка загрузки избранного</div>;

    const materials = materialsData || [];

    return (
        <div className="min-h-screen bg-fiit-bg font-inter">
            <div className="px-6 pt-8 pb-4 md:px-10 lg:px-16">
                <div className="flex items-center gap-4 mb-12">
                    <button
                        onClick={handleClickBack}
                        className="flex-shrink-0 text-fiit-text hover:opacity-80 transition-opacity"
                        aria-label="Go back"
                    >
                        <ChevronLeft className="w-14 h-14" strokeWidth={1.5} />
                    </button>
                    <h2 className="text-4xl md:text-5xl font-semibold text-fiit-text tracking-wide leading-tight">
                        Избранное
                    </h2>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8 lg:gap-10 pb-20">
                    {materials.map((material) => (
                        <FavoriteCard key={material.id} material={material} />
                    ))}
                    {materials.length === 0 && (
                        <div className="col-span-full text-center text-fiit-text text-xl">
                            У вас пока нет избранных материалов
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

function FavoriteCard({ material }: { material: MaterialDto }) {
    const navigate = useNavigate();
    const handleClickFile = () => {
        navigate("/fileview_page", {
            state: {
                material: material
            }
        });
    }
    return (
        <div
            onClick={() => handleClickFile()}
            className="relative w-full aspect-[5/4] rounded-[10px] bg-fiit-card overflow-hidden group cursor-pointer block transition-all duration-300 hover:shadow-lg hover:scale-105 active:scale-100"
        >
            <div className="absolute top-0 left-0 right-0 p-[10px_10px_10px_7px] flex flex-col gap-[7px] z-10">
                <h3 className="text-[23px] font-medium text-fiit-text tracking-[0.23px] leading-tight truncate group-hover:text-opacity-80 transition-colors">
                    {material.name}
                </h3>
                <p className="text-xl text-fiit-text truncate group-hover:text-opacity-80 transition-colors">
                    {material.authorName || "Аноним"}
                </p>
            </div>

            <div className="absolute bottom-0 left-0 right-0 h-[35px] bg-fiit-bg/35 group-hover:bg-fiit-bg/50 transition-colors" />

            <div className="absolute bottom-0 left-0 right-0 h-[35px] flex items-center justify-between px-[15px] z-10">
                <div className="flex items-center gap-4">
                    <div className="flex items-center gap-1">
                        <Heart className="w-[20px] h-[20px] fill-red-500 text-red-500" />
                    </div>
                    <div className="flex items-center gap-1">
                        <ThumbsUp className={`w-[20px] h-[20px] ${material.currentUserRating === "Like" ? "fill-blue-500 text-blue-500" : "text-fiit-text"}`} />
                        <span className="text-lg text-fiit-text">{material.likesCount || 0}</span>
                    </div>
                </div>
                <span className="text-lg text-fiit-text">{material.sizeFormatted}</span>
            </div>
        </div>
    );
}
