import React from "react";
import { Col, Container, Row } from "reactstrap";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../auth/LoginButton";

export default function Home() {
  const { isAuthenticated } = useAuth0();
  return (
    <Container
      fluid
      className="d-flex flex-column justify-content-center"
      style={{ height: "75vh" }}
    >
      <Row>
        <Col>
          <div className="text-center">
            <h1>Welcome to Band Manager</h1>
            {!isAuthenticated && <p className="fs-3">Please log in</p>}
          </div>
        </Col>
      </Row>
      <Row>
        <Col>
          <div className="text-center">
            {!isAuthenticated && <LoginButton />}
          </div>
        </Col>
      </Row>
    </Container>
  );
}
