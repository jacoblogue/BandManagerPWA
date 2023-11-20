import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";
import React, { useEffect } from "react";

const AdminPage = () => {
  const domain = import.meta.env.VITE_AUTH0_DOMAIN;
  const { getAccessTokenSilently, user } = useAuth0();

  const fetchUsers = async () => {
    await axios.get("/api/users", {
      headers: {
        Authorization: `Bearer ${await getAccessTokenSilently()}`,
      },
    });
  };

  return (
    <div>
      <h1>Admin Page</h1>
    </div>
  );
};

export default AdminPage;
