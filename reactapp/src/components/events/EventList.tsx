import axios from "axios";
import React, { useState, useEffect } from "react";
import { Button, Col, Container, Row } from "reactstrap";
import CreateEventForm from "./CreateEventForm";
import EventCard from "./EventCard";
import ExistingEventModel from "@/models/ExistingEventModel";

export default function EventList() {
  const [events, setEvents] = useState<ExistingEventModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [formSubmitted, setFormSubmitted] = useState(false);

  useEffect(() => {
    async function fetchEvents() {
      // const events = await getEvents();
      axios.get("/api/event").then((response) => {
        setEvents(response.data);
      });
      setLoading(false);
    }

    fetchEvents();
  }, []);

  const handleButtonClick = () => {
    // show the form
    setFormSubmitted(false);
    setShowForm(!showForm);
  };

  const deleteEvent = (id: string) => {
    axios.delete(`/api/event/${id}`).then((response) => {
      setEvents(events.filter((event) => event.id !== id));
    });
  };

  const onFormSubmit = () => {
    setFormSubmitted(true);
  };

  if (loading) {
    return <strong>Loading...</strong>;
  }

  return (
    <Container fluid>
      <Row>
        <Col xs={3}></Col>
        <Col xs={6}>
          <h1 className="text-center">Events</h1>
          {events.map((event) => (
            <EventCard event={event} deleteEvent={deleteEvent} key={event.id} />
          ))}
          {showForm && !formSubmitted && (
            <CreateEventForm
              setEvents={setEvents}
              onFormSubmit={onFormSubmit}
              events={events}
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
