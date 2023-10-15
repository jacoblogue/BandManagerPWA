import React, { useEffect, useState } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./components/common/Layout";
import RouteModel from "./models/RouteModel";
import EventPage from "./components/events/EventPage";
import * as signalR from "@microsoft/signalr";
import { useEventStore } from "./state/eventStore";
import { useThemeStore } from "./state/themeStore";
import axios from "axios";
import ExistingEventModel from "./models/ExistingEventModel";
import SignalRMessage from "./models/SignalRMessage";

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
  const { replaceEvents, deleteEvent } = useEventStore();
  const { setPreferredColorScheme, preferredColorScheme } = useThemeStore();

  console.log("App rendered.");

  /**
   * This `useEffect` hook establishes a SignalR connection to the server and listens for updates to events.
   * When an update is received, it sends a GET request to the server to retrieve the updated list of events
   * and replaces the existing list in the event store with the new list.
   * The connection is automatically re-established if it is lost.
   */
  useEffect(() => {
    let connection: signalR.HubConnection;

    const startConnection = async () => {
      try {
        connection = new signalR.HubConnectionBuilder()
          .withUrl("/eventHub", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
          })
          .withAutomaticReconnect([0, 1000, 5000, 10000, 30000])
          .build();

        connection.on("ReceiveEventUpdate", (message: SignalRMessage) => {
          console.log("Received a SignalR message: ", message);
          if (message && message.type === "eventDeleted" && message.eventId) {
            deleteEvent(message.eventId);
          } else {
            axios.get<ExistingEventModel[]>(`/api/event`).then((response) => {
              replaceEvents(response.data);
            });
          }
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

  useEffect(() => {
    // Initialize and update Zustand store based on media query
    const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");
    const updateColorScheme = () => {
      setPreferredColorScheme(mediaQuery.matches ? "dark" : "light");
    };

    // Initial setting
    updateColorScheme();

    // Listen for changes
    mediaQuery.addEventListener("change", (e) => updateColorScheme());

    return () => {
      mediaQuery.removeEventListener("change", (e) => updateColorScheme());
    };
  }, [setPreferredColorScheme]);

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
