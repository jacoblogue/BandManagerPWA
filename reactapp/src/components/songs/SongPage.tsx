import React from "react";
import SongList from "./SongList";
import { Button, Col, Container, Row } from "reactstrap";

export default function SongPage() {
  return (
    <Container fluid>
      <Row>
        <Col />
        <Col sm={10}>
          <SongList />
        </Col>
        <Col />
      </Row>
    </Container>
  );
}
