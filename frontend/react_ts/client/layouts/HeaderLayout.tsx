import React, { ReactNode, useEffect, useState } from "react";
import Header from "../components/Header.tsx";
import AddFileButton from "./AddFileButton.tsx";
import { Outlet, useLocation } from "react-router-dom";


const HeaderLayout: React.FC = () => {
  const location = useLocation();
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
  }, []);

  const excludedPaths = ['/login', '/register', '/landing'];
  const isExcluded = excludedPaths.some(path =>
    location.pathname.startsWith(path)
  );

  // На сервере рендерим всё, на клиенте - условно
  const showHeader = isClient ? !isExcluded : true;
  const token = localStorage.getItem("token");

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Header будет на всех страницах */}
      {!isExcluded && <Header />}
      {!isExcluded && token && <AddFileButton />}

      {/* Основной контент страницы */}
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default HeaderLayout;
