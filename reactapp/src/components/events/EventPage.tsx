import React, { useEffect, useState } from "react";
import { Container, Row, Col, Button } from "reactstrap";
import EventList from "./EventList";
import CreateEventForm from "./CreateEventForm";
import axios from "axios";
import ExistingEventModel from "@/models/ExistingEventModel";

export default function EventPage() {
  const [events, setEvents] = useState<ExistingEventModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = React.useState(false);

  useEffect(() => {
    async function fetchEvents() {
      await axios.get("/api/event").then((response) => {
        setEvents(response.data);
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
        <Col xs={3}></Col>
        <Col xs={6}>
          <h1 className="text-center">Events</h1>
          {!showForm ? (
            <EventList events={events} setEvents={setEvents} />
          ) : (
            <CreateEventForm
              events={events}
              setEvents={setEvents}
              onFormSubmit={onFormSubmit}
            />
          )}
        </Col>
        <Col xs={3}>
          <Button
            style={{ position: "fixed", bottom: "1rem", right: "1rem" }}
            onClick={handleButtonClick}
          >
            Add Event
          </Button>
        </Col>
      </Row>
    </Container>
  );
}
