import { Outlet } from "react-router-dom";

import { AddFileButton } from "@/features/file-upload";
import { Header } from "@/widgets/header";

const HeaderLayout = () => {
  return (
    <div className="min-h-screen bg-gray-100">
      <Header />
      <AddFileButton />
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default HeaderLayout;
