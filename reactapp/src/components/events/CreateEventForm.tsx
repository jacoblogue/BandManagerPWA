import EventModel from "@/models/EventModel";
import axios from "axios";
import React from "react";
import { Button, Form, FormGroup, Input, Label } from "reactstrap";

interface Props {
  setEvents: React.Dispatch<React.SetStateAction<EventModel[]>>;
  events: EventModel[];
  onFormSubmit: () => void;
}

export default function CreateEventForm({
  setEvents,
  events,
  onFormSubmit,
}: Props) {
  const [newEvent, setNewEvent] = React.useState<EventModel>({
    title: "",
    description: "",
    date: new Date(),
    location: "",
  });
  const createEvent = (newEvent: EventModel) => {
    axios.post("/api/event", newEvent).then((response) => {
      setEvents([...events, response.data]);
    });
  };
  const handleSubmit = (formEvent: React.FormEvent<HTMLFormElement>) => {
    formEvent.preventDefault();
    createEvent(newEvent);
    onFormSubmit();
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setNewEvent((prevState) => {
      // Clone the existing state
      const updatedState: EventModel = { ...prevState };

      if ((name === "date" || name === "time") && prevState.date) {
        // Clone the existing Date object
        const date = new Date(prevState.date.getTime());

        if (name === "date") {
          const [year, month, day] = value.split("-");
          date.setFullYear(+year, +month - 1, +day);
        } else if (name === "time") {
          const [hours, minutes] = value.split(":");
          date.setHours(+hours, +minutes);
        }

        // Update the date field in the state
        updatedState.date = date;
      } else {
        // Update other fields in the state
        (updatedState as any)[name] = value;
      }

      return updatedState;
    });
  };

  return (
    <Form onSubmit={handleSubmit}>
      <FormGroup>
        <Label for="title">Title</Label>
        <Input
          type="text"
          name="title"
          id="title"
          placeholder="Enter the event title"
          value={newEvent.title}
          onChange={handleChange}
        />
      </FormGroup>
      <FormGroup>
        <Label for="description">Description</Label>
        <Input
          type="textarea"
          name="description"
          id="description"
          placeholder="Enter the event description"
          value={newEvent.description}
          onChange={handleChange}
        />
      </FormGroup>
      <FormGroup>
        <Label for="date">Date</Label>
        <Input
          type="date"
          name="date"
          id="date"
          placeholder="Enter the event date"
          onChange={handleChange}
        />
      </FormGroup>
      <FormGroup>
        <Label for="time">Time</Label>
        <Input
          type="time"
          name="time"
          id="time"
          placeholder="Enter the event time"
          onChange={handleChange}
        />
      </FormGroup>
      <FormGroup>
        <Label for="location">Location</Label>
        <Input
          type="text"
          name="location"
          id="location"
          placeholder="Enter the event location"
          value={newEvent.location}
          onChange={handleChange}
        />
      </FormGroup>
      <Button type="submit">Create Event</Button>
    </Form>
  );
}
