import React, { useEffect } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./components/common/Layout";
import RouteModel from "./models/RouteModel";
import Home from "./components/home/Home";
import EventPage from "./components/events/EventPage";
import * as signalR from "@microsoft/signalr";
import { useEventStore } from "./state/eventStore";
import axios from "axios";
import { replace } from "formik";
import ExistingEventModel from "./models/ExistingEventModel";

const routes: RouteModel[] = [
  {
    path: "/",
    element: <Home />,
    exact: true,
    name: "Home",
  },
  {
    path: "/events",
    element: <EventPage />,
    name: "Events",
  },
];

export default function App() {
  const { replaceEvents } = useEventStore();
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/eventHub")
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveEventUpdate", (message) => {
      console.log("Received a SignalR message: ", message);
      axios.get<ExistingEventModel[]>(`/api/event`).then((response) => {
        replaceEvents(response.data);
      });
    });

    connection.start().catch((err) => console.log(err));

    return () => {
      connection.stop();
    };
  }, []);

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
