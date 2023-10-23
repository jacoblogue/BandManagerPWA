import React, { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import UserMetadata from "@/models/UserMetadata";

export default function Profile() {
  const { user, isAuthenticated, isLoading, getAccessTokenSilently } =
    useAuth0();
  const [userMetadata, setUserMetadata] = React.useState<UserMetadata | null>(
    null
  );

  // useEffect(() => {
  //   const getUserMetadata = async () => {
  //     const domain = "dev-f0jsmjla2lggonlk.us.auth0.com";
  //     console.log(domain);
  //     try {
  //       // const accessToken = await getAccessTokenSilently({
  //       //   authorizationParams: {
  //       //     audience: `https://${domain}/api/v2/`,
  //       //     scope: "read:current_user",
  //       //   },
  //       // });

  //       const userDetailsByIdUrl = `https://${domain}/api/v2/users/${
  //         user!.sub
  //       }`;

  //       // const metadataResponse = await fetch(userDetailsByIdUrl, {
  //       //   headers: {
  //       //     Authorization: `Bearer ${accessToken}`,
  //       //   },
  //       // });

  //       const { user_metadata } = await metadataResponse.json();

  //       console.log("User MetaData:", user_metadata);
  //       setUserMetadata(user_metadata);
  //     } catch (e: any) {
  //       console.log(e);
  //     }
  //   };

  //   getUserMetadata();
  // }, [getAccessTokenSilently, user?.sub]);

  if (isLoading) {
    return <div>Loading ...</div>;
  }

  return (
    isAuthenticated &&
    user && (
      <div>
        <img src={user.picture} alt={user.name} />
        <h2>{user.name}</h2>
        <p>{user.email}</p>
        <h3>User Metadata</h3>
        {userMetadata ? (
          <pre>{JSON.stringify(userMetadata, null, 2)}</pre>
        ) : (
          "No user metadata defined"
        )}
      </div>
    )
  );
}
