import React, { useEffect } from "react";
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
import { MessageTypeEnum } from "./models/MessageTypeEnum";
import Home from "./components/home/Home";
import { Auth0Provider, useAuth0 } from "@auth0/auth0-react";
import CallbackPage from "./components/auth/CallbackPage";
import Auth0ProviderWithNavigate from "./components/auth/Auth0ProviderWithNavigate";

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
  {
    path: "/callback",
    element: <CallbackPage />,
    name: "Callback",
  },
];

export default function App() {
  const { replaceEvents, deleteEvent, addEvent } = useEventStore();
  const { setPreferredColorScheme, preferredColorScheme } = useThemeStore();
  const { getAccessTokenSilently } = useAuth0();
  const apiAudience = import.meta.env.VITE_API_AUDIENCE;
  const domain = import.meta.env.VITE_AUTH0_DOMAIN;
  const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID;
  const redirectUri = import.meta.env.VITE_AUTH0_CALLBACK_URL;

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
          if (message && message.type === MessageTypeEnum.DeleteEvent) {
            deleteEvent(message.eventId);
          } else if (message && message.type === MessageTypeEnum.AddEvent) {
            addEvent(message.event);
          } else {
            getAccessTokenSilently({
              authorizationParams: {
                audience: apiAudience,
                scope: "read:events",
              },
            })
              .then(async (accessToken) => {
                try {
                  const response = await axios.get<ExistingEventModel[]>(
                    `/api/event`,
                    {
                      headers: {
                        Authorization: `Bearer ${accessToken}`,
                      },
                    }
                  );
                  replaceEvents(response.data);
                } catch (err) {
                  console.error(err);
                }
              })
              .catch((error) => {
                console.error(error);
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

  /**
   * This `useEffect` hook listens for changes to the preferred color scheme and updates the Zustand store accordingly.
   */
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
      <Auth0ProviderWithNavigate>
        <Routes>
          <Route element={<Layout routes={routes} />}>
            {routes.map((route) => (
              <Route
                key={route.path}
                path={route.path}
                element={route.element}
              />
            ))}
          </Route>
        </Routes>
      </Auth0ProviderWithNavigate>
    </BrowserRouter>
  );
}
