import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export enum Theme {
  DARK = "dark",
  LIGHT = "light",
  SYSTEM = "system",
}

export function scrollToTop() {
  window.scrollTo(0, 0);
}
