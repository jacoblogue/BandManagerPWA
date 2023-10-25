import RouteModel from "@/models/RouteModel";
import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import { Navbar, NavbarBrand, Nav, NavItem } from "reactstrap";
import ThemeToggler from "./ThemeToggler";
import { useThemeStore } from "@/state/themeStore";
import CreateEventModal from "../events/CreateEventModal";
import BottomNavbar from "./nav/BottomNavbar";

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
        <NavbarBrand>BM</NavbarBrand>
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
      <BottomNavbar setIsModalOpen={setIsModalOpen} />
      <CreateEventModal isOpen={isModalOpen} setModal={setIsModalOpen} />
    </div>
  );
}
