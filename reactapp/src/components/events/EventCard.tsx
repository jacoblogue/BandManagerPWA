import ExistingEventModel from "@/models/ExistingEventModel";
import { formatDate, formatTime } from "@/utilities/dateUtils";
import axios from "axios";
import React, { useState } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Dropdown,
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
} from "reactstrap";
import { BiDotsVertical } from "react-icons/bi";
import { useAuth0 } from "@auth0/auth0-react";
interface Props {
  event: ExistingEventModel;
}

export default function EventCard({ event }: Props) {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const { getAccessTokenSilently, loginWithRedirect, getAccessTokenWithPopup } =
    useAuth0();
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const audience = import.meta.env.VITE_API_AUDIENCE;

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const handleDelete = async (e: React.MouseEvent<HTMLElement, MouseEvent>) => {
    e.preventDefault();
    e.stopPropagation();
    try {
      const accessToken = await getAccessTokenSilently({
        authorizationParams: {
          audience: audience,
          scope: "delete:events",
        },
      });

      await axios.delete(`/api/event/${event.id}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
    } catch (e: any) {
      console.error(e);
    }
  };

  return (
    <Card className="mb-2">
      <CardHeader style={{ cursor: "pointer" }} onClick={toggleCollapse}>
        <span className="d-flex justify-content-between align-items-center">
          <strong>
            {formatDate(event.date)} {formatTime(event.date)}
            <h3 className="fs-4 me-auto">{event.title}</h3>
            <h4 className="fs-5">{event.location}</h4>
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
