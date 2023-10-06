import EventModel from "@/models/EventModel";
import axios from "axios";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import {
  Button,
  Card,
  CardBody,
  CardHeader,
  Col,
  Container,
  Row,
} from "reactstrap";
import CreateEventForm from "./CreateEventForm";
import { BsPlus, BsPlusCircle } from "react-icons/bs";
import { formatDate, formatTime } from "@/utilities/dateUtils";

export default function EventList() {
  const [events, setEvents] = useState<EventModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [formSubmitted, setFormSubmitted] = useState(false);

  function generateGuid(): string {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  }

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

  const onFormSubmit = () => {
    setFormSubmitted(true);
  };

  if (loading) {
    return <strong>Loading...</strong>;
  }

  const formatDateString = (date: Date) => {
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const hours = date.getHours();
    const minutes = date.getMinutes();

    return `${year}-${month}-${day} ${hours}:${minutes}`;
  };

  return (
    <Container fluid>
      <Row>
        <Col xs={3}></Col>

        <Col xs={6}>
          <h1>Events</h1>
          {events.map((event) => (
            <Card key={event.id} className="mb-3">
              <CardHeader>
                <h3>{event.title}</h3>
              </CardHeader>
              <CardBody>
                <p>
                  <strong>Location:</strong> {event.location}
                </p>
                <p>
                  <strong>Date:</strong>
                  {event.date ? formatDate(event.date) : "No date specified"}
                </p>
                <p>
                  <strong>Description:</strong> {event.description}
                </p>
                <p>
                  <strong>Time:</strong>{" "}
                  {event.date
                    ? formatTime(new Date(event.date))
                    : "No time specified"}
                </p>
              </CardBody>
            </Card>
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
