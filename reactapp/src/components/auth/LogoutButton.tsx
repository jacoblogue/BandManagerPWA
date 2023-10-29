import { useAuth0 } from "@auth0/auth0-react";
import React from "react";
import { Button } from "reactstrap";

function LogoutButton() {
  const { logout } = useAuth0();
  return (
    <Button
      size="lg"
      onClick={() =>
        logout({ logoutParams: { returnTo: window.location.origin } })
      }
    >
      Log out
    </Button>
  );
}

export default LogoutButton;
