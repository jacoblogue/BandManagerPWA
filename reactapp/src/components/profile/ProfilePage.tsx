import React from "react";
import { Container, Row, Col } from "reactstrap";
import styles from "./ProfilePage.module.css";
import { useAuth0 } from "@auth0/auth0-react";

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
    <Container>
      <h1>Profile Page</h1>
      <Row>
        <Col>
          <h2>{user.name}</h2>
        </Col>
        <Col>
          <h3>{user.email}</h3>
        </Col>
        <Col>
          <img src={user.picture} alt={user.name} />
        </Col>
      </Row>
    </Container>
  );
};

export default ProfilePage;
