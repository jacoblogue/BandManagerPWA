import EventModel from "@/models/EventModel";
import axios from "axios";
import React from "react";
import { Button, Form, FormGroup, Input, Label } from "reactstrap";

interface Props {
  setEvents: React.Dispatch<React.SetStateAction<EventModel[]>>;
  events: EventModel[];
}

export default function CreateEventForm({ setEvents, events }: Props) {
  const [newEvent, setNewEvent] = React.useState<EventModel>({
    title: "",
    description: "",
    date: new Date(),
    location: "",
  });
  const createEvent = (newEvent: EventModel) => {
    axios.post("/api/event", newEvent).then((response) => {
      setEvents([...events, newEvent]);
      console.log(response.data);
    });
  };
  const handleSubmit = (formEvent: React.FormEvent<HTMLFormElement>) => {
    formEvent.preventDefault();
    createEvent(newEvent);
    // TODO: Send the event data to the server
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setNewEvent((prevState) => ({
      ...prevState,
      [name]: value,
    }));
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
