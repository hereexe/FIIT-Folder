// components/Modal/Modal.tsx
import React, { ReactNode, useEffect } from 'react';
import ReactDOM from 'react-dom';
import './Modal.css';


interface ModalProps {
  children: ReactNode;
  isOpen: boolean;
  onClose: () => void;
  title?: string;
}

const Modal: React.FC<ModalProps> = ({
  children,
  isOpen,
  onClose,
  title
}) => {
  // Блокировка скролла при открытии модального окна
  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = 'hidden';
      // Блокировка клавиши Escape
      const handleEscape = (e: KeyboardEvent) => {
        if (e.key === 'Escape') onClose();
      };
      document.addEventListener('keydown', handleEscape);
      
      return () => {
        document.body.style.overflow = 'unset';
        document.removeEventListener('keydown', handleEscape);
      };
    }
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return ReactDOM.createPortal(
    <div className="modal-overlay" onClick={onClose}>
      <div 
        className="modal-container" 
        onClick={(e: React.MouseEvent) => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-labelledby={title ? "modal-title" : undefined}
      >
        <div className= "absolute inset-0 top-16 bottom-16 left-40 right-40 overflow-y-auto rounded-[10px] ">
          {children}
        </div>
      </div>
    </div>,
    document.body
  );
};

export default Modal;