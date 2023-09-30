import EventModel from "@/models/EventModel";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

export default function EventList() {
  const [events, setEvents] = useState<EventModel[]>([]);
  const [loading, setLoading] = useState(true);

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

  const testEvents: EventModel[] = [
    {
      id: generateGuid(),
      title: "Event 1",
      description: "Description 1",
      date: new Date(),
      location: "Location 1",
    },
    {
      id: generateGuid(),
      title: "Event 2",
      description: "Description 2",
      date: new Date(),
      location: "Location 2",
    },
    {
      id: generateGuid(),
      title: "Event 3",
      description: "Description 3",
      date: new Date(),
      location: "Location 3",
    },
  ];

  useEffect(() => {
    async function fetchEvents() {
      // const events = await getEvents();
      setEvents(testEvents);
      setLoading(false);
    }

    fetchEvents();
  }, []);

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
    </div>
  );
}
