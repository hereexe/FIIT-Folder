import React, { ReactNode } from 'react';
import Header from '../components/Header.tsx';
import { Outlet } from 'react-router-dom';


const HeaderLayout: React.FC = () => {
    return (
      <div className="min-h-screen bg-gray-100">
        {/* Header будет на всех страницах */}
        <Header />
        
        {/* Основной контент страницы */}
        <main>
          <Outlet />
        </main>
      </div>
    );
  };
  
  export default HeaderLayout;