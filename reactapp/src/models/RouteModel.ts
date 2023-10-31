import React, { ComponentType } from "react";

export default interface RouteModel {
  path: string;
  name: string;
  element: ComponentType<any>;
  exact?: boolean;
}
