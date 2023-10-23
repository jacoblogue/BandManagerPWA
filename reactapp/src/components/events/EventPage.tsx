import React, { useEffect, useState } from "react";
import { Container, Row, Col, Button, Spinner } from "reactstrap";
import EventList from "./EventList";
import CreateEventForm from "./CreateEventForm";
import axios from "axios";
import { useEventStore } from "@/state/eventStore";
import { useAuth0 } from "@auth0/auth0-react";

export default function EventPage() {
  const { getAccessTokenSilently, getAccessTokenWithPopup } = useAuth0();
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = React.useState(false);
  const { replaceEvents } = useEventStore();

  useEffect(() => {
    async function fetchEvents() {
      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: "https://bandmanager/auth0",
            scope: "read:events",
          },
        });

        console.log(accessToken);

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
              <Spinner color="primary" /> {/* add Spinner */}
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
}
