import React from "react";
import { Container, Row, Col } from "reactstrap";
import styles from "./ProfilePage.module.css";
import { useAuth0 } from "@auth0/auth0-react";
import LogoutButton from "../auth/LogoutButton";

interface User {
  name: string;
  email: string;
  picture: string;
}

/**
 * Renders a user's profile information in a visually pleasing manner.
 */
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
          <h3>Email</h3>
          <p>{user.email}</p>
        </Col>
      </Row>
      <Row>
        <LogoutButton />
      </Row>
    </Container>
  );
};

export default ProfilePage;
