import React from "react";
import { Navbar, Nav, NavItem } from "reactstrap";
import ThemeToggler from "../ThemeToggler";
import { useThemeStore } from "@/state/themeStore";
import {
  BsPerson,
  BsPersonBadge,
  BsPersonCircle,
  BsPersonFill,
} from "react-icons/bs";
import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

const TopNavbar = () => {
  const { user } = useAuth0();
  const { preferredColorScheme } = useThemeStore();

  return (
    <Navbar color={preferredColorScheme} light expand="lg">
      <NavLink to={"/profile"} className="ms-auto">
        <span>
          {user && user.picture ? (
            <img
              src={user.picture}
              alt="user avatar"
              style={{ height: "2rem", borderRadius: "50%" }}
            />
          ) : (
            <BsPersonCircle size={"2rem"} />
          )}
        </span>
      </NavLink>
    </Navbar>
  );
};

export default TopNavbar;
