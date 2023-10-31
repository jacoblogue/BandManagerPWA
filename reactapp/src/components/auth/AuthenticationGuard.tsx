import React, { ComponentType, useMemo } from "react";
import { withAuthenticationRequired } from "@auth0/auth0-react";
import PageLoader from "../common/PageLoader";

interface AuthenticationGuardProps {
  component: ComponentType<any>; // Replace 'any' with the actual props if known
}

export default function AuthenticationGuard({
  component,
}: AuthenticationGuardProps) {
  const Component = useMemo(
    () =>
      withAuthenticationRequired(component, {
        onRedirecting: () => <PageLoader />,
      }),
    [component]
  );

  return <Component />;
}
