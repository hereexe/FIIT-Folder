import "./global.css";
import "./Main_page.css"
import "./Exam_type.css"

import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import HeaderLayout from "@/layouts/HeaderLayout"
import Docs_page from "./pages/Docs_page";
import NotFound from "./pages/NotFound";
import Main_page from "./pages/Main_page"
import Exam_type from "./pages/Content_type"
import Fileview_page from "./pages/FileView_page"
import AddFile_page from "./components/AddFile"

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
            <Route path="/" element={<Main_page />} />
            <Route path="/main_page" element={<Main_page />} />
            <Route path="/doc_page" element={<Docs_page />} />
            <Route path="/exam_type" element={<Exam_type />} />
            <Route path="/fileview_page" element={<Fileview_page />} />
            {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
          </Route>
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </QueryClientProvider>
);

export default App;
