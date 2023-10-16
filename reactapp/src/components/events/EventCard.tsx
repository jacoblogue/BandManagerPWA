import ExistingEventModel from "@/models/ExistingEventModel";
import { useEventStore } from "@/state/eventStore";
import { formatDate, formatTime } from "@/utilities/dateUtils";
import axios from "axios";
import React, { useState } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Button,
  Dropdown,
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
} from "reactstrap";
import { BiDotsVertical } from "react-icons/bi";
interface Props {
  event: ExistingEventModel;
}

export default function EventCard({ event }: Props) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const [dropdownOpen, setDropdownOpen] = useState(false);

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const handleDelete = async (e: React.MouseEvent<HTMLElement, MouseEvent>) => {
    e.preventDefault();
    e.stopPropagation();
    await axios.delete(`/api/event/${event.id}`);
  };

  return (
    <Card className="">
      <CardHeader style={{ cursor: "pointer" }} onClick={toggleCollapse}>
        <span className="d-flex justify-content-between align-items-center">
          <strong>
            {formatDate(event.date)} {formatTime(event.date)}
            <h3 className="fs-4 me-auto">{event.title}</h3>
          </strong>
          <div className="d-flex align-items-center">
            <Dropdown isOpen={dropdownOpen} toggle={toggleDropdown}>
              <DropdownToggle
                tag="span"
                data-toggle="dropdown"
                aria-expanded={dropdownOpen}
                onClick={(e) => e.stopPropagation()}
              >
                <BiDotsVertical size={"1.3rem"} />
              </DropdownToggle>
              <DropdownMenu>
                <DropdownItem onClick={(e) => handleDelete(e)}>
                  Delete Event
                </DropdownItem>
              </DropdownMenu>
            </Dropdown>
          </div>
        </span>
      </CardHeader>
      {!isCollapsed && (
        <CardBody>
          <p>
            <strong>Location:</strong> {event.location}
          </p>
          <p>
            <strong>Date:</strong>
            {event.date ? ` ${formatDate(event.date)}` : "No date specified"}
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
