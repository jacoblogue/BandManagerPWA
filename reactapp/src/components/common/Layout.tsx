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

export default function Layout() {
  return (
    <div>
      {/* Navbar */}
      <Navbar color="light" light expand="lg">
        <NavbarBrand href="#">Band Manager</NavbarBrand>
        <NavbarToggler />
        <Collapse navbar>
          <Nav className="mr-auto" navbar>
            <NavItem>
              <NavLink to={"/"} className={"nav-link"}>
                Home
              </NavLink>
            </NavItem>
          </Nav>
        </Collapse>
      </Navbar>
      {/* Main content */}
      <div className="container mt-5">{<Outlet />}</div>
    </div>
  );
}
