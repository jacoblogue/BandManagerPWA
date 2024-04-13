import React from "react";
import { Table } from "reactstrap";
import { mockSongs } from "../../../mockData/mockSongs";

export default function SongList() {
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
          {mockSongs.map((song) => (
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
