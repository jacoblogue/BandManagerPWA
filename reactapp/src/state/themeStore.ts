import { create } from "zustand";

// Define the state shape and actions
type ThemeState = {
  preferredColorScheme: string;
  setPreferredColorScheme: (scheme: string) => void;
};

// Initialize Zustand store
export const useThemeStore = create<ThemeState>((set) => ({
  preferredColorScheme: "light",
  setPreferredColorScheme: (scheme) => {
    set({ preferredColorScheme: scheme });
    document.documentElement.setAttribute("data-bs-theme", scheme);
  },
}));
