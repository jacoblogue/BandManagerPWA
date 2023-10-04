import RouteModel from "@/models/RouteModel";
import React, { ReactNode } from "react";
import { Outlet, NavLink } from "react-router-dom";
import {
  Navbar,
  NavbarBrand,
  NavbarToggler,
  Collapse,
  Nav,
  NavItem,
} from "reactstrap";

interface Props {
  routes: RouteModel[];
}

export default function Layout({ routes }: Props) {
  const [collapsed, setCollapsed] = React.useState(true);

  const toggleNavbar = () => setCollapsed(!collapsed);
  return (
    <div>
      {/* Navbar */}
      <Navbar color="light" light expand="lg">
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
                <NavLink to={route.path} className={"nav-link"}>
                  {route.name}
                </NavLink>
              </NavItem>
            ))}
          </Nav>
        </Collapse>
      </Navbar>
      {/* Main content */}
      <div className="container mt-5">{<Outlet />}</div>
    </div>
  );
}
