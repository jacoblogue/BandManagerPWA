import { useAuth0 } from "@auth0/auth0-react";
import React from "react";
import { Button } from "reactstrap";

export default function LoginButton() {
  const { loginWithRedirect, loginWithPopup } = useAuth0();

  const handleLogin = async () => {
    try {
      await loginWithRedirect({
        authorizationParams: {
          prompt: "consent",
        },
        appState: {
          returnTo: "/songs",
        },
      });
    } catch (error) {
      console.error(error);
    }
  };

  /* Trying loginWithPopup during dev to see if that mitigates the Consent issues */
  return (
    <Button size="lg" onClick={handleLogin}>
      Log In
    </Button>
  );
}
