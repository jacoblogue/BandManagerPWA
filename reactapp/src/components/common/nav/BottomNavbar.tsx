import { useThemeStore } from "@/state/themeStore";
import React from "react";
import { BiHome, BiPlusCircle, BiCalendar } from "react-icons/bi";
import { NavLink } from "react-router-dom";
import { Navbar, Nav, NavItem, Button } from "reactstrap";

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
          <NavLink to="/" className="nav-link">
            <span className="d-flex flex-column justify-content-center align-items-center">
              <BiHome size={"1.6rem"} />
              Home
            </span>
          </NavLink>
        </NavItem>
        <NavItem className="">
          <Button
            color={preferredColorScheme}
            onClick={() => setIsModalOpen(true)}
          >
            <span className="d-flex flex-column justify-content-center align-items-center">
              <BiPlusCircle size={"1.6rem"} />
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
              Events
            </span>
          </NavLink>
        </NavItem>
      </Nav>
    </Navbar>
  );
}
