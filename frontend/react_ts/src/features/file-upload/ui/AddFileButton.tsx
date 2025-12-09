import AddFileForm from "./AddFileForm";
import { useModal } from "@/shared/hooks/useModal";
import Modal from "@/shared/ui/modal/Modal";

const AddFileButton = () => {
  const { isOpen, openModal, closeModal } = useModal();

  return (
    <>
      <button
        className="fixed bottom-8 right-8 text-fiit-text hover:opacity-80 transition-opacity"
        aria-label="Add new material"
        onClick={openModal}
      >
        <svg
          className="w-[60px] h-[60px]"
          viewBox="0 0 60 60"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M30 55C43.8071 55 55 43.8071 55 30C55 16.1929 43.8071 5 30 5C16.1929 5 5 16.1929 5 30C5 43.8071 16.1929 55 30 55Z"
            fill="#E8E7F9"
            fillOpacity="0.77"
          />
          <path
            d="M30 20V40M20 30H40M55 30C55 43.8071 43.8071 55 30 55C16.1929 55 5 43.8071 5 30C5 16.1929 16.1929 5 30 5C43.8071 5 55 16.1929 55 30Z"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </svg>
      </button>
      <Modal isOpen={isOpen} onClose={closeModal}>
        <AddFileForm />
      </Modal>
    </>
  );
};

export default AddFileButton;
