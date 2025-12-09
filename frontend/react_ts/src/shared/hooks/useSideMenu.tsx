import { useState, useCallback } from 'react';

const useSideMenu = () => {
  const [isSideMenuOpen, setIsSideMenuOpen] = useState(false);

  const openSideMenu = useCallback(() => {
    setIsSideMenuOpen(true);
  }, []);

  const closeSideMenu = useCallback(() => {
    setIsSideMenuOpen(false);
  }, []);

  const toggleSideMenu = useCallback(() => {
    setIsSideMenuOpen(true);
  }, []);

  return {
    isSideMenuOpen,
    openSideMenu,
    closeSideMenu,
    toggleSideMenu
  };
};

export default useSideMenu;