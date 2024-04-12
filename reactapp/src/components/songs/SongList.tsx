import React from "react";
import { Table } from "reactstrap";

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
          <tr>
            <td>song1</td>
            <td>artist1</td>
            <td>key1</td>
          </tr>
          <tr>
            <td>song2</td>
            <td>artist2</td>
            <td>key2</td>
          </tr>
        </tbody>
      </Table>
    </div>
  );
}
