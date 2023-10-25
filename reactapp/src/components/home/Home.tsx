import React from "react";
import { Button, Container } from "reactstrap";
import { useAuth0 } from "@auth0/auth0-react";
import Profile from "@/profile/Profile";

export default function Home() {
  const { loginWithRedirect, logout, loginWithPopup } = useAuth0();
  return (
    <Container>
      <h1 className="display-1">Home</h1>
      <p className="lead">Welcome to the home page.</p>
      <Button onClick={() => loginWithPopup()}>Log In</Button>
      <Button
        onClick={() =>
          logout({ logoutParams: { returnTo: window.location.origin } })
        }
      >
        Log out
      </Button>

      <Profile />
    </Container>
  );
}
