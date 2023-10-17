import React from "react";
import { useThemeStore } from "@/state/themeStore";
interface Props {
  title: string;
  subtitle?: string;
}

export default function StickyHeader({ title, subtitle }: Props) {
  const { preferredColorScheme } = useThemeStore();

  const bgColor = preferredColorScheme === "dark" ? "bg-dark" : "bg-light";
  return (
    <div
      className={`sticky-top ${bgColor} opacity-100 py-2 mb-2 d-flex justify-content-between align-items-center border-bottom`}
    >
      <h5 className="mb-0">{title}</h5>
      {subtitle && <span>{subtitle}</span>}
    </div>
  );
}
