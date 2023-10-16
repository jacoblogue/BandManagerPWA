import RouteModel from "@/models/RouteModel";
import React, { useEffect } from "react";
import { Outlet, NavLink } from "react-router-dom";
import {
  Navbar,
  NavbarBrand,
  NavbarToggler,
  Collapse,
  Nav,
  NavItem,
} from "reactstrap";
import ThemeToggler from "./ThemeToggler";

interface Props {
  routes: RouteModel[];
}

export default function Layout({ routes }: Props) {
  const [collapsed, setCollapsed] = React.useState(true);

  const toggleNavbar = () => setCollapsed(!collapsed);
  return (
    <div>
      {/* Navbar */}
      <Navbar expand="lg" className="d-flex justify-content-between">
        <NavbarBrand href="/">
          <img
            src="src\assets\BandManagerLogo.svg"
            alt="BandManager logo"
            width={"180"}
            height={"40"}
            style={{
              display: "inline-block",
              marginRight: "5px",
              verticalAlign: "middle",
            }}
          />
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} />
        <Collapse isOpen={!collapsed} navbar>
          <Nav className="mr-auto" navbar>
            {routes.map((route) => (
              <NavItem key={route.path}>
                <NavLink
                  to={route.path}
                  onClick={() => setCollapsed(!collapsed)}
                  className={"nav-link"}
                >
                  {route.name}
                </NavLink>
              </NavItem>
            ))}
            <NavItem>
              <ThemeToggler />
            </NavItem>
          </Nav>
        </Collapse>
      </Navbar>
      {/* Main content */}
      <div className="mt-5">{<Outlet />}</div>
    </div>
  );
}
