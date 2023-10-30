import React from "react";
import { Spinner } from "reactstrap";

export default function PageLoader() {
  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <Spinner color="primary" />
    </div>
  );
}
