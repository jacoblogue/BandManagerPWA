import ExistingEventModel from "@/models/ExistingEventModel";
import { useEventStore } from "@/state/eventStore";
import { formatDate, formatTime } from "@/utilities/dateUtils";
import axios from "axios";
import React, { useState } from "react";
import { Card, CardHeader, CardBody, Button } from "reactstrap";

interface Props {
  event: ExistingEventModel;
}

export default function EventCard({ event }: Props) {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const { deleteEvent } = useEventStore();

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const handleDelete = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    e.stopPropagation();
    await axios.delete(`/api/event/${event.id}`);
  };

  return (
    <Card className="mb-3">
      <CardHeader style={{ cursor: "pointer" }} onClick={toggleCollapse}>
        <strong>{formatDate(event.date)}</strong>
        <strong>{formatTime(event.date)}</strong>
        <h3>{event.title}</h3>
        <Button color="primary" onClick={(e) => handleDelete(e)}>
          Delete
        </Button>
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
