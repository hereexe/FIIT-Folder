import { useState } from "react";
import {
  Heart,
  Download,
  PlusCircle,
  Settings,
  ChevronLeft,
  Search,
  ChevronUp,
  ChevronDown,
  UserCircle,
  LayoutGrid,
  Menu,
  X,
} from "lucide-react";

export default function Index() {
  const [examsExpanded, setExamsExpanded] = useState(true);
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

  return (
    <div className="flex min-h-screen bg-app-bg font-[Inter]">
      {/* Mobile Sidebar Overlay */}
      {sidebarOpen && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 z-40 md:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`${
          sidebarOpen ? "translate-x-0" : "-translate-x-full"
        } md:translate-x-0 fixed md:relative ${
          sidebarCollapsed ? "md:w-[100px]" : "md:w-[338px]"
        } w-[338px] bg-app-sidebar flex flex-col h-screen z-50 transition-all duration-300`}
      >
        {/* Sidebar Header */}
        <div className="flex items-center justify-between px-5 py-[10px] bg-app-sidebar-header h-[77px]">
          {!sidebarCollapsed && (
            <div className="flex items-center gap-4">
              <svg className="w-10 h-10 text-app-sidebar" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8.86041 32.3974C9.87429 30.0087 12.2415 28.3334 14.9999 28.3334H24.9999C27.7584 28.3334 30.1255 30.0087 31.1394 32.3974M26.6666 15.8334C26.6666 19.5153 23.6818 22.5 19.9999 22.5C16.318 22.5 13.3333 19.5153 13.3333 15.8334C13.3333 12.1515 16.318 9.16671 19.9999 9.16671C23.6818 9.16671 26.6666 12.1515 26.6666 15.8334ZM36.6666 20C36.6666 29.2048 29.2047 36.6667 19.9999 36.6667C10.7952 36.6667 3.33325 29.2048 3.33325 20C3.33325 10.7953 10.7952 3.33337 19.9999 3.33337C29.2047 3.33337 36.6666 10.7953 36.6666 20Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
              <div className="text-app-sidebar text-[25px] font-medium tracking-[0.25px] max-w-[190px] truncate">
                Vasya Vasechkin
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
                <path d="M8.86041 32.3974C9.87429 30.0087 12.2415 28.3334 14.9999 28.3334H24.9999C27.7584 28.3334 30.1255 30.0087 31.1394 32.3974M26.6666 15.8334C26.6666 19.5153 23.6818 22.5 19.9999 22.5C16.318 22.5 13.3333 19.5153 13.3333 15.8334C13.3333 12.1515 16.318 9.16671 19.9999 9.16671C23.6818 9.16671 26.6666 12.1515 26.6666 15.8334ZM36.6666 20C36.6666 29.2048 29.2047 36.6667 19.9999 36.6667C10.7952 36.6667 3.33325 29.2048 3.33325 20C3.33325 10.7953 10.7952 3.33337 19.9999 3.33337C29.2047 3.33337 36.6666 10.7953 36.6666 20Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
            </button>
          )}
          {!sidebarCollapsed && (
            <button
              onClick={() => setSidebarCollapsed(true)}
              className="hover:opacity-80 transition-opacity hidden md:block"
              title="Collapse sidebar"
            >
              <svg className="w-[35px] h-[35px] text-app-sidebar" viewBox="0 0 35 35" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M13.125 4.375V30.625M11.375 4.375H23.625C26.0752 4.375 27.3003 4.375 28.2362 4.85185C29.0594 5.27129 29.7287 5.94058 30.1482 6.76379C30.625 7.69966 30.625 8.92477 30.625 11.375V23.625C30.625 26.0752 30.625 27.3003 30.1482 28.2362C29.7287 29.0594 29.0594 29.7287 28.2362 30.1482C27.3003 30.625 26.0752 30.625 23.625 30.625H11.375C8.92477 30.625 7.69966 30.625 6.76379 30.1482C5.94058 29.7287 5.27129 29.0594 4.85185 28.2362C4.375 27.3003 4.375 26.0752 4.375 23.625V11.375C4.375 8.92477 4.375 7.69966 4.85185 6.76379C5.27129 5.94058 5.94058 5.27129 6.76379 4.85185C7.69966 4.375 8.92477 4.375 11.375 4.375Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
            </button>
          )}
        </div>

        {/* Sidebar Menu */}
        {!sidebarCollapsed && (
          <div className="flex flex-col gap-5 px-5 py-[30px] pt-5">
            <button className="flex items-center gap-4 text-app-text hover:opacity-80 transition-opacity">
              <Heart className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                Favourites
              </span>
            </button>

            <button className="flex items-center gap-4 text-app-text hover:opacity-80 transition-opacity">
              <Download className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                Downloads
              </span>
            </button>

            <button className="flex items-center gap-4 text-app-text hover:opacity-80 transition-opacity">
              <PlusCircle className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                Add new file
              </span>
            </button>

            <button className="flex items-center gap-4 text-app-text hover:opacity-80 transition-opacity">
              <Settings className="w-[38px] h-[38px] stroke-[2]" />
              <span className="text-[23px] font-medium tracking-[0.23px] max-w-[190px] truncate">
                Settings
              </span>
            </button>
          </div>
        )}
      </aside>

      {/* Main Content */}
      <main className="flex-1">
        {/* Top Bar */}
        <div className="flex items-center gap-4 md:gap-[121px] px-5 md:px-[24px] py-[14px] flex-wrap md:flex-nowrap">
          <div className="flex items-center gap-2">
            <button
              className="md:hidden hover:opacity-80 transition-opacity"
              onClick={() => setSidebarOpen(!sidebarOpen)}
            >
              <Menu className="w-8 h-8 text-app-text stroke-[2]" />
            </button>
            <button className="hover:opacity-80 transition-opacity">
              <ChevronLeft className="w-10 h-10 text-app-text stroke-[2]" />
            </button>
          </div>

          <h1 className="text-app-text text-[28px] md:text-[45px] font-semibold tracking-[0.9px] flex-1 min-w-0">
            Математический анализ
          </h1>

          <button className="hover:opacity-80 transition-opacity">
            <Search className="w-10 h-10 text-app-text stroke-[2]" />
          </button>
        </div>

        {/* Content Sections */}
        <div className="px-5 md:px-[24px] py-0 flex flex-col gap-[15px] max-w-[780px]">
          {/* Экзамены Section */}
          <div className="flex flex-col gap-[15px]">
            <button
              onClick={() => setExamsExpanded(!examsExpanded)}
              className="flex items-center justify-between w-full md:w-[300px] hover:opacity-80 transition-opacity"
            >
              <div className="flex items-center gap-3">
                <div className="w-5 h-5 rounded-full bg-app-text flex-shrink-0" />
                <span className="text-app-text text-[25px] font-medium tracking-[0.25px]">
                  Экзамены
                </span>
              </div>
              {examsExpanded ? (
                <ChevronUp className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
              ) : (
                <ChevronDown className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
              )}
            </button>

            {examsExpanded && (
              <div className="flex flex-col gap-[15px]">
                <div className="bg-app-item rounded-[10px] px-9 py-[11px] hover:opacity-80 transition-opacity cursor-pointer">
                  <span className="text-black text-[23px] font-medium tracking-[0.23px]">
                    Экзамен, 1 семестр
                  </span>
                </div>
                <div className="bg-app-item rounded-[10px] px-9 py-[11px] hover:opacity-80 transition-opacity cursor-pointer">
                  <span className="text-black text-[23px] font-medium tracking-[0.23px]">
                    Экзамен, 2 семестр
                  </span>
                </div>
                <div className="bg-app-item rounded-[10px] px-9 py-[11px] hover:opacity-80 transition-opacity cursor-pointer">
                  <span className="text-black text-[23px] font-medium tracking-[0.23px]">
                    Экзамен, 3 семестр
                  </span>
                </div>
              </div>
            )}
          </div>

          {/* Коллоквиумы Section */}
          <button className="flex items-center justify-between w-full md:w-[300px] hover:opacity-80 transition-opacity">
            <div className="flex items-center gap-3">
              <div className="w-5 h-5 rounded-full bg-app-text flex-shrink-0" />
              <span className="text-black text-[25px] font-medium tracking-[0.25px]">
                Коллоквиумы
              </span>
            </div>
            <ChevronDown className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
          </button>

          {/* Лекции Section */}
          <button className="flex items-center justify-between w-full md:w-[300px] hover:opacity-80 transition-opacity">
            <div className="flex items-center gap-3">
              <div className="w-5 h-5 rounded-full bg-app-text flex-shrink-0" />
              <span className="text-black text-[25px] font-medium tracking-[0.25px]">
                Лекции
              </span>
            </div>
            <ChevronDown className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
          </button>

          {/* Практики Section */}
          <button className="flex items-center justify-between w-full md:w-[300px] hover:opacity-80 transition-opacity">
            <div className="flex items-center gap-3">
              <div className="w-5 h-5 rounded-full bg-app-text flex-shrink-0" />
              <span className="text-black text-[25px] font-medium tracking-[0.25px]">
                Практики
              </span>
            </div>
            <ChevronDown className="w-[30px] h-[30px] text-app-text stroke-[2] flex-shrink-0" />
          </button>
        </div>
      </main>
    </div>
  );
}
