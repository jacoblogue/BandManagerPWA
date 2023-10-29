import { useAuth0 } from "@auth0/auth0-react";
import React from "react";
import { Button } from "reactstrap";

export default function LoginButton() {
  const { loginWithRedirect, loginWithPopup } = useAuth0();

  /* Trying loginWithPopup during dev to see if that mitigates the Consent issues */
  return (
    <Button
      size="lg"
      onClick={() =>
        loginWithRedirect({
          authorizationParams: {
            prompt: "consent",
            redirect_uri: window.location.origin + "/events",
          },
        })
      }
    >
      Log In
    </Button>
  );
}
