import { useThemeStore } from "@/state/themeStore";
import React from "react";
import {
  BiPlusCircle,
  BiCalendar,
  BiCalendarEvent,
  BiCalendarPlus,
} from "react-icons/bi";
import { NavLink } from "react-router-dom";
import { Navbar, Nav, NavItem, Button } from "reactstrap";
import styles from "./BottomNavbar.module.css";

interface Props {
  setIsModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function BottomNavbar({ setIsModalOpen }: Props) {
  const { preferredColorScheme } = useThemeStore();
  const currentLocation = window.location.pathname;

  return (
    <Navbar
      className="border-top p-0"
      fixed="bottom"
      color={preferredColorScheme}
      expand="lg"
    >
      <Nav className="w-100 d-flex justify-content-around ">
        <NavItem className="">
          <NavLink to="/events" className="nav-link">
            <span className="d-flex flex-column justify-content-center align-items-center">
              <BiCalendarEvent size={"1.6rem"} />
              Events
            </span>
          </NavLink>
        </NavItem>
        <NavItem className="">
          <Button
            className={`${styles.newEventButton} text-${
              preferredColorScheme === "dark" ? "light" : "dark"
            }`}
            color={"link"}
            onClick={() => setIsModalOpen(true)}
          >
            <span className="d-flex flex-column justify-content-center align-items-center">
              <BiCalendarPlus size={"1.6rem"} />
              New Event
            </span>
          </Button>
          {/* icon for event */}
        </NavItem>
        <NavItem>
          <NavLink
            to="/calendar"
            className={({ isActive }) =>
              isActive ? "nav-link disabled" : "nav-link disabled"
            }
          >
            <span className="d-flex flex-column justify-content-center align-items-center">
              <BiCalendar size={"1.6rem"} />
              Calendar
            </span>
          </NavLink>
        </NavItem>
      </Nav>
    </Navbar>
  );
}
