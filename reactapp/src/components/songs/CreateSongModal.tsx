import React from "react";
import { Modal, ModalBody, ModalHeader } from "reactstrap";

interface Props {
  isOpen: boolean;
  setModal: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function CreateSongModal({ isOpen, setModal }: Props) {
  const toggle = () => setModal(!isOpen);

  return (
    <Modal centered isOpen={isOpen} toggle={toggle}>
      <ModalHeader>Add Song</ModalHeader>
      <ModalBody>Add form here</ModalBody>
    </Modal>
  );
}
