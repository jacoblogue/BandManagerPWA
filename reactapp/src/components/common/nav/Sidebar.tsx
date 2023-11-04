import { useThemeStore } from "@/state/themeStore";
import React from "react";
import { NavLink } from "react-router-dom";
import {
  ListGroup,
  ListGroupItem,
  Offcanvas,
  OffcanvasBody,
  OffcanvasHeader,
} from "reactstrap";

interface Props {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
}

const Sidebar = ({ collapsed, setCollapsed }: Props) => {
  const { preferredColorScheme } = useThemeStore();
  const linkColor = preferredColorScheme === "dark" ? "light" : "dark";

  return (
    <Offcanvas
      style={{ maxWidth: "40%" }}
      isOpen={!collapsed}
      toggle={() => setCollapsed(!collapsed)}
    >
      <OffcanvasHeader>Navigate</OffcanvasHeader>
      <OffcanvasBody>
        <ListGroup flush>
          <ListGroupItem>
            <NavLink
              onClick={() => setCollapsed(!collapsed)}
              className={`text-${linkColor}`}
              to={"/admin"}
            >
              Admin
            </NavLink>
          </ListGroupItem>
        </ListGroup>
      </OffcanvasBody>
    </Offcanvas>
  );
};

export default Sidebar;
