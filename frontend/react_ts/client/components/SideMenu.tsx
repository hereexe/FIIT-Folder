// SideMenu.tsx
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Heart,
  PlusCircle,
  UserCircle,
  LayoutGrid,
  X,
} from "lucide-react";

interface SideMenuProps {
  isOpen: boolean;
  onClose: () => void;
}

const SideMenu: React.FC<SideMenuProps> = ({ isOpen, onClose }) => {
  const navigate = useNavigate();
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
  const [userName, setUserName] = useState<string>("Войдите в аккаунт");
  const [isMounted, setIsMounted] = useState(false);

  // Инициализация имени пользователя
  // Инициализация имени пользователя
  useEffect(() => {
    const sessionName = sessionStorage.getItem("userName");

    if (sessionName) {
      setUserName(sessionName);
    } else {
      // Пытаемся восстановить из токена
      const token = localStorage.getItem("token");
      if (token) {
        try {
          // Декодируем JWT (payload - вторая часть)
          const base64Url = token.split('.')[1];
          const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
          const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
          }).join(''));

          const payload = JSON.parse(jsonPayload);

          // Check various common claim names
          const login =
            payload.unique_name ||
            payload.name ||
            payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
            payload.sub;

          if (login) {
            setUserName(login);
            sessionStorage.setItem("userName", login);
          }
        } catch (e) {
          console.error("Failed to decode token", e);
        }
      } else {
        setUserName("Войдите в аккаунт");
      }
    }
  }, [isOpen]); // Обновляем при каждом открытии

  // Для плавной анимации
  useEffect(() => {
    if (isOpen) {
      setIsMounted(true);
    }
  }, [isOpen]);

  const handleAnimationEnd = () => {
    if (!isOpen) {
      setIsMounted(false);
    }
  };

  if (!isMounted && !isOpen) {
    return null;
  }

  const handleLoginClick = () => {
    navigate("/login");
    onClose();
  };

  const token = localStorage.getItem("token");

  return (
    <>
      {/* Overlay с анимацией */}
      <div
        className={`fixed inset-0 z-50 bg-black transition-all duration-300 ease-in-out ${isOpen ? 'bg-opacity-50' : 'bg-opacity-0'
          } ${!isMounted ? 'hidden' : ''}`}
        onClick={onClose}
        onTransitionEnd={handleAnimationEnd}
      />

      {/* Sidebar с анимацией */}
      <aside
        className={`fixed top-0 left-0 h-full z-50 bg-app-sidebar flex flex-col transition-all duration-300 ease-in-out ${isOpen ? 'translate-x-0' : '-translate-x-full'
          } ${sidebarCollapsed ? 'md:w-[100px]' : 'md:w-[400px]'} w-[338px]`}
        onTransitionEnd={handleAnimationEnd}
      >
        {/* Sidebar Header */}
        <div className="flex items-center justify-between px-5 py-[10px] bg-app-sidebar-header h-[77px]">
          {!sidebarCollapsed && (
            <div className="flex items-center gap-4">
              <svg className="w-10 h-10 text-app-sidebar" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8.86041 32.3974C9.87429 30.0087 12.2415 28.3334 14.9999 28.3334H24.9999C27.7584 28.3334 30.1255 30.0087 31.1394 32.3974M26.6666 15.8334C26.6666 19.5153 23.6818 22.5 19.9999 22.5C16.318 22.5 13.3333 19.5153 13.3333 15.8334C13.3333 12.1515 16.318 9.16671 19.9999 9.16671C23.6818 9.16671 26.6666 12.1515 26.6666 15.8334ZM36.6666 20C36.6666 29.2048 29.2047 36.6667 19.9999 36.6667C10.7952 36.6667 3.33325 29.2048 3.33325 20C3.33325 10.7953 10.7952 3.33337 19.9999 3.33337C29.2047 3.33337 36.6666 10.7953 36.6666 20Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
              </svg>
              <div className="text-app-sidebar text-[25px] font-medium tracking-[0.25px] truncate max-w-[190px]">
                {userName}
              </div>
            </div>
          )}
          {sidebarCollapsed && (
            <button
              onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
              className="hover:opacity-80 transition-opacity mx-auto"
              title="Expand sidebar"
            >
              <svg className="w-10 h-10 text-app-sidebar" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8.86041 32.3974C9.87429 30.0087 12.2415 28.3334 14.9999 28.3334H24.9999C27.7584 28.3334 30.1255 30.0087 31.1394 32.3974M26.6666 15.8334C26.6666 19.5153 23.6818 22.5 19.9999 22.5C16.318 22.5 13.3333 19.5153 13.3333 15.8334C13.3333 12.1515 16.318 9.16671 19.9999 9.16671C23.6818 9.16671 26.6666 12.1515 26.6666 15.8334ZM36.6666 20C36.6666 29.2048 29.2047 36.6667 19.9999 36.6667C10.7952 36.6667 3.33325 29.2048 3.33325 20C3.33325 10.7953 10.7952 3.33337 19.9999 3.33337C29.2047 3.33337 36.6666 10.7953 36.6666 20Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
              </svg>
            </button>
          )}
          {!sidebarCollapsed && (
            <button
              onClick={onClose}
              className="hover:opacity-80 transition-opacity hidden md:block"
              title="Close sidebar"
            >
              <svg className="w-[35px] h-[35px] text-app-sidebar" viewBox="0 0 35 35" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M13.125 4.375V30.625M11.375 4.375H23.625C26.0752 4.375 27.3003 4.375 28.2362 4.85185C29.0594 5.27129 29.7287 5.94058 30.1482 6.76379C30.625 7.69966 30.625 8.92477 30.625 11.375V23.625C30.625 26.0752 30.625 27.3003 30.1482 28.2362C29.7287 29.0594 29.0594 29.7287 28.2362 30.1482C27.3003 30.625 26.0752 30.625 23.625 30.625H11.375C8.92477 30.625 7.69966 30.625 6.76379 30.1482C5.94058 29.7287 5.27129 29.0594 4.85185 28.2362C4.375 27.3003 4.375 26.0752 4.375 23.625V11.375C4.375 8.92477 4.375 7.69966 4.85185 6.76379C5.27129 5.94058 5.94058 5.27129 6.76379 4.85185C7.69966 4.375 8.92477 4.375 11.375 4.375Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
              </svg>
            </button>
          )}
        </div>

        {/* Sidebar Menu */}
        {!sidebarCollapsed && (
          <div className="flex flex-col gap-5 px-5 py-[30px] pt-5">
            <button
              onClick={() => { navigate("/main_page"); onClose(); }}
              className="flex items-center gap-5 text-app-text hover:opacity-80 transition-opacity"
            >
              <LayoutGrid className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[220px] truncate">
                Главная
              </span>
            </button>

            <button
              onClick={() => { navigate("/favorites"); onClose(); }}
              className="flex items-center gap-5 text-app-text hover:opacity-80 transition-opacity"
            >
              <Heart className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[220px] truncate">
                Избранное
              </span>
            </button>

            <button
              onClick={() => { navigate("/add_file"); onClose(); }}
              className="flex items-center gap-5 text-app-text hover:opacity-80 transition-opacity"
            >
              <PlusCircle className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[220px] truncate">
                Добавить файл
              </span>
            </button>

            {token ? (
              <button
                onClick={() => {
                  localStorage.removeItem("token");
                  sessionStorage.removeItem("userName");
                  onClose();
                  navigate("/main_page");
                  window.location.reload();
                }}
                className="flex items-center gap-5 text-red-500 hover:opacity-80 transition-opacity"
              >
                <X className="w-[38px] h-[38px] stroke-[2]" />
                <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                  Выйти
                </span>
              </button>
            ) : (
              <button
                onClick={handleLoginClick}
                className="flex items-center gap-5 text-app-text hover:opacity-80 transition-opacity"
              >
                <UserCircle className="w-[38px] h-[38px] stroke-[2]" />
                <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                  Войти
                </span>
              </button>
            )}
          </div>
        )}
      </aside>
    </>
  );
};

export default SideMenu;