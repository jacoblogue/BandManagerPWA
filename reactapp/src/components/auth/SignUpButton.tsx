import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "reactstrap";
import React from "react";

export default function SignUpButton() {
  const { loginWithRedirect } = useAuth0();

  const handleSignUp = async () => {
    try {
      await loginWithRedirect({
        authorizationParams: { screen_hint: "signup" },
        appState: {
          returnTo: "/events",
        },
      });
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Button size="lg" onClick={handleSignUp}>
      Sign Up
    </Button>
  );
}
