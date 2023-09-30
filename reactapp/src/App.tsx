import React, { Component } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./components/common/Layout";
import EventList from "./components/eventList/EventList";
import RouteModel from "./models/RouteModel";
import Home from "./components/home/home";

const routes: RouteModel[] = [
  {
    path: "/",
    element: <Home />,
    exact: true,
    name: "Home",
  },
  {
    path: "/events",
    element: <EventList />,
    name: "Events",
  },
];

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout routes={routes} />}>
          {routes.map((route) => (
            <Route key={route.path} path={route.path} element={route.element} />
          ))}
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
