import { useAuth0 } from "@auth0/auth0-react";
import React from "react";
import { Button } from "reactstrap";

export default function LoginButton() {
  const { loginWithRedirect } = useAuth0();

  /* Trying loginWithPopup during dev to see if that mitigates the Consent issues */
  return (
    <Button size="lg" onClick={() => loginWithRedirect()}>
      Log In
    </Button>
  );
}
