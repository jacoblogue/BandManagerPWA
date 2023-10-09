import axios from "axios";
import React, { useState, useEffect } from "react";
import { Container, Row } from "reactstrap";
import EventCard from "./EventCard";
import ExistingEventModel from "@/models/ExistingEventModel";

interface Props {
  events: ExistingEventModel[];
  setEvents: React.Dispatch<React.SetStateAction<ExistingEventModel[]>>;
}

export default function EventList({ events, setEvents }: Props) {
  const deleteEvent = (id: string) => {
    axios.delete(`/api/event/${id}`).then((response) => {
      setEvents(events.filter((event) => event.id !== id));
    });
  };

  // if (loading) {
  //   return <strong>Loading...</strong>;
  // }

  return (
    <Container fluid>
      <Row>
        {events.map((event) => (
          <EventCard event={event} deleteEvent={deleteEvent} key={event.id} />
        ))}
      </Row>
    </Container>
  );
}
