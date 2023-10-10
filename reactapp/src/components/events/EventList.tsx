import React from "react";
import { Container, Row } from "reactstrap";
import EventCard from "./EventCard";
import { useEventStore } from "@/state/eventStore";

export default function EventList() {
  const { events } = useEventStore();
  return (
    <Container fluid>
      <Row>
        {events.map((event) => (
          <EventCard event={event} key={event.id} />
        ))}
      </Row>
    </Container>
  );
}
