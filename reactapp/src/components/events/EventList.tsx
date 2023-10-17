import React, { useState } from "react";
import { Card, CardBody, CardColumns, CardHeader, Collapse } from "reactstrap";
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
    const month = date.toLocaleString("default", {
      month: "long",
      year: "numeric",
    });

    if (!grouped[month]) {
      grouped[month] = [];
    }

    grouped[month].push(event);
  });

  return grouped;
};
// Set the current month to be open by default
const currentMonth = new Date().toLocaleString("default", {
  month: "long",
  year: "numeric",
});
const defaultOpenSections = { [currentMonth]: true };

export default function EventList() {
  const { events } = useEventStore();
  const groupedEvents = groupEventsByMonth(events);
  const [openSections, setOpenSections] =
    useState<Record<string, boolean>>(defaultOpenSections);

  /**
   * Toggle the visibility of a collapsible section.
   * @param month The name of the month to toggle.
   */
  const toggleSection = (month: string) => {
    setOpenSections((prev) => ({ ...prev, [month]: !prev[month] }));
  };

  return (
    <CardColumns>
      {Object.keys(groupedEvents).map((month) => (
        <Card key={month} className="mb-2">
          <CardHeader onClick={() => toggleSection(month)}>
            <div className="d-flex justify-content-between align-items-center">
              <h5 className="mb-0">{month}</h5>
              <span>
                {groupedEvents[month].length === 1
                  ? "1 event"
                  : `${groupedEvents[month].length} events`}
              </span>
              <span>
                {openSections[month] ? <BiChevronUp /> : <BiChevronDown />}
              </span>
            </div>
          </CardHeader>
          <Collapse isOpen={openSections[month]}>
            <CardBody>
              {groupedEvents[month].map((event) => (
                <EventCard event={event} key={event.id} />
              ))}
            </CardBody>
          </Collapse>
        </Card>
      ))}
    </CardColumns>
  );
}
