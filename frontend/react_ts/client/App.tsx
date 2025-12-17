import "./global.css";
import "./Main_page.css"
import "./Exam_type.css"

import { Toaster } from "@/components/ui/toaster";
import { createRoot } from "react-dom/client";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import HeaderLayout from "@/layouts/HeaderLayout"
import Docs_page from "./pages/Docs_page";
import NotFound from "./pages/NotFound";
import Main_page from "./pages/Main_page"
import Exam_type from "./pages/Content_type"
import Fileview_page from "./pages/FileView_page"
import AddFile_page from "./components/AddFile"
import { Provider } from 'react-redux';
import { store } from "../api/store"
import React from 'react';

const queryClient = new QueryClient();
{/*Todo: разобраться как встроить загрузку уже полученного файла*/}
const App = () => (
  <React.StrictMode>
  <Provider store={store}>
  <QueryClientProvider client={queryClient}>
    <TooltipProvider>
      <Toaster />
      <Sonner />
      <BrowserRouter>
        <Routes>
          <Route element={<HeaderLayout />}>
            <Route path="/" element={<Navigate to="/main_page" replace />} />
            <Route path="/main_page" element={<Main_page />} />
            <Route path="/doc_page" element={<Docs_page />} />
            <Route path="/exam_type" element={<Exam_type />} />
            <Route path="/fileview_page" element={<Fileview_page />} />
            {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
          
          <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </QueryClientProvider>
  </Provider>
  </React.StrictMode>
);

createRoot(document.getElementById("root")!).render(<App />);
