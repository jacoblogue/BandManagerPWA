import { Modal, ModalBody, ModalHeader } from "reactstrap";
import React from "react";
import CreateEventForm from "./CreateEventForm";

interface Props {
  isOpen: boolean;
  setModal: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function CreateEventModal({ isOpen, setModal }: Props) {
  const onFormSubmit = () => {
    setModal(false);
  };

  const toggle = () => setModal(!isOpen);

  return (
    <Modal centered isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Add New Event</ModalHeader>
      <ModalBody>
        <CreateEventForm onFormSubmit={onFormSubmit} />
      </ModalBody>
    </Modal>
  );
}
