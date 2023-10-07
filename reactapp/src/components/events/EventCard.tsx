import EventModel from "@/models/ExistingEventModel";
import { formatDate, formatTime } from "@/utilities/dateUtils";
import React, { useState } from "react";
import { Card, CardHeader, CardBody, Button } from "reactstrap";

interface Props {
  event: EventModel;
  deleteEvent: (id: string) => void;
}

export default function EventCard({ event, deleteEvent }: Props) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  return (
    <Card className="mb-3">
      <CardHeader style={{ cursor: "pointer" }} onClick={toggleCollapse}>
        <strong>{formatDate(event.date)}</strong>
        <strong>{formatTime(event.date)}</strong>
        <h3>{event.title}</h3>
        <Button onClick={() => deleteEvent(event.id)}>Delete</Button>
      </CardHeader>
      {!isCollapsed && (
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
      )}
    </Card>
  );
}
