import "./global.css";
import "./Main_page.css"
import "./Exam_type.css"

import { Toaster } from "@/components/ui/toaster";
import { createRoot } from "react-dom/client";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import HeaderLayout from "@/layouts/HeaderLayout"
import Docs_page from "./pages/Docs_page";
import NotFound from "./pages/NotFound";
import Main_page from "./pages/Main_page"
import Exam_type from "./pages/Content_type"

const queryClient = new QueryClient();
{/*Todo: разобраться как встроить загрузку уже полученного файла*/}
const App = () => (
  <QueryClientProvider client={queryClient}>
    <TooltipProvider>
      <Toaster />
      <Sonner />
      <BrowserRouter>
        <Routes>
          <Route element={<HeaderLayout />}>
            <Route path="/main_page" element={<Main_page />} />
            <Route path="/doc_page" element={<Docs_page />} />
            <Route path="/exam_type" element={<Exam_type />} />
            {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
          
          <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </QueryClientProvider>
);

createRoot(document.getElementById("root")!).render(<App />);
