import React, { ComponentType, useEffect, useMemo } from "react";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import PageLoader from "../common/PageLoader";
import axios from "axios";
import UserDTO from "@/models/UserDTO";

interface AuthenticationGuardProps {
  component: ComponentType<any>;
}

const AuthenticationGuard: React.FC<AuthenticationGuardProps> = ({
  component,
}) => {
  const { isAuthenticated, isLoading, user, getAccessTokenSilently } =
    useAuth0();

  // Sends user data to backend
  useEffect(() => {
    let isMounted = true; // This flag denotes if the component is mounted

    const sendUserDataToAPI = async () => {
      if (!isAuthenticated || !user) {
        return;
      }

      try {
        const accessToken = await getAccessTokenSilently();
        if (isMounted) {
          // Only proceed if the component is still mounted

          const userDTO: UserDTO = {
            email: user.email,
          };
          await axios.post("api/user", userDTO, {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${accessToken}`,
            },
          });
        }
        // User data updated on backend
      } catch (error) {
        // Handle or log error
        console.error(error);
      }
    };

    sendUserDataToAPI();

    return () => {
      isMounted = false; // Set it to false when the component unmounts
    };
  }, [isAuthenticated, getAccessTokenSilently]); // Removed `user` from dependencies to prevent re-runs when user data changes

  const ComponentWithAuth = useMemo(
    () =>
      withAuthenticationRequired(component, {
        onRedirecting: () => <PageLoader />,
        returnTo: "/events",
      }),
    [component]
  );

  if (isLoading) {
    return <PageLoader />;
  }

  return <ComponentWithAuth />;
};

export default AuthenticationGuard;
