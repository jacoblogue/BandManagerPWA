import EventModel from "@/models/EventModel";
import axios from "axios";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Button } from "reactstrap";
import CreateEventForm from "./CreateEventForm";

export default function EventList() {
  const [events, setEvents] = useState<EventModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);

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
    setShowForm(!showForm);
  };

  if (loading) {
    return <h1>Loading...</h1>;
  }

  return (
    <div>
      <h1>Events</h1>
      <ul>
        {events.map((event) => (
          <li key={event.id}>
            <Link to={`/events/${event.id}`}>{event.title}</Link>
          </li>
        ))}
      </ul>
      <Button onClick={handleButtonClick}>Add Event</Button>
      {showForm && <CreateEventForm setEvents={setEvents} events={events} />}
    </div>
  );
}
