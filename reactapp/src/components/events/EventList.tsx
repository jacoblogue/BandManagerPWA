import React, { useState } from "react";
import { Button, Collapse, Container, Row } from "reactstrap";
import EventCard from "./EventCard";
import { useEventStore } from "@/state/eventStore";
import ExistingEventModel from "@/models/ExistingEventModel";
import { BiChevronDown, BiChevronUp } from "react-icons/bi";

/**
 * Group events by month and return an object where keys are month names
 * and values are arrays of events for each month.
 * @param events Array of events to group by month.
 */
const groupEventsByMonth = (events: ExistingEventModel[]) => {
  const grouped: { [key: string]: ExistingEventModel[] } = {};

  events.forEach((event) => {
    const date = new Date(event.date); // Assuming event.date is in a Date-compatible format
    const month = date.toLocaleString("default", { month: "long" });

    if (!grouped[month]) {
      grouped[month] = [];
    }

    grouped[month].push(event);
  });

  return grouped;
};

export default function EventList() {
  const { events } = useEventStore();
  const groupedEvents = groupEventsByMonth(events);
  const [openSections, setOpenSections] = useState<Record<string, boolean>>({});

  /**
   * Toggle the visibility of a collapsible section.
   * @param month The name of the month to toggle.
   */
  const toggleSection = (month: string) => {
    setOpenSections((prev) => ({ ...prev, [month]: !prev[month] }));
  };
  return (
    <Container className="p-2 rounded" fluid>
      {Object.keys(groupedEvents).map((month) => (
        <div className="text-center" key={month}>
          <Button className="w-100 mb-2" onClick={() => toggleSection(month)}>
            {month} {openSections[month] ? <BiChevronUp /> : <BiChevronDown />}
          </Button>
          <Collapse isOpen={openSections[month]}>
            {groupedEvents[month].map((event) => (
              <EventCard event={event} key={event.id} />
            ))}
          </Collapse>
        </div>
      ))}
    </Container>
  );
}
