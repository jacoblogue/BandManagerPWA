import React, { useEffect } from "react";
import { Table } from "reactstrap";
import { mockSongs } from "../../../mockData/mockSongs";
import { api } from "@/utilities/api";
import useSongStore from "@/state/songStore";
import { useAuth0 } from "@auth0/auth0-react";

export default function SongList() {
  const { songs, loading, error, fetchSongs } = useSongStore();
  const { getAccessTokenSilently, getAccessTokenWithPopup } = useAuth0();
  useEffect(() => {
    fetchAccessTokenAndSongs();
  }, []);

  const fetchAccessTokenAndSongs = async () => {
    const accessToken = await getAccessTokenSilently({
      authorizationParams: {
        audience: import.meta.env.VITE_API_AUDIENCE,
        scope: "read:songs",
      },
    });
    if (accessToken) {
      fetchSongs(accessToken);
    }
  };

  return (
    <div>
      <Table>
        <thead>
          <tr>
            <th>Title</th>
            <th>Artist</th>
            <th>Key</th>
          </tr>
        </thead>
        <tbody>
          {songs.map((song) => (
            <tr key={song.id}>
              <td>{song.title}</td>
              <td>{song.artist}</td>
              <td>{song.key}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
}
