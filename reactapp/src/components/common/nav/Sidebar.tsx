import { useThemeStore } from "@/state/themeStore";
import React from "react";
import { BsArrowBarLeft } from "react-icons/bs";
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
      <OffcanvasHeader toggle={() => setCollapsed(!collapsed)}>
        Navigate
      </OffcanvasHeader>
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
          <ListGroupItem>
            <NavLink
              onClick={() => setCollapsed(!collapsed)}
              to={"/groups"}
              className={`text-${linkColor}`}
            >
              Groups
            </NavLink>
          </ListGroupItem>
        </ListGroup>
      </OffcanvasBody>
    </Offcanvas>
  );
};

export default Sidebar;
