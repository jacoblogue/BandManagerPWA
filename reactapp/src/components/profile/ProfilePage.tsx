import React from "react";
import { Container, Row, Col } from "reactstrap";
import { useAuth0 } from "@auth0/auth0-react";
import ThemeToggler from "../common/ThemeToggler";
import LogoutButtonWithConfirmation from "../auth/LogoutButtonWithConfirmation";

const ProfilePage: React.FC = () => {
  const { user } = useAuth0();

  if (!user) {
    return null;
  }

  return (
    <Container
      style={{ height: "100%" }}
      className="text-center d-flex flex-column justify-content-evenly align-items-center"
    >
      <h1>Profile Page</h1>
      <Row>
        <Col>
          <img src={user.picture} alt={user.name} />
        </Col>
      </Row>
      <Row>
        <Col>
          <h2>Username</h2>
          <p>{user.name}</p>
        </Col>
      </Row>
      <Row>
        <Col>
          <h2>Email</h2>
          <p>{user.email}</p>
        </Col>
      </Row>
      <Row>
        <Col>
          <h2>Set Theme</h2>
          <div className="mx-5">
            <ThemeToggler />
          </div>
        </Col>
      </Row>
      <Row>
        <Col>
          <LogoutButtonWithConfirmation />
        </Col>
      </Row>
    </Container>
  );
};

export default ProfilePage;
