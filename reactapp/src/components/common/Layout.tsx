import RouteModel from "@/models/RouteModel";
import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import { Navbar, NavbarBrand, Nav, NavItem } from "reactstrap";
import ThemeToggler from "./ThemeToggler";
import { useThemeStore } from "@/state/themeStore";
import CreateEventModal from "../events/CreateEventModal";
import BottomNavbar from "./nav/BottomNavbar";
import { useAuth0 } from "@auth0/auth0-react";
import TopNavbar from "./nav/TopNavbar";

export default function Layout() {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const { isAuthenticated } = useAuth0();

  return (
    <div className="d-flex flex-column" style={{ height: "100vh" }}>
      {/* Top Navbar */}
      {isAuthenticated && <TopNavbar />}
      {/* Main content */}
      <div className="flex-grow-1 mb-5">
        <Outlet />
      </div>
      {/* Bottom Navbar */}
      {isAuthenticated && <BottomNavbar setIsModalOpen={setIsModalOpen} />}
      <CreateEventModal isOpen={isModalOpen} setModal={setIsModalOpen} />
    </div>
  );
}
