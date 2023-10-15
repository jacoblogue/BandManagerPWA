import React from "react";
import { Button } from "reactstrap";
import { BiSolidMoon, BiSolidSun } from "react-icons/bi"; // Importing the sun and moon icons from Bootstrap icons
import { useThemeStore } from "../../state/themeStore";

export default function ThemeToggler() {
  const { preferredColorScheme, setPreferredColorScheme } = useThemeStore();

  const toggleTheme = () => {
    const newTheme = preferredColorScheme === "light" ? "dark" : "light";
    setPreferredColorScheme(newTheme);
  };

  return (
    <Button className="d-flex align-items-center" onClick={toggleTheme}>
      {preferredColorScheme === "light" ? (
        <BiSolidMoon color="yellow" />
      ) : (
        <BiSolidSun color="yellow" />
      )}
    </Button>
  );
}
