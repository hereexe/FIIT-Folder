
import { useState, useCallback } from 'react';

const useSearch = () => {
    const [isSearchMenuOpen, setSearchOpen] = useState(false);
  
    const openSearchMenu = useCallback(() => {
      setSearchOpen(true);
    }, []);
  
    const closeSearchMenu = useCallback(() => {
      setSearchOpen(false);
    }, []);
  
    const toggleSearchMenu = useCallback(() => {
      setSearchOpen(prev => !prev); // Используем функциональную форму
    }, []); // Убрали зависимость от isSearchMenuOpen
  
    return {
      isSearchMenuOpen,
      openSearchMenu,
      closeSearchMenu,
      toggleSearchMenu
    };
  };
  
  export default useSearch;