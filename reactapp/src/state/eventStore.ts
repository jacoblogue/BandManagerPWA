import ExistingEventModel from "@/models/ExistingEventModel";
import { create } from "zustand";

type EventStore = {
  events: ExistingEventModel[];
  addEvent: (event: ExistingEventModel) => void;
  deleteEvent: (eventId: string) => void;
  replaceEvents: (newEvents: ExistingEventModel[]) => void;
};

export const useEventStore = create<EventStore>()((set) => ({
  events: [],
  addEvent: (newEvent) => {
    set((state) => {
      // Check for duplicate
      if (state.events.some((event) => event.id === newEvent.id)) {
        console.warn(`Event with id ${newEvent.id} already exists.`);
        return state;
      }

      return {
        ...state,
        events: [...state.events, newEvent],
      };
    });
  },
  deleteEvent: (eventId: string) =>
    set((state) => ({
      events: state.events.filter((e) => e.id !== eventId),
    })),
  replaceEvents: (newEvents: ExistingEventModel[]) => {
    set({ events: newEvents });
  },
}));
