import React, { useEffect, useState } from "react";
import { Container, Row, Col, Button, Spinner } from "reactstrap";
import EventList from "./EventList";
import CreateEventForm from "./CreateEventForm";
import axios from "axios";
import { useEventStore } from "@/state/eventStore";
import { useAuth0 } from "@auth0/auth0-react";

export const EventPage: React.FC = () => {
  const apiAudience = import.meta.env.VITE_API_AUDIENCE;
  const { getAccessTokenSilently } = useAuth0();
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = React.useState(false);
  const { replaceEvents } = useEventStore();

  useEffect(() => {
    async function fetchEvents() {
      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: apiAudience,
            scope: "read:events",
          },
        });
        await axios
          .get("/api/event", {
            headers: {
              Authorization: `Bearer ${accessToken}`,
            },
          })
          .then((response) => {
            replaceEvents(response.data);
          });
      } catch (error) {
        console.error(error);
      }
      setLoading(false);
    }

    fetchEvents();
  }, [getAccessTokenSilently]);

  const onFormSubmit = () => {
    setShowForm(false);
  };

  const handleButtonClick = () => {
    // show the form
    setShowForm(!showForm);
  };

  if (loading) {
    return (
      <Container fluid>
        <Row>
          <Col xs={12}>
            <h1 className="text-center">Events</h1>
            <div className="text-center">
              <Spinner color="primary" />
            </div>
          </Col>
        </Row>
      </Container>
    );
  }

  return (
    <Container className="pb-5" fluid>
      <Row>
        <Col md={12}>
          <h1 className="text-center">Upcoming Events</h1>
          {!showForm ? (
            <EventList />
          ) : (
            <CreateEventForm onFormSubmit={onFormSubmit} />
          )}
        </Col>
        <Col>
          <Button
            style={{ position: "fixed", bottom: "1rem", right: "1rem" }}
            onClick={handleButtonClick}
          >
            {!showForm ? "New Event" : "Cancel"}
          </Button>
        </Col>
      </Row>
    </Container>
  );
};
