import RouteModel from "@/models/RouteModel";
import React, { useState } from "react";
import { Outlet, NavLink } from "react-router-dom";
import { Navbar, NavbarBrand, Nav, NavItem, Button } from "reactstrap";
import ThemeToggler from "./ThemeToggler";
import { useThemeStore } from "@/state/themeStore";
import { BiCalendar, BiHome, BiPlusCircle } from "react-icons/bi";
import CreateEventModal from "../events/CreateEventModal";

interface Props {
  routes: RouteModel[];
}

export default function Layout({ routes }: Props) {
  const { preferredColorScheme } = useThemeStore();
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  return (
    <div className="d-flex flex-column" style={{ height: "100vh" }}>
      {/* Top Navbar */}
      <Navbar color={preferredColorScheme} light expand="lg">
        <NavbarBrand href="/">BM</NavbarBrand>
        <Nav className="me-auto" navbar>
          {routes.map((route) => (
            <NavItem key={route.name}></NavItem>
          ))}
          <NavItem>
            <ThemeToggler />
          </NavItem>
        </Nav>
      </Navbar>

      {/* Main content */}
      <div className="flex-grow-1 mb-5">
        <Outlet />
      </div>

      {/* Bottom Navbar */}
      <Navbar
        className="border-top"
        fixed="bottom"
        color={preferredColorScheme}
        expand="lg"
      >
        <Nav className="w-100 d-flex justify-content-around ">
          <NavItem className="">
            <NavLink to="/" className="nav-link disabled">
              <BiHome size={"1.4rem"} />
            </NavLink>
          </NavItem>
          <NavItem className="">
            <Button
              color={preferredColorScheme}
              onClick={() => setIsModalOpen(true)}
            >
              <BiPlusCircle size={"1.6rem"} />
            </Button>
          </NavItem>
          <NavItem>
            <NavLink
              to="/calendar"
              className={({ isActive }) =>
                isActive ? "nav-link disabled" : "nav-link disabled"
              }
            >
              <BiCalendar size={"1.4rem"} />
            </NavLink>
          </NavItem>
        </Nav>
      </Navbar>
      <CreateEventModal isOpen={isModalOpen} setModal={setIsModalOpen} />
    </div>
  );
}
