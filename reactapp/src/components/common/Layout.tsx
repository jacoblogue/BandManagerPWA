import RouteModel from "@/models/RouteModel";
import React, { useState } from "react";
import { Outlet, NavLink } from "react-router-dom";
import { Navbar, NavbarBrand, Nav, NavItem, Button } from "reactstrap";
import ThemeToggler from "./ThemeToggler";
import { useThemeStore } from "@/state/themeStore";
import { BiPlus } from "react-icons/bi";

interface Props {
  routes: RouteModel[];
}

export default function Layout({ routes }: Props) {
  const { preferredColorScheme } = useThemeStore();

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
            <NavLink to="/home" className="nav-link disabled">
              Home
            </NavLink>
          </NavItem>
          <NavItem className="">
            {/* Assuming you have some logic to handle adding new events, 
            you can attach that to the onClick handler of this button */}
            <Button
              color={preferredColorScheme}
              onClick={() => {
                /* Handle add new event */
              }}
            >
              <BiPlus size={"1.6rem"} />
            </Button>
          </NavItem>
          <NavItem>
            <NavLink // change to "/events
              to="/"
              className={({ isActive }) =>
                isActive ? "nav-link disabled" : "nav-link"
              }
            >
              Events
            </NavLink>
          </NavItem>
        </Nav>
      </Navbar>
    </div>
  );
}
