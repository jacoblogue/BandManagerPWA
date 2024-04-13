import { api } from "@/utilities/api";
import { create } from "zustand";

type SongStoreState = {
  songs: any[];
  loading: boolean;
  error: Error | null;
  fetchSongs: (accessToken: string) => Promise<void>;
};

const useSongStore = create<SongStoreState>((set) => ({
  songs: [],
  loading: false,
  error: null,
  fetchSongs: async (accessToken) => {
    set({ loading: true, error: null });
    try {
      api
        .get("/song", {
          headers: { Authorization: `Bearer ${accessToken}` },
        })
        .then((response) => {
          set({ songs: response.data, loading: false });
          console.log(response.data);
        })
        .catch((error) => {
          set({ error, loading: false });
        });
    } catch (error: any) {
      set({ error, loading: false });
    }
  },
}));

export default useSongStore;
