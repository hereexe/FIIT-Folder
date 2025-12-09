import { BrowserRouter, Route, Routes } from "react-router-dom";

import { DocsPage } from "@/pages/docs";
import { ExamTypePage } from "@/pages/exam-type";
import { FileViewPage } from "@/pages/file-view";
import { MainPage } from "@/pages/main";
import { NotFoundPage } from "@/pages/not-found";
import { HeaderLayout } from "@/widgets/header-layout";

export const AppRouter = () => (
  <BrowserRouter>
    <Routes>
      <Route element={<HeaderLayout />}>
        <Route path="/" element={<MainPage />} />
        <Route path="/main_page" element={<MainPage />} />
        <Route path="/doc_page" element={<DocsPage />} />
        <Route path="/exam_type" element={<ExamTypePage />} />
        <Route path="/fileview_page" element={<FileViewPage />} />
      </Route>
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  </BrowserRouter>
);
