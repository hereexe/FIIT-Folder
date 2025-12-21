import FileViewer from "@/components/FileView";
import { useLocation } from "react-router-dom";
import { MaterialDto } from "../../api/types";
import { useEffect, useState } from "react";

export default function Index() {
  const location = useLocation();
  const [material, setMaterial] = useState<MaterialDto | null>(null);

  useEffect(() => {
    // Получаем материал из state навигации
    if (location.state?.material) {
      setMaterial(location.state.material);
    }
  }, [location]);

  // Если материал не передан, показываем сообщение или загрузку
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
        <FileViewer {...material}/>
      </main>
    </div>
  );
}