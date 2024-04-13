import React from "react";
import { Collapse, Nav, NavItem, Navbar, NavbarToggler } from "reactstrap";
import { useThemeStore } from "@/state/themeStore";
import {
  BsArrowBarRight,
  BsArrowRight,
  BsMenuApp,
  BsMenuButton,
  BsPersonCircle,
} from "react-icons/bs";
import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Sidebar from "./Sidebar";
import styles from "./TopNavbar.module.scss";

const TopNavbar = () => {
  const { user } = useAuth0();
  const { preferredColorScheme } = useThemeStore();
  const [collapsed, setCollapsed] = React.useState(true);
  const linkColor = preferredColorScheme === "dark" ? "light" : "dark";

  const toggleSidebar = () => setCollapsed(!collapsed);

  return (
    <React.Fragment>
      <Navbar color={preferredColorScheme} expand="lg">
        {/* <NavbarToggler onClick={toggleSidebar}>
          <BsArrowBarRight />
        </NavbarToggler> */}
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
      {/* <Sidebar setCollapsed={setCollapsed} collapsed={collapsed} /> */}
    </React.Fragment>
  );
};

export default TopNavbar;
