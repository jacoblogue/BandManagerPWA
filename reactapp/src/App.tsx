import React, { useEffect } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./components/common/Layout";
import RouteModel from "./models/RouteModel";
import EventPage from "./components/events/EventPage";
import * as signalR from "@microsoft/signalr";
import { useEventStore } from "./state/eventStore";
import axios from "axios";
import ExistingEventModel from "./models/ExistingEventModel";

const routes: RouteModel[] = [
  // {
  //   path: "/",
  //   element: <Home />,
  //   exact: true,
  //   name: "Home",
  // },
  {
    path: "/",
    element: <EventPage />,
    name: "Events",
  },
];

export default function App() {
  const { replaceEvents } = useEventStore();

  useEffect(() => {
    let connection: signalR.HubConnection;

    const startConnection = async () => {
      try {
        connection = new signalR.HubConnectionBuilder()
          .withUrl("/eventHub", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
          })
          .withAutomaticReconnect()
          .build();

        connection.on("ReceiveEventUpdate", (message) => {
          console.log("Received a SignalR message: ", message);
          axios.get<ExistingEventModel[]>(`/api/event`).then((response) => {
            replaceEvents(response.data);
          });
        });

        await connection.start();
        console.log("SignalR Connected.");
      } catch (err) {
        console.log("Error while establishing connection:", err);
        setTimeout(() => startConnection(), 5000);
      }
    };

    startConnection();

    return () => {
      if (connection) {
        connection.stop();
      }
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
