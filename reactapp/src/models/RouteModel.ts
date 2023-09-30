import React from "react";

export default interface RouteModel {
  path: string;
  name: string;
  element: React.JSX.Element;
  exact?: boolean;
}
