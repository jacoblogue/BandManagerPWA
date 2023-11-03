import { useAuth0 } from "@auth0/auth0-react";
import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";

const LogoutButtonWithConfirmation = () => {
  const [modal, setModal] = useState(false);
  const { logout } = useAuth0();

  const toggle = () => setModal(!modal);

  const handleClickYes = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
  };

  return (
    <div>
      <Button color="danger" onClick={toggle}>
        Log Out
      </Button>
      <Modal centered isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Confirm Logout</ModalHeader>
        <ModalBody>Are you sure you want to log out?</ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleClickYes}>
            Yes
          </Button>{" "}
          <Button color="secondary" onClick={toggle}>
            No
          </Button>
        </ModalFooter>
      </Modal>
    </div>
  );
};

export default LogoutButtonWithConfirmation;
