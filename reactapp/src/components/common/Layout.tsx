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
  return (
    <div>
      {/* Navbar */}
      <Navbar color="light" light expand="lg">
        <NavbarBrand href="#">Band Manager</NavbarBrand>
        <NavbarToggler />
        <Collapse navbar>
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
