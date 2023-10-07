/**
 * Format a Date object to a more readable date string.
 * @param date - The original Date object.
 * @param locale - The locale format to use, defaults to 'en-US'.
 * @param options - Additional formatting options.
 * @returns The formatted date string.
 */
export function formatDate(
  date: Date,
  locale: string = "en-US",
  options: Intl.DateTimeFormatOptions = {}
): string {
  console.log("date", date);
  return new Date(date).toLocaleDateString(locale, {
    year: "numeric",
    month: "long",
    day: "numeric",
    ...options,
  });
}

/**
 * Format a Date object to a more readable time string.
 * @param date - The original Date object.
 * @param locale - The locale format to use, defaults to 'en-US'.
 * @param options - Additional formatting options.
 * @returns The formatted time string.
 */
export function formatTime(
  date: Date,
  locale: string = "en-US",
  options: Intl.DateTimeFormatOptions = {}
): string {
  return new Date(date).toLocaleTimeString(locale, {
    hour: "numeric",
    minute: "numeric",
    ...options,
  });
}
