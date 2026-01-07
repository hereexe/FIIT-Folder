import FileViewer from "@/components/FileView";
import { useLocation } from "react-router-dom";
import { MaterialDto } from "../../api/types";
import { useGetMaterialByIdQuery, useIncrementViewCountMutation } from "../../api/api";
import { useEffect, useRef } from "react";

export default function Index() {
  const location = useLocation();
  const materialFromState = location.state?.material as MaterialDto | null;
  const viewIncrementedRef = useRef(false);

  // Используем query для получения актуальных данных (лайки, избранное)
  const { data: fetchedMaterial } = useGetMaterialByIdQuery(
    materialFromState?.id || "",
    { skip: !materialFromState?.id }
  );

  const [incrementViewCount] = useIncrementViewCountMutation();

  const material = fetchedMaterial || materialFromState;

  // Increment view count on mount (only once)
  useEffect(() => {
    if (material?.id && (material.name.toLowerCase().endsWith(".pdf") || material.name.toLowerCase().endsWith(".md")) && !viewIncrementedRef.current) {
      viewIncrementedRef.current = true;
      incrementViewCount(material.id);
    }
  }, [material?.id, material?.name, incrementViewCount]);

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
          viewCount={material.viewCount || 0}
          downloadCount={material.downloadCount || 0}
        />
      </main>
    </div>
  );
}
