import React, { useEffect, useState } from "react";
import { Container, Row, Col, Button } from "reactstrap";
import EventList from "./EventList";
import CreateEventForm from "./CreateEventForm";
import axios from "axios";
import ExistingEventModel from "@/models/ExistingEventModel";
import { useEventStore } from "@/state/eventStore";

export default function EventPage() {
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = React.useState(false);
  const { addEvent } = useEventStore();

  useEffect(() => {
    async function fetchEvents() {
      await axios.get("/api/event").then((response) => {
        response.data.forEach((event: ExistingEventModel) => {
          addEvent(event);
        });
      });
      setLoading(false);
    }

    fetchEvents();
  }, []);

  const onFormSubmit = () => {
    setShowForm(false);
  };

  const handleButtonClick = () => {
    // show the form
    setShowForm(!showForm);
  };
  return (
    <Container fluid>
      <Row>
        <Col xs={1}></Col>
        <Col xs={10}>
          <h1 className="text-center">Events</h1>
          {!showForm ? (
            <EventList />
          ) : (
            <CreateEventForm onFormSubmit={onFormSubmit} />
          )}
        </Col>
        <Col xs={1}>
          <Button
            style={{ position: "fixed", bottom: "1rem", right: "1rem" }}
            onClick={handleButtonClick}
          >
            {!showForm ? "Add Event" : "Cancel"}
          </Button>
        </Col>
      </Row>
    </Container>
  );
}
