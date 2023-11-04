import React from "react";
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
  return (
    <Offcanvas
      style={{ maxWidth: "40%" }}
      isOpen={!collapsed}
      toggle={() => setCollapsed(!collapsed)}
    >
      <OffcanvasHeader>Navigate</OffcanvasHeader>
      <OffcanvasBody>
        <ListGroup flush>
          <ListGroupItem>Item 1</ListGroupItem>
        </ListGroup>
      </OffcanvasBody>
    </Offcanvas>
  );
};

export default Sidebar;
