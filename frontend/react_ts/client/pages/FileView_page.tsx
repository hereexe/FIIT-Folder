import FileViewer from "@/components/FileView";
import { useLocation } from "react-router-dom";
import { MaterialDto } from "../../api/types";
import { useGetMaterialByIdQuery } from "../../api/api";

export default function Index() {
  const location = useLocation();
  const materialFromState = location.state?.material as MaterialDto | null;

  // Используем query для получения актуальных данных (лайки, избранное)
  const { data: fetchedMaterial } = useGetMaterialByIdQuery(
    materialFromState?.id || "",
    { skip: !materialFromState?.id }
  );

  const material = fetchedMaterial || materialFromState;

  if (!material) {
    return (
      <div className="min-h-screen bg-folder-gradient flex items-center justify-center">
        <div className="text-xl text-fiit-text">Материал не найден</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-folder-gradient">
      <main className="py-6">
        <FileViewer
          id={material.id}
          title={material.name}
          author={material.authorName || "Аноним"}
          description={material.description || ""}
          likes={material.likesCount}
          dislikes={material.dislikesCount}
          currentUserRating={material.currentUserRating as "Like" | "Dislike" | null}
          isFavorite={material.isFavorite}
          pdfUrl={material.downloadUrl}
        />
      </main>
    </div>
  );
}