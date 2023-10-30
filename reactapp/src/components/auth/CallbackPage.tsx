import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Container } from "reactstrap";

export default function CallbackPage() {
  const { error, isLoading, isAuthenticated } = useAuth0();
  const navigate = useNavigate();

  useEffect(() => {
    if (!isLoading && isAuthenticated) {
      navigate("/events");
    }
  }, [isLoading, isAuthenticated, navigate]);

  if (error) {
    return (
      <Container>
        <div className="text-center">
          <h1>Error</h1>
          <p>{error.message}</p>
        </div>
      </Container>
    );
  }

  return (
    <Container>
      <div className="text-center">
        <h1>Loading...</h1>
      </div>
    </Container>
  );
}
